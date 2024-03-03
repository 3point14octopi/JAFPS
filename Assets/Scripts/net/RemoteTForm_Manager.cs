using JAFPS_API;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using JAFBillboard;

public class RemoteTForm_Manager : MonoBehaviour
{
    public static RemoteTForm_Manager Instance { get; private set; }

    private Parser parser = new Parser();

    private Queue<object[]> commands = new Queue<object[]>();
    private Mutex queueTex = new Mutex();


    private Dictionary<short, RemoteTransformUpdate> remoteBodies = new Dictionary<short, RemoteTransformUpdate>();
    private Mutex rbMutex = new Mutex();

    
    short localKeys = 1;
    Mutex register = new Mutex();

    


    // Start is called before the first frame update

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
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (queueTex.WaitOne())
        {
            while (commands.Count > 0)
            {
                ParseCommand(commands.Dequeue());
            }
            queueTex.ReleaseMutex();
        }
    }

    private void ParseCommand(object[] command)
    {
        switch ((short)command[1])
        {
            case (CTFORM_EVENTCODES.REMOTE_UPDATE):
                Debug.Log("updating remote body");
                UpdateRemoteBody(command);
                break;
            default:
                Debug.Log("unrecognized subevent: " + command[1].ToString());
                break;
        }
    }

    public void UpdateRemoteBody(object[] tformData)
    {
        short key = (short)tformData[2];
        Debug.Log("body to update is " + key.ToString());
        if (rbMutex.WaitOne())
        {
            if (remoteBodies.ContainsKey(key))
            {
                float[] velData = (float[])tformData[3];
                float[] posData = (float[])tformData[4];
                float[] rotData = (float[])tformData[5];
            
                remoteBodies[key].TransformUpdate(velData, posData, rotData);
                rbMutex.ReleaseMutex();
            }
            else
            {
                Debug.Log("can't find body, will make a new one");
                rbMutex.ReleaseMutex();
                RemoteBodyManager.Instance.RegisterRemoteObject(tformData, false);
            }
        }
        
    }


    private float[] VtoArr(Vector3 vec)
    {
        float x, y, z;

        x = vec.x;
        y = vec.y;
        z = vec.z;

        return new float[] { x, y, z };
    }

    public object[] ParseForInit(Vector3 v, Vector3 p, Vector3 e)
    {
        object[] array = new object[3]
        {
            VtoArr(v), VtoArr(p), VtoArr(e)
        };
        return array;
    }

    public void SendLocalUpdate(short remoteHash, Vector3 v, Vector3 p, Vector3 e)
    {
        object[] array = new object[]
        {
            ClientRoot.serverID,
            JAFPS_EVENTCODES.EVENT_TRANSFORM,
            CTFORM_EVENTCODES.MY_UPDATE,
            remoteHash,
            VtoArr(v),
            VtoArr(p),
            VtoArr(e)
        };
        ClientRoot.SendToServer(parser.Convert(array));
    }

    public void AddAction(object[] command)
    {
        if (queueTex.WaitOne())
        {
            commands.Enqueue(command);
            queueTex.ReleaseMutex();
            Debug.Log("queued a transform action");
        }
    }

    public void AddOrSet(short code, RemoteTransformUpdate rt)
    {
        if (rbMutex.WaitOne())
        {
            if (remoteBodies.ContainsKey(code))
            {
                remoteBodies[code] = rt;
            }
            else
            {
                remoteBodies.Add(code, rt);
            }
            rbMutex.ReleaseMutex();
        }
    }

    public void RemoveFromList(short code)
    {
        if (rbMutex.WaitOne())
        {
            remoteBodies.Remove(code);
            rbMutex.ReleaseMutex();
        }
    }
}
