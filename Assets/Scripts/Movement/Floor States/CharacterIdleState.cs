using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("IDLE TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        //Transition for running 
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
        if(character.horizontalInput > 0f || character.verticalInput > 0f){
            character.SwitchState(character.RunState);
        }

        //Jump Transtion
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpState);
        }

        //Primary Transtion
        else if(Input.GetKeyDown(character.primaryKey)){
            character.SwitchState(character.PrimaryState);
        }

        //Secondary Transtion
        else if(Input.GetKeyDown(character.secondaryKey)){
            character.SwitchState(character.SecondaryState);
        }

        //Reload Transtion
        else if(Input.GetKeyDown(character.reloadKey)){
            character.SwitchState(character.ReloadState);
        }
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
