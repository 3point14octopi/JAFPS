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
    private bool isBelow;
    private float cHeight;



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
        
        if(value !=0)plane.Rotate(value, 0, 0);
    }
    
    float HeightAngle()
    {
        float dist = Vector3.Distance(this.transform.position, lookposition);
        if (dist < 1 && cHeight > 1)
        {
            return 90;
        }
        else
        {
            float hyp = Mathf.Sqrt(Mathf.Pow(cHeight, 2) + Mathf.Pow(dist, 2));

            return Mathf.Rad2Deg * (float)(Math.Asin(cHeight * (Math.Sin(90) / hyp)));
        }
    }
    
    int CalculateAngleSprite()
    {
        float d = Vector3.Angle(this.transform.forward, plane.forward);

        d = (Vector3.Angle(this.transform.right, plane.forward) <= 90) ? 360 - d : d;

        float div = (float)(360 / grid.GetLength(1));

        int column = (int)(d / div);
        column = (column >=grid.GetLength(1) -1)?7:column;

        return column;
    }

    void Speen()
    {
        lookposition = activeCamera.transform.position;
        float y = activeCamera.transform.position.y;
        lookposition.y = plane.parent.transform.position.y;
        cHeight = y - lookposition.y;

        isBelow = (cHeight < 0);
        if (isBelow) cHeight = Math.Abs(cHeight);

        plane.LookAt(lookposition);
        //plane.LookAt(activeCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (bbEnabled)
        {
            Speen();
            int column = CalculateAngleSprite();

            int row = 1;
            if (HeightAngle() > 50)
            {
                row = (isBelow) ? 2 : 0;
                RotatetheBanana(90);
            }

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

        }
        
       
    }


}
