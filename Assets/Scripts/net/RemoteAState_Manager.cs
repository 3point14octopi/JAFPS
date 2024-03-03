using Character_Data;
using JAFPS_API;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using UnityEngine;

public class RemoteAState_Manager : MonoBehaviour
{
    public static RemoteAState_Manager Instance;
    private Parser parser = new Parser();

    private Queue<object[]> commands = new Queue<object[]>();
    private Mutex queueTex = new Mutex();

    private Dictionary<short, RemoteCharacter>remoteControllers = new Dictionary<short, RemoteCharacter>();
    private Mutex rcMutex = new Mutex();


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
    

    // Update is called once per frame
    void Update()
    {
        if (queueTex.WaitOne())
        {
            while(commands.Count > 0)
            {
                ParseCommand(commands.Dequeue());
            }
            queueTex.ReleaseMutex();
        }
    }

    public void AddOrSet(short code, RemoteCharacter rc)
    {
        if (rcMutex.WaitOne())
        {
            if (remoteControllers.ContainsKey(code))
            {
                remoteControllers[code] = rc;
            }
            else
            {
                remoteControllers.Add(code, rc);
            }

            rcMutex.ReleaseMutex();
        }
    }

    public void RemoveFromList(short code)
    {
        if(rcMutex.WaitOne())
        {
            remoteControllers.Remove(code);
            rcMutex.ReleaseMutex();
        }
    }

    private void ParseCommand(object[] command)
    {
        Debug.Log("starting ostate command parse: " + ((short)command[1]).ToString());

        switch ((short)command[1])
        {
            case (OBJANIM_EVENTCODES.REMOTE_UPDATE):
                UpdateRemoteAnim(command);
                break;
            default:
                break;
        }
    }

    private void UpdateRemoteAnim(object[] data)
    {
        short key = (short)data[2];
        if (rcMutex.WaitOne())
        {
            Debug.Log("parsing anim update request");
            if (remoteControllers.ContainsKey(key))
            {
                Debug.Log("key found");
                remoteControllers[key].SetState((short)data[3]);
                rcMutex.ReleaseMutex();
            }
            else
            {
                Debug.Log("key not found");
                rcMutex.ReleaseMutex();
                RemoteBodyManager.Instance.RegisterRemoteObject(data, true);
            }
        }
    }

    public void SendLocalUpdate(short index, string code, short prefab)
    {
        short sCode = 0;
        sCode = StateData.stateCodes[code];
        Debug.Log(sCode);
        object[] array = new object[]
        {
            ClientRoot.serverID,
            JAFPS_EVENTCODES.EVENT_ANIM,
            OBJANIM_EVENTCODES.MY_UPDATE,
            index,
            StateData.stateCodes[code],
            prefab
        };

        ClientRoot.SendToServer(parser.Convert(array));
    }


    public void AddAction(object[] command)
    {
        if (queueTex.WaitOne())
        {
            commands.Enqueue(command);
            queueTex.ReleaseMutex();
        }
    }
}
