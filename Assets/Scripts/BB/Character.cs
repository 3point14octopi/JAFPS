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
        PRIMARY_JUMP,
        SECONDARY,
        RELOAD,
        RELOAD_JUMP,
        RUN,
        LONG_JUMP,
        RUN_AND_GUN,
        RUN_AND_RELOAD,
        FALL,
        FALLING_PRIMARY,
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