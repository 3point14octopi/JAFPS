using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Character_Data;



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
}







public class RemoteCharacter : MonoBehaviour
{
    public Animator characterAnimator;
    public CharacterState actionState = CharacterState.DEFAULT;
    [SerializeField] Transform characterTransform;
    [SerializeField] Rigidbody characterRigidbody;


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
        if (newState == actionState)
        {
            Debug.Log("Changed direction: " + direction);
            AnimatorStateInfo animState = characterAnimator.GetCurrentAnimatorStateInfo((int)newState);
            timeOverride = animState.normalizedTime % animState.length;
            Debug.Log(timeOverride.ToString());
        }
        else
        {
            actionState = newState;
            characterAnimator.SetLayerWeight((int)actionState, 0);
            characterAnimator.SetLayerWeight((int)newState, 1);
        }

        characterAnimator.Play(direction, (int)newState);
    }

    public void PlayPauseAnimator(int value)
    {
        characterAnimator.speed = value;
    }

    public void RepositionCharacter(Vector3 newPos)
    {
        characterTransform.position = newPos;
    }

    public void RotateCharacter(Vector3 newRot)
    {
        float newX = characterTransform.rotation.x - newRot.x;
        float newY = characterTransform.rotation.y - newRot.y;
        float newZ = characterTransform.rotation.z - newRot.z;

        if(newX + newY + newZ != 0f)characterTransform.Rotate(newX, newY, newZ);
    }

    public void ApplyForce(Vector3 force)
    {
        characterRigidbody.AddForce(force);
    }

    public void SetVelocity(Vector3 velocity)
    {
        characterRigidbody.velocity = velocity;
    }
}
