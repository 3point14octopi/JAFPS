using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    //This script is to match the camera to position to the child of the character because have the camera be the child of a rigidbody directly is buggy apparently
    public Transform cameraPosition;
    void Update()
    {
        //has the camera follow around a child of the player
        transform.position = cameraPosition.position;
    }
}
