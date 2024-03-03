using System;
using System.Collections.Generic;
using System.Threading;
using JAFPS_API;
using JAFBillboard;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using UnityEngine.Pool;

public class RemoteBodyManager: MonoBehaviour
{
    public static RemoteBodyManager Instance;

    Dictionary<short, GameObject> remoteObjects = new Dictionary<short, GameObject>();
    Dictionary<short, GameObject> localObjects = new Dictionary<short, GameObject>();

    short localKey = 1;

    Mutex localObjectMutex = new Mutex();
    Mutex remoteObjectMutex = new Mutex();

    public List<GameObject> objectPrefabs = new List<GameObject>();
    private Parser parser = new Parser();

    private Queue<object[]> commandQueue = new Queue<object[]>();
    private Mutex queueTex = new Mutex();








    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        if (queueTex.WaitOne())
        {
            while(commandQueue.Count > 0)
            {
                ParseCommand(commandQueue.Dequeue());
            }
            queueTex.ReleaseMutex();
        }
    }

    void ParseCommand(object[] command)
    {
        switch ((short)command[1])
        {
            case (REMOTEOBJ_EVENTCODES.CONFIRM_INIT):
                FinishRegisterLocal((short)command[2], (short)command[3]);
                break;
            case (REMOTEOBJ_EVENTCODES.NEW_OBJ):
                RegisterRemoteObject(command, true, true);
                break;
            default:
                break;
        }
    }



    public void RegisterRemoteObject(object[] data, bool usingObjData = true, bool fullUpdate = false)
    {
        short key = (short)data[2];
        if (remoteObjectMutex.WaitOne())
        {
            if (usingObjData)
            {
                int prefabIndex = (fullUpdate) ? 3 : 4;
                int stateIndex = (fullUpdate) ? 4 : 3;
                GameObject obj = Instantiate(objectPrefabs[(short)data[prefabIndex]], new Vector3(0, -100, 0), Quaternion.identity);
                obj.GetComponent<RemoteCharacter>().SetState((short)data[stateIndex]);
                obj.GetComponent<Billboard>().activeCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

                if (fullUpdate)
                {

                    obj.GetComponent<RemoteTransformUpdate>().TransformUpdate((float[])data[5], (float[])data[6], (float[])data[7]);
                }
                else
                {
                    SendUpdateRequest(JAFPS_EVENTCODES.EVENT_TRANSFORM, key);
                }
                
                remoteObjects.Add(key, obj);

            }
            else
            {
                //iDEALly we would predict this based on like, velocity or some shit 
                //for now we're going to guess that it's the pig
                float[] pos = (float[])data[4];
                GameObject obj = Instantiate(objectPrefabs[0], new Vector3(pos[0], pos[1], pos[2]), Quaternion.identity);
                obj.GetComponent<RemoteCharacter>().SetState(0);
                obj.GetComponent<Billboard>().activeCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                obj.GetComponent<RemoteTransformUpdate>().TransformUpdate((float[])data[3], pos, (float[])data[5]);
                remoteObjects.Add(key, obj);

                SendUpdateRequest(JAFPS_EVENTCODES.EVENT_ANIM, key);
            }

            RemoteAState_Manager.Instance.AddOrSet(key, remoteObjects[key].GetComponent<RemoteCharacter>());
            RemoteTForm_Manager.Instance.AddOrSet(key, remoteObjects[key].GetComponent<RemoteTransformUpdate>());
            remoteObjectMutex.ReleaseMutex();
        }
        
    }

    public void UpdateRemotePrefab(object[] data)
    {
        short key = (short)data[2];
        GameObject obj = Instantiate(objectPrefabs[(short)data[3]], new Vector3(0, -100, 0), Quaternion.identity);
        obj.GetComponent<RemoteCharacter>().SetState((short)data[4]);
        obj.GetComponent<Billboard>().activeCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        if (remoteObjectMutex.WaitOne())
        {
            obj.GetComponent<Transform>().Equals(remoteObjects[key].transform);
            obj.GetComponent<RemoteTransformUpdate>().Equals(remoteObjects[key].GetComponent<RemoteTransformUpdate>());
            remoteObjects[key] = obj;

            RemoteAState_Manager.Instance.AddOrSet(key, remoteObjects[key].GetComponent<RemoteCharacter>());
            RemoteTForm_Manager.Instance.AddOrSet(key, remoteObjects[key].GetComponent<RemoteTransformUpdate>());
            remoteObjectMutex.ReleaseMutex();
        }
    }

    public void StartRegisterLocal(GameObject lob, object[] data)
    {
        Debug.Log("wow. the function of all time");
        if (localObjectMutex.WaitOne())
        {
            short k = localKey;
            localObjects.Add(k, lob);

            lob.GetComponent<LocalObjectTracker>().localHash = k;
            localKey++;

            
            //SEND REGISTRATION REQUEST
            object[] toSend = new object[data.Length + 4];
            toSend[0] = ClientRoot.serverID;
            toSend[1] = JAFPS_EVENTCODES.EVENT_OBJECT;
            toSend[2] = REMOTEOBJ_EVENTCODES.INIT_OBJECT;
            toSend[3] = k;

            for (int i = 0; i < data.Length; toSend[i + 4] = data[i], i++) ;
            ClientRoot.SendToServer(parser.Convert(toSend));


            localObjectMutex.ReleaseMutex();

        }
    }

    public void FinishRegisterLocal(short index, short remoteHash)
    {
        if (localObjectMutex.WaitOne())
        {
            localObjects[index].GetComponent<LocalObjectTracker>().Register(remoteHash);
        }
    }


    public bool HasRegisteredRemote(short key)
    {
        bool retVal = false;
        if (remoteObjectMutex.WaitOne())
        {
            retVal = remoteObjects.ContainsKey(key);
            remoteObjectMutex.ReleaseMutex();
        }
        return retVal;
    }


    

    public RemoteCharacter GetRemoteCharacter(short key)
    {
        return remoteObjects[key].GetComponent<RemoteCharacter>();
    }

    public void UpdateRemoteTransform(short key, float[]vel, float[] pos, float[] rot)
    {
        if (remoteObjectMutex.WaitOne()){
            remoteObjects[key].GetComponent<RemoteTransformUpdate>().TransformUpdate(vel, pos, rot);
            remoteObjectMutex.ReleaseMutex();
        }
        
    }

    public void UpdateRemoteAnim(short key, short code)
    {
        if (remoteObjectMutex.WaitOne())
        {
            remoteObjects[key].GetComponent<RemoteCharacter>().SetState(code);
            remoteObjectMutex.ReleaseMutex();
        }
    }


    void SendUpdateRequest(short typeOfUpdate, short key)
    {
        object[] request = new object[]
        {
            ClientRoot.serverID,
            typeOfUpdate,
            (short)3,
            key
        };
        ClientRoot.SendToServer(parser.Convert(request));
    }


    public void AddAction(object[] data)
    {
        if (queueTex.WaitOne())
        {
            commandQueue.Enqueue(data);
            queueTex.ReleaseMutex();
            Debug.Log("object action queued");
        }
    }
}
