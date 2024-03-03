using System;
using System.Threading;
using UnityEngine;
using JAFPS_API;

public class CTFUpdate : MonoBehaviour, LocalObjectComponent
{
    public Transform myRotation;
    public Vector3 prevEuler;
    public Rigidbody myVelocity;
    public Vector3 prevVelocity;
    public Vector3 prevLocation;
    bool canSend;
    bool active;
    Mutex activator;
    public short remoteHash;
    bool registered = false;
    float sinceLast = 0f;

    public LocalObjectTracker subject;

    bool thisFrame = false;
  
    // Start is called before the first frame update
    void Start()
    {
        if (subject != null) subject.AddToNotify(this);
        active = false;
        canSend = false;
        activator = new Mutex();

        prevVelocity = myVelocity.velocity;
        prevEuler = myRotation.eulerAngles;
        prevLocation = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(registered)sinceLast += Time.fixedDeltaTime;

        if (DirtyTransform())
        {
            prevVelocity = myVelocity.velocity;
            prevEuler = myRotation.eulerAngles;
            prevLocation = transform.position;
            
            if (CheckActive())
            {
                if (registered)
                {
                    RemoteTForm_Manager.Instance.SendLocalUpdate(remoteHash, prevVelocity, prevLocation, prevEuler);
                }
            }
            sinceLast = 0f;
        }else if (registered && DirtyPos())
        {
            prevLocation = transform.position;
            RemoteTForm_Manager.Instance.SendLocalUpdate(remoteHash, prevVelocity, prevLocation, prevEuler);
            sinceLast = 0f;
        }



    }

    private void Update()
    {
        if(!canSend && ClientRoot.isConnected)
        {
            ToggleActive(true);
        }
    }

    bool CheckActive()
    {
        bool retVal = false;
        if (activator.WaitOne(30))
        {
            retVal = active;
            activator.ReleaseMutex();
            return retVal;
        }
        return retVal;
    }

    public void ToggleActive(bool isActive)
    {
        if (activator.WaitOne())
        {
            active = isActive;
            activator.ReleaseMutex();
        }
    }

    bool DirtyTransform()
    {
        if (myVelocity.velocity != prevVelocity)
        {
            return true;
        } else if (myRotation.eulerAngles != prevEuler)
        {
            return true;
        }
        return false;
    }

    bool DirtyPos()
    {
        return (transform.position != prevLocation && sinceLast > 0.5f);
    }

    public void RegisterForNet(short hash)
    {
        remoteHash = hash;
        registered = true;
        Debug.Log("transform tracker registered");
    }

    public object[] SendData()
    {
        object[] ret = new object[5];
        ret[0] = (short)2;

        object[] vpe = RemoteTForm_Manager.Instance.ParseForInit(prevVelocity, prevLocation, prevEuler);
        for(int i = 0; i < 3; i++)
        {
            ret[i + 1] = vpe[i];
        }

        return ret;
    }
}
