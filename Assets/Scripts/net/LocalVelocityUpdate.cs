using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalVelocityUpdate : MonoBehaviour
{
    public Rigidbody myBody;
    Vector3 myVelocity;
    Vector3 prevVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myVelocity = myBody.velocity;
        if (DirtyVelocity())
        {
            myVelocity = myBody.velocity;
            prevVelocity = myVelocity;
             
        }
    }

    bool DirtyVelocity()
    {
        return (myBody.velocity == prevVelocity);
    }
}
