using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifespan = 2;

    public float bulletDamage = 5;

    public GameObject hitEffect;

    private void Awake() 
    {
        Destroy(gameObject, lifespan);    
    }

    private void OnCollisionEnter(Collision other) 
    {
        //Fancy Bullet Impact things may go here later
        GameObject particle = Instantiate(hitEffect, transform.position - transform.forward, transform.rotation);
        particle.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}
