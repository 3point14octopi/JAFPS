using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFallingPrimaryState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("FALLING PRIMARY TIME");
        //takes off teh ground friction
        character.rb.drag = character.airDrag;
   }

    public override void UpdateState(CharacterStateManager character){
        
        //Falling Transition
        if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.FallingState);
        }

        //Primary Transition
        else if(character.grounded){
            character.SwitchState(character.PrimaryState);
        }

        Moving(character);
        PrimaryFire(character);
        JumpCheck(character);
    }

    public void Moving(CharacterStateManager character){     
        //WASD movement || finds our vector then gives momentum in that direction
        character.moveDirection = character.orientation.forward * character.verticalInput + character.orientation.right * character.horizontalInput;
        character.rb.AddForce(character.moveDirection.normalized * character.movementSpeed * 10f, ForceMode.Force);

        //check current speed
        Vector3 speed = new Vector3(character.rb.velocity.x, 0f, character.rb.velocity.z);

        //lowers our velocity if we accidently surpassed the ground limit
        if(speed.magnitude > character.movementSpeed){
            character.speedCap = speed.normalized * character.movementSpeed;
            character.rb.velocity = new Vector3(character.speedCap.x, character.rb.velocity.y, character.speedCap.z);
        }
    }

    public void JumpCheck(CharacterStateManager character){     
        //limits our jumps to one every .1 seconds so we cant spam forever
        if(character.jumpCooldown <= 1f){
            character.readyToJump = true;
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
            character.SwitchState(character.FallingReloadState);
        }
    }
}
