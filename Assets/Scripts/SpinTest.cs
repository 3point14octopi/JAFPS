using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinTest : MonoBehaviour
{
    public Transform thingToSpin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            thingToSpin.Rotate(new Vector3(0, 15 * Time.deltaTime, 0));
        }

        if(Input.GetKey(KeyCode.RightArrow)) 
        { 
            thingToSpin.Rotate(new Vector3(0, -15 * Time.deltaTime, 0)); 
        }


        //strafing
        if (Input.GetKey(KeyCode.W))
        {
            thingToSpin.position += thingToSpin.forward * 4 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            thingToSpin.position -= thingToSpin.forward * 4 * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            thingToSpin.position += thingToSpin.right * 4* Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            thingToSpin.position -= thingToSpin.right * 4 * Time.deltaTime;
        }
    }
}
