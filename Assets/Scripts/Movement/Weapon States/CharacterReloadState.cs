using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReloadState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("RELOAD TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        
        //Transition for falling 
        if(!character.grounded){
            character.SwitchState(character.FallingReloadState);
        }

        //Transition for running reload 
        else if(character.horizontalInput != 0f || character.verticalInput != 0f){
            character.SwitchState(character.RunReloadState);
        }

        //Jump Reload Transtion
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpReloadState);       
        }

        //When reloading is done reloads then sends you back to idle
        if(character.primaryTimer <=0){
            Reload(character);
            character.SwitchState(character.IdleState);
        }

    }

    //Sets the magazine to max ammo 
    public void Reload(CharacterStateManager character){      
        Debug.Log("Reloaded"); 
        character.primary = character.primaryMax;
    }
}
