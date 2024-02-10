using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunAndGunState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("RUN AND GUN TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        
        //Primary Falling Transiton
        if(!character.grounded){
            character.SwitchState(character.FallingPrimaryState);
        }

        //Run Transition
        else if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.RunState);
        }

        //Primary Transition
        else if(character.horizontalInput == 0f && character.verticalInput == 0f){
            character.SwitchState(character.IdleState);
        }

        //Jump Primary Transtion
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpPrimaryState);       
        }

        //Running Reload Transtion
        else if(Input.GetKeyDown(character.reloadKey)){
            if(character.primary < character.primaryMax){
                character.primaryTimer = character.primaryReload;
                character.SwitchState(character.RunReloadState);
            }
        }
        
        //this is run and gun so we use the functions for run and gun
        Moving(character);
        PrimaryFire(character);
    }

    public void Moving(CharacterStateManager character){     
        //WASD movement || finds our vector then gives momentum in that direction
        character.moveDirection = character.orientation.forward * character.verticalInput + character.orientation.right * character.horizontalInput;
        character.rb.AddForce(character.moveDirection.normalized * character.primaryWalkSpeed * 10f, ForceMode.Force);


        //check current speed
        Vector3 speed = new Vector3(character.rb.velocity.x, 0f, character.rb.velocity.z);

        //lowers our velocity if we accidently surpassed the ground limit
        if(speed.magnitude > character.primaryWalkSpeed){
            character.speedCap = speed.normalized * character.primaryWalkSpeed;
            character.rb.velocity = new Vector3(character.speedCap.x, character.rb.velocity.y, character.speedCap.z);
        }
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
