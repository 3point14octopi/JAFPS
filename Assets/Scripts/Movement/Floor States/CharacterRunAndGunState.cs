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
        
        //Primary Transition
        if(character.horizontalInput == 0f && character.verticalInput == 0f){
            character.SwitchState(character.IdleState);
        }
        
        //Run Transition
        //Debug.Log("PEW(On the Move)");
        if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.RunState);
        }

        //WASD movement || finds our vector then gives momentum in that direction
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
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
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}