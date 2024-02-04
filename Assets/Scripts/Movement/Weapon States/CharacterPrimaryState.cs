using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrimaryState : CharacterBaseState
{
    private float timer = 0f; //local timer

    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("PRIMARY TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        
        //Idle Transition
        if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.IdleState);
        }

        //Run and Gun Transition
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
        if(character.horizontalInput > 0f || character.verticalInput > 0f){
            character.SwitchState(character.RunAndGunState);
        }
        
        //Manages the firerate
        timer -= Time.deltaTime;

        //If you have bullets and the timer is done you shoot
        if(character.primary > 0 && timer <= 0){
            Debug.Log("Pew"); //THIS IS WHERE SHOOTING WILL GO
            character.primary = character.primary - 1;
            timer = character.primaryFireRate;
        }
        //If out of bullets your gun clicks and you automatically reload
        else if(character.primary <= 0){
            Debug.Log("Click");
            character.SwitchState(character.ReloadState);
        }
        
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
