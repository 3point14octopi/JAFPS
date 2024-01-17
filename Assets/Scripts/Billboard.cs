using System;
using UnityEditor;
using UnityEngine;
using Character_Data;
using System.Runtime.InteropServices.WindowsRuntime;

public class Billboard : MonoBehaviour
{
    public Transform plane;
    
    public Camera activeCamera;
    public bool bbEnabled = true;

    public SpriteRenderer spr;
    public RemoteCharacter rControl;
    //public Sprite[] upper;
    //public Sprite[] mid;
    //public Sprite[] lower;

    //private Sprite[,] grid;

    private string[] directions = new string[]
    {
        "South", "SouthWest", "West", "NorthWest", "North", "NorthEast", "East", "SouthEast"
    };


    private Vector3 lookposition;
    private bool isBelow;
    private float cHeight;
    private int prevPerspective;

    public Animator bb_animator;
    public int NumberOfDirections = 8;
    public bool isBillboarding = true;


    private void AddToGrid(Sprite[] sList, int row)
    {
        //for(int i = 0; i < sList.Length; i++)
        //{
        //    grid[row, i] = sList[i];
        //}
    }

    // Start is called before the first frame update
    void Start()
    {
        //grid = new Sprite[3, upper.Length];
        //AddToGrid(upper, 0);
        //AddToGrid(mid, 1);
        //AddToGrid(lower, 2);
        prevPerspective = -1;
        PrintSomeStuff();
    }


    string ConvertToDirection(int value)
    {
        return (NumberOfDirections == 8) ? directions[value] : directions[value * 2];
    }
    
    //filty if true
    string ConvertToLevel(int value)
    {
        if (value == 2) return "_Bottom";
        if (value == 1) return "_Middle";

        return "_Top";
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

        float div = (float)(360 / NumberOfDirections);

        int column = (int)(d / div);
        column = (column >=NumberOfDirections)?NumberOfDirections-1:column;

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


    void ToggleByView()
    {
        bool inSight = GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(activeCamera), spr.bounds);

        if (inSight != isBillboarding)
        {
            prevPerspective = -1;
            rControl.PlayPauseAnimator((inSight) ? 1 : 0);

            isBillboarding = inSight;
        }
    }


    // Update is called once per frame
    void Update()
    {
        ToggleByView();
        if (bbEnabled && isBillboarding)
        {
            Speen();
            int column = CalculateAngleSprite();

            int row = 10;
            if (HeightAngle() > 50)
            {
                row = (isBelow) ? 20 : 0;
                RotatetheBanana(90);
            }


            if(prevPerspective != row + column)
            {
                try
                {
                    string name = ConvertToDirection(column) + ConvertToLevel(row%10);
                    rControl.AnimationUpdate(name, rControl.actionState);
                    //bb_animator.SetInteger("Perspective", row + column);
                    //spr.sprite = grid[row, column];
                }
                catch (IndexOutOfRangeException)
                {
                    Debug.Log("you have GOT to be kidding me");
                    column = (column < 0) ? 0 : NumberOfDirections - 1;
                    name = ConvertToDirection(column) + ConvertToLevel(row%10);
                    rControl.AnimationUpdate(name, rControl.actionState);
                    //bb_animator.SetInteger("Perspective", row + column);
                    Debug.Log("Set to " + column.ToString());
                }

                prevPerspective = row + column;
            }
            

        }

        
    }


    void PrintSomeStuff()
    {
        Debug.Log(bb_animator.GetLayerIndex("Idle"));
        Debug.Log(bb_animator.GetLayerIndex("Jump"));
        Debug.Log(bb_animator.GetLayerIndex("Primary"));
        Debug.Log(bb_animator.GetLayerIndex("Secondary"));
        Debug.Log(bb_animator.GetLayerIndex("Reload"));
        Debug.Log(bb_animator.GetLayerIndex("Run"));
        Debug.Log(bb_animator.GetLayerIndex("Long Jump"));
        Debug.Log(bb_animator.GetLayerIndex("Running Primary"));
        Debug.Log(bb_animator.GetLayerIndex("Running Reload"));
        Debug.Log(bb_animator.GetLayerIndex("Fall"));
        Debug.Log(bb_animator.GetLayerIndex("Falling Primary"));
        Debug.Log(bb_animator.GetLayerIndex("Falling Secondary"));
        Debug.Log(bb_animator.GetLayerIndex("Falling Reload"));
    }
}
