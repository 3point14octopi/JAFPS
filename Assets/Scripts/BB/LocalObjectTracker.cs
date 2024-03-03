using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace JAFPS_API
{
    public interface LocalObjectComponent
    {
        void RegisterForNet(short hash);
        object[] SendData();
    }

    public interface LocalObjectComponentManager
    {
        void AddToNotify(LocalObjectComponent loc);
        void NotifyAll();
    }

    public class LocalObjectTracker : MonoBehaviour, LocalObjectComponentManager
    {
        public short localHash;
        public short remoteHash;

        private List<LocalObjectComponent> trackedComponents = new List<LocalObjectComponent>();

        bool active = false;
        bool canSend = false;
        bool pending = false;
        Mutex activator;

        // Start is called before the first frame update
        void Start()
        {
            activator = new Mutex();
           
        }

        // Update is called once per frame
        void Update()
        {
            if(!canSend && ClientRoot.isConnected)
            {
                if (!pending)
                {
                    pending = true;
                    RequestData();
                }
            }
        }

        public void ToggleActive(bool isActive)
        {
            if (activator.WaitOne())
            {
                active = isActive;
                activator.ReleaseMutex();
            }
        }

        public void Register(short hash)
        {
            remoteHash = hash;
            canSend = true;
            for (int i = 0; i < trackedComponents.Count; trackedComponents[i].RegisterForNet(hash), i++) ;
        }

        public void AddToNotify(LocalObjectComponent loc)
        {
            trackedComponents.Add(loc);
        }

        public void NotifyAll()
        {
            
        }

        public void RequestData()
        {
            object[] data = new object[5];
            for(int i = 0; i < trackedComponents.Count; i++)
            {
                object[] info = trackedComponents[i].SendData();

                switch ((short)info[0])
                {
                    case (1):
                        data[0] = (short)info[1];
                        data[1] = (short)info[2];
                        Debug.Log("appearance info found (" + i.ToString() + ")");
                        break;
                    case (2):
                        data[2] = (float[])info[1];
                        data[3] = (float[])info[2];
                        data[4] = (float[])info[3];
                        Debug.Log("tform info found (" + i.ToString() + ")");
                        break;
                    default:
                        Debug.Log("nothing found (" + i.ToString() + ")");
                        break;
                }
            }
            Debug.Log("sending local data to be registered");
            RemoteBodyManager.Instance.StartRegisterLocal(gameObject, data);
        }
    }
}

