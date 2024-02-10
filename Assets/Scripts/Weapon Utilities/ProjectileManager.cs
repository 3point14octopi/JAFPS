using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [Header("Primary \n")]
    public GameObject primaryBulletObject;

    public Transform primaryBulletSpawnPoint;
    public float primaryBulletSpeed;

    public static ProjectileManager instance { get; private set; }
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
        else
        {
            Destroy(this);
        }
    }


    public void PrimaryShoot()
    {
        Debug.Log("Pew");
        GameObject bullet = Instantiate(primaryBulletObject, primaryBulletSpawnPoint.position + primaryBulletSpawnPoint.forward, primaryBulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = primaryBulletSpawnPoint.forward * primaryBulletSpeed;
    }
}
