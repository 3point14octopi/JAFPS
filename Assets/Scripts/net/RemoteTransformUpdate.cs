using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class RemoteTransformUpdate : MonoBehaviour
{
    
    public Transform tform;
    public Rigidbody body;
    public Vector3 posV;
    public Vector3 eulersV;
    public Vector3 velV;
    Vector3 prevPos;
    bool dirtyVel = false;
    public float timesum = 0;
    Vector3 movement = new Vector3();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dirtyVel)
        {
            if (transform.position != posV)
            {
                prevPos = transform.position;
                transform.position = posV;
            }
            if (transform.rotation.eulerAngles != eulersV) transform.eulerAngles = eulersV;


            //movement = (velV.x + velV.y +velV.z > 0)?velV * Time.fixedDeltaTime * 3:Vector3.zero;
            dirtyVel = false;
            timesum = 0f;
        }
        //this is NOT how LERP works :stare:
        transform.position = Vector3.LerpUnclamped(transform.position, transform.position + (velV), (0.008f));
        timesum += Time.deltaTime;
    }

    //private void FixedUpdate()
    //{
    //    if (dirtyVel)
    //    {
    //        if (transform.position != posV)
    //        {
    //            prevPos = transform.position;
    //            transform.position = posV;
    //        }
    //        if(transform.rotation.eulerAngles != eulersV)transform.eulerAngles = eulersV;

    //        body.velocity = velV;
            
    //        //movement = (velV.x + velV.y +velV.z > 0)?velV * Time.fixedDeltaTime * 3:Vector3.zero;
    //        dirtyVel = false;
    //        timesum = 0f;
    //    }
    //    //this is NOT how LERP works :stare:
    //    transform.position += Vector3.Lerp(posV, posV + velV, Time.fixedDeltaTime);
    //    timesum += Time.fixedDeltaTime;
    //    //if(timesum < 10) tform.position = Vector3.LerpUnclamped(posV, extrapolated, timesum);

    //    //timesum += Time.fixedDeltaTime;
    //}

    public void TransformUpdate(float[] vel, float[] pos, float[] rot)
    {
        Debug.Log("yippee.mp3");
        velV = new Vector3(vel[0], vel[1], vel[2]);
        
        posV = new Vector3(pos[0], pos[1], pos[2]);

        eulersV = new Vector3(rot[0], rot[1], rot[2]);
        dirtyVel = true;
    }

    bool Jumped(Vector3 loc)
    {
        float jump = Mathf.Abs(loc.x - prevPos.x);
        if (jump > 4) return true;
        jump = Mathf.Abs(loc.z - prevPos.z);
        if (jump > 4) return true;
        return false;
    }
}
