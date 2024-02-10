using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrimaryState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("PRIMARY TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        
        //Primary Falling Transiton
        if(!character.grounded){
            character.SwitchState(character.FallingPrimaryState);
        }
        
        //Idle Transition
        if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.IdleState);
        }

        //Run and Gun Transition
        else if(character.horizontalInput > 0f || character.verticalInput > 0f){
            character.SwitchState(character.RunAndGunState);
        }

        //Jump Primary Transtion
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpPrimaryState);       
        }

        //Reload Transtion
        else if(Input.GetKeyDown(character.reloadKey)){
            if(character.primary < character.primaryMax){
                character.primaryTimer = character.primaryReload;
                character.SwitchState(character.ReloadState);
            }
        }

        PrimaryFire(character);
    }


    public void PrimaryFire(CharacterStateManager character){      
        //If you have bullets and the timer is done you shoot
        if(character.primary > 0 && character.primaryTimer <= 0){
            Debug.Log("Pew"); //THIS IS WHERE SHOOTING WILL GO
            ProjectileManager.instance.PrimaryShoot();
            //THAT'S WHERE SHOOTING WENT
            character.primary = character.primary - 1;
            character.primaryTimer = character.primaryFireRate;
        }
        //If out of bullets your gun clicks and you automatically reload
        else if(character.primary <= 0){
            Debug.Log("Click");
            character.SwitchState(character.ReloadState);
        }
    }
}
