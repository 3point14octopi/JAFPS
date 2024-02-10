using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifespan = 2;

    private void Awake() 
    {
        Destroy(gameObject, lifespan);    
    }

    private void OnCollisionEnter(Collision other) 
    {
        //Fancy Bullet Impact things may go here later
        Destroy(gameObject);
    }
}
