using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [Header("Camera")]
    public float sensX;
    float rotationX;
    public float sensY;
    float rotationY;

    //Someone online said to do this cause it makes movement alot easier
    public Transform orientation;

    private void Start()
    {
        //gets ride of the cursor so we can actually debug. USe CTRL + P to stop gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Based on mouse move
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        //updates our variables and clamps them
        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        //once to rotate the camera on both access and one to rotate the player on the y axis
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
