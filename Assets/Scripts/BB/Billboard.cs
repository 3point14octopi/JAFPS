using System;
using UnityEditor;
using UnityEngine;
using Character_Data;
using System.Runtime.InteropServices.WindowsRuntime;

namespace JAFBillboard
{
    public interface IBillboardMode
    {
        void OnStartup(Billboard c);
        void Render(int row, int col);
    }

    public class StaticBB : IBillboardMode
    {
        Billboard bb;
        public void OnStartup(Billboard c)
        {
            bb = c;

            bb.InitGrid();
            bb.NumberOfDirections = 8;
        }

        public void Render(int row, int column)
        {
            try
            {
                bb.spr.sprite = bb.grid[row, column];
            }
            catch (IndexOutOfRangeException)
            {
                Debug.Log("you have GOT to be kidding me");
                column = (column < 0) ? 0 : bb.NumberOfDirections - 1;
                bb.spr.sprite = bb.grid[row, column];
                Debug.Log("Set to " + column.ToString());
            }

            bb.prevPerspective = row * 10 + column;
        }
    }

    public class AnimatedBB : IBillboardMode
    {
        Billboard bb;
        public void OnStartup(Billboard c)
        {
            bb = c;
        }

        public void Render(int row, int column)
        {
            try
            {
                string name = bb.ConvertToDirection(column) + bb.ConvertToLevel(row);
                bb.rControl.AnimationUpdate(name, bb.rControl.actionState);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.Log("you have GOT to be kidding me");
                column = (column < 0) ? 0 : bb.NumberOfDirections - 1;
                string name = bb.ConvertToDirection(column) + bb.ConvertToLevel(row);
                bb.rControl.AnimationUpdate(name, bb.rControl.actionState);
                Debug.Log("Set to " + column.ToString());
            }

            bb.prevPerspective = row * 10 + column;
        }
    }

    public enum EBillboardState
    {
        Static,
        Animated
    }


    public class Billboard : MonoBehaviour
    {
        IBillboardMode billboardStateController;


        public Transform plane;
        public Camera activeCamera;
        public SpriteRenderer spr;

        public EBillboardState renderStyle = EBillboardState.Animated;

        //animated sprite variable - will be hidden if set to Static
        public RemoteCharacter rControl;

        //static sprite variables - will be hidden if set to Animated
        public Sprite[] upper;
        public Sprite[] mid;
        public Sprite[] lower;
        public int NumberOfDirections = 8;
        [HideInInspector] public Sprite[,] grid;

        //math
        private string[] directions = new string[]
        {
            "South", "SouthWest", "West", "NorthWest", "North", "NorthEast", "East", "SouthEast"
        };
        private Vector3 lookposition;
        private bool isBelow;
        private float cHeight;
        [HideInInspector] public int prevPerspective;


        //controls
        public bool bbEnabled = true;//determines if billboarding can happen at all
        public bool isBillboarding = true;//shows if billboarding is currently occurring (is changed dynamically, so is separate)

        private void AddToGrid(Sprite[] sList, int row)
        {
            for (int i = 0; i < sList.Length; i++)
            {
                grid[row, i] = sList[i];
            }
        }

        public void InitGrid()
        {
            grid = new Sprite[3, upper.Length];
            AddToGrid(upper, 0);
            AddToGrid(mid, 1);
            AddToGrid(lower, 2);
        }
        // Start is called before the first frame update
        void Start()
        {
            billboardStateController = (renderStyle == EBillboardState.Static) ? new StaticBB() : new AnimatedBB();
            billboardStateController.OnStartup(this);
            prevPerspective = -1;
        }

        
        public string ConvertToDirection(int value)
        {
            return (NumberOfDirections == 8) ? directions[value] : directions[value * 2];
        }
        public string ConvertToLevel(int value)
        {
            if (value == 2) return "_Bottom";
            if (value == 1) return "_Middle";

            return "_Top";
        }

        void RotateBillboard(float value)
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
                    RotateBillboard(90);
                }


                if(prevPerspective != row + column)
                {
                    billboardStateController.Render(row / 10, column);
                }
            

            }

        
        }

    }
}

