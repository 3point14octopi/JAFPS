using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character_Data;
using System.Threading;

namespace Character_Data
{
    public static class DirectionalData
    {
        static string[] directions = new string[]
        {
            "South", "SouthWest", "West", "NorthWest", "North", "NorthEast", "East", "SouthEast"
        };

        public static string ConvertToDirection(int value, int NumberOfDirections = 8)
        {
            return (NumberOfDirections == 8) ? directions[value] : directions[value * 2];
        }

        public static string ConvertToLevel(int value)
        {
            if (value == 2) return "_Bottom";
            if (value == 1) return "_Middle";

            return "_Top";
        }
    }

    public static class StateData
    {
        public static readonly Dictionary<string, short> stateCodes = new Dictionary<string, short>()
        {
            {"CharacterIdleState", 1 } ,
            {"CharacterJumpState", 2},
            {"CharacterPrimaryState", 3},
            {"CharacterSecondaryState", 4},
            {"CharacterReloadState", 5},
            {"CharacterRunState", 6},
            {"CharacterLongJumpState", 7},
            {"CharacterRunAndGunState", 8},
            {"CharacterRunningReloadState", 9},
            {"CharacterFallState", 10},
            {"CharacterFallingPrimaryState", 11},
            {"CharacterFallingSecondaryState", 12},
            {"CharacterFallingReloadState", 13}
        };
    }
}







public class RemoteCharacter : MonoBehaviour
{



    public Animator characterAnimator;
    public CharacterState actionState = CharacterState.DEFAULT;
    private string lastDirection = "South_Middle";
    private bool isPlaying = true;
    public short visualizer;
    private Mutex animblock = new Mutex();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates the character's in-game visual, and their state if it is different
    /// This function should be called if the character is either performing a new action or is being viewed from a new angle
    /// </summary>
    /// <param name="direction"> The string representing the direction the character is being viewed from </param>
    /// <param name="newState"> The new state of the character</param>
    /// <param name="timeOverride"> Skips to a point in the animation between 0 and 1, should only be given a value if a time-sensitive action is being performed. 
    /// Entered value will be ignored if state has not changed. </param>
    public void AnimationUpdate(string direction, CharacterState newState, float timeOverride = 0f)
    {
        if (animblock.WaitOne())
        {
            if (direction == "LAST") direction = lastDirection;
            if (newState == actionState)
            {
                AnimatorStateInfo animState = characterAnimator.GetCurrentAnimatorStateInfo((int)newState);
                //we want to know where we are in the animation, in case it's a time-sensitive animation
                //for instance, you don't want to see me start shooting and then appear to shoot twice
                //because you started viewing me from another angle before the anim completed
                timeOverride = animState.normalizedTime % animState.length;
            }
            else
            {
                actionState = newState;
                characterAnimator.SetLayerWeight((int)actionState, 0);
                characterAnimator.SetLayerWeight((int)newState, 1);
                //we're autosetting for now but we'll account for like. the actual start times LATER:tm:
                timeOverride = 0f;
            }

            characterAnimator.Play(direction, (int)newState, timeOverride);
            lastDirection = direction;
            animblock.ReleaseMutex();
        }
        
    }

    public void PlayPauseAnimator(int value)
    {
        characterAnimator.speed = value;
        isPlaying = (value > 0);
    }

    /// <summary>
    /// Updates state 
    /// Should only really be used by remote managers
    /// should start worrying about time sensitive actions later
    /// </summary>
    public void SetState(short stateCode)
    {
        visualizer = stateCode;
        int converter = (int)stateCode;
        if(isPlaying)
        {
            
            AnimationUpdate("LAST", (CharacterState)converter);
            Debug.Log("new state: " + ((CharacterState)converter).ToString());
        }
        else
        {
            actionState = (CharacterState)converter;
        }
    }

    public void SetState(string stateName)
    {
        short stateCode = 0;
        if(StateData.stateCodes.TryGetValue(stateName, out stateCode))
        {
            SetState(stateCode);
        }
    }
}
