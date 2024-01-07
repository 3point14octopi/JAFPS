using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform plane;
    public SpriteRenderer spr;
    public Camera activeCamera;
    public bool bbEnabled = true;
    public bool correctionOn = false;


    public Sprite[] upper;
    public Sprite[] mid;
    public Sprite[] lower;

    private Sprite[,] grid;

    private Vector3 lookposition;
    private float upAngle;
    private bool isBelow;
    private float dFromC;
    private float cHeight;
    private float oToCAngle = 0;


    private void AddToGrid(Sprite[] sList, int row)
    {
        for(int i = 0; i < sList.Length; i++)
        {
            grid[row, i] = sList[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        grid = new Sprite[3, upper.Length];
        AddToGrid(upper, 0);
        AddToGrid(mid, 1);
        AddToGrid(lower, 2);
    }


    void RotatetheBanana(float value)
    {
        value -= plane.rotation.x;
        
        plane.Rotate(value, 0, 0);
        Debug.Log("banana rotated");
    }


    void Speen()
    {
        lookposition = activeCamera.transform.position;
        float y = activeCamera.transform.position.y;
        lookposition.y = plane.parent.transform.position.y;
        cHeight = y - lookposition.y;

        if(cHeight < 0)
        {
            isBelow = true;
            cHeight = Math.Abs(cHeight);
        }

        plane.LookAt(lookposition);
        //plane.LookAt(activeCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (bbEnabled)
        {
            Vector3 prev = this.transform.forward;
            Speen();
            
            
            float d = Vector3.Angle(prev, plane.forward);

            d = (Vector3.Angle(this.transform.right, plane.forward) <= 90) ? 360 - d : d;

            float div = (float)(360 / grid.GetLength(1));

            int column = (int)(d / div);
            column = (column >=grid.GetLength(1))?7:column;



            float dist = Vector3.Distance(this.transform.position, lookposition);
            if (dist < 1 && cHeight > 1)
            {

                Debug.Log("very close");
                oToCAngle = 90;
            }
            else
            {
                float hyp = Mathf.Sqrt(Mathf.Pow(cHeight, 2) + Mathf.Pow(dist, 2));

                oToCAngle = Mathf.Rad2Deg * (float)(Math.Asin(cHeight * (Math.Sin(90) / hyp)));

                Debug.Log("the angle is " + oToCAngle + "º vs " + (Mathf.Rad2Deg * Vector3.Angle(this.transform.position, activeCamera.transform.position)).ToString() + "º");

                //plane.rotation.Set(new Vector3())


            }

            

            int row = 1;
            if (oToCAngle > 50)
            {
                row = (isBelow) ? 2 : 0;
                RotatetheBanana(90);
            }
            //else
            //{

            //}

            try
            {
                spr.sprite = grid[row, column];
            }
            catch (IndexOutOfRangeException)
            {
                Debug.Log("you have GOT to be kidding me");
                column = (column < 0) ? 0 : grid.GetLength(1) - 1;
                spr.sprite = grid[row, column];
                Debug.Log("Set to " + column.ToString());
            }

            ////plane.LookAt(activeCamera.transform, transform.up);
            ////plane.rotation *= Quaternion.FromToRotation(Vector3.up, -Vector3.up);
            //if (correctionOn)
            //{
            //    plane.rotation *= Quaternion.FromToRotation(Vector3.right, -Vector3.right);
            //    plane.rotation *= Quaternion.FromToRotation(Vector3.forward, -Vector3.forward);
            //}



            //int row;
            //float hAng = Vector3.Angle(plane.parent.transform.up, plane.forward);

            //if(hAng >= 120)
            //{
            //    row = 2;
            //}else if(hAng <= 60)
            //{
            //    row = 0;
            //}
            //else
            //{
            //    row = 1;
            //}

            //spr.sprite = grid[row, column];


            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Debug.Log(prev.x.ToString() + ", " + prev.y.ToString() + ", " + prev.z.ToString() + '\n' +
            //        plane.forward.x.ToString() + ", " + plane.forward.y.ToString() + ", " + plane.forward.z.ToString());
            //    Debug.Log(d);



            //}



        }
        
       
    }


}
