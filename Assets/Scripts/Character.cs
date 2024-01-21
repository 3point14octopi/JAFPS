using System;
using UnityEngine;


namespace Character_Data
{
    public enum CharacterState
    {
        DEFAULT,
        IDLE,
        JUMP,
        PRIMARY,
        SECONDARY,
        RELOAD,
        RUN,
        LONG_JUMP,
        RUNNING_PRIMARY,
        RUNNING_RELOAD,
        FALL,
        FALLING_PRIMARY,
        FALLING_SECONDARY,
        FALLING_RELOAD
    }

    public class Character
    {
        public Animator characterAnimator;
        private CharacterState actionState;
        public Transform characterTransform;
        public Rigidbody characterRigidbody;


        public void UpdateCharacterState(CharacterState newState)
        {
            actionState = newState;
        }


    }



}