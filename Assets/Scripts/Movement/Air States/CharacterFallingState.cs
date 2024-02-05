using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFallingState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("FALLING TIME");
        //takes off teh ground friction
        character.rb.drag = character.airDrag;
   }

    public override void UpdateState(CharacterStateManager character){
        
        //Idle and Running Transition
        if(character.grounded){

            if(character.horizontalInput != 0f || character.verticalInput != 0f){
                character.SwitchState(character.RunState);
            }
            else if(character.horizontalInput == 0f && character.verticalInput == 0f){
                character.SwitchState(character.IdleState);
            }
        }

        //Primary Transtion
        else if(Input.GetKeyDown(character.primaryKey)){
            character.SwitchState(character.PrimaryState);
        }
        
        //Falling Reload Transtion
        else if(Input.GetKeyDown(character.reloadKey)){
            if(character.primary < character.primaryMax){
                character.primaryTimer = character.primaryReload;
                character.SwitchState(character.FallingReloadState);
            }
        }

        Moving(character);
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
}
