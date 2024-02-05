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
        
        //Transition for falling 
        if(!character.grounded){
            character.SwitchState(character.FallingState);
        }

        //Transition for running 
        else if(character.horizontalInput != 0f || character.verticalInput != 0f){
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
            if(character.primary < character.primaryMax){
                character.primaryTimer = character.primaryReload;
                character.SwitchState(character.ReloadState);
            }
        }
    }
}
