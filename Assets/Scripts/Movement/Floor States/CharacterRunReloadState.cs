using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunReloadState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("RUNNING RELOAD TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        //Transition for falling 
        if(!character.grounded){
            character.SwitchState(character.FallingReloadState);
        }

        //Transition for idle reload 
        else  if(character.horizontalInput == 0f && character.verticalInput == 0f){
            character.SwitchState(character.ReloadState);
        }

        //Jump Reload Transtion
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpReloadState);       
        }

        //When reloading is done reloads then sends you back to running
        if(character.primaryTimer <=0){
            Reload(character);
            character.SwitchState(character.RunState);
        }

        Moving(character);
    }

    //Sets the magazine to max ammo 
    public void Reload(CharacterStateManager character){      
        Debug.Log("Reloaded"); 
        character.primary = character.primaryMax;
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

}
