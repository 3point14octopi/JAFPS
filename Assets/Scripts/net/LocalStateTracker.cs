using Character_Data;
using JAFPS_API;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class LocalStateTracker : MonoBehaviour, LocalObjectComponent
{
    public LocalObjectTracker subject;
    public short state = 1;
    public short prefabIndex;
    public string stateName;

    bool active;
    bool canSend;
    bool registered = false;
    Mutex activator;

    public short remoteHash;
    

    // Start is called before the first frame update
    void Start()
    {
        if (subject != null) subject.AddToNotify(this);
        active = false;
        canSend = false;
        activator = new Mutex();
        stateName = "";
    }

    public void RegisterForNet(short hash)
    {
        remoteHash = hash;
        registered = true;
        Debug.Log("State tracker registered");
    }

    public object[] SendData()
    {
        return new object[] { (short)1, prefabIndex, state };
    }

    public void StateUpdate(string update)
    {
        
        if (registered)
        {
            Debug.Log(update);
            RemoteAState_Manager.Instance.SendLocalUpdate(remoteHash, update, prefabIndex);
        }
        state = StateData.stateCodes[update];
        stateName = update;
    }

}
