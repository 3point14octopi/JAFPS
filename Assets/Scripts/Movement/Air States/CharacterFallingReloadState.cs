using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFallingReloadState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("FALLING RELOAD TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        
        //Reload Transition
        if(character.grounded){
            character.SwitchState(character.ReloadState);
        }

        //When reloading is done reloads then sends you back to regular falling
        if(character.primaryTimer <=0){
            Reload(character);
            character.SwitchState(character.FallingState);
        }

        
        Moving(character);
        JumpCheck(character);

    }
  
    //Sets the magazine to max ammo 
    public void Reload(CharacterStateManager character){      
        Debug.Log("Reloaded"); 
        character.primary = character.primaryMax;
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
