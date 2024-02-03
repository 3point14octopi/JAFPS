using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("Jump TIME");
        //takes off teh ground friction
        character.rb.drag = 0;
        //resets our jump
        character.readyToJump = false;
        //starts our jump timer
        character.jumpCooldown = 1f;
        //Gives us a boost of force upward
        character.rb.AddForce(character.orientation.up * character.jumpForce, ForceMode.Impulse);
   }

    public override void UpdateState(CharacterStateManager character){
        
        //Transitions
        character.grounded = Physics.Raycast(character.transform.position, Vector3.down, character.playerHeight * 0.5f + 0.2f, character.whatIsGround);
        if(character.grounded && character.readyToJump){
            //Idle Transition
            if(character.horizontalInput == 0f && character.verticalInput == 0f){
                character.SwitchState(character.IdleState);
            }
            //Run Transition
            else{
                character.SwitchState(character.RunState);
            }
        }

        //lowers our jumpcooldown until we get jump back
        character.jumpCooldown -= Time.deltaTime;
        if(character.jumpCooldown <= 1f){
            character.readyToJump = true;
        }

        //WASD movement || finds our vector then gives momentum in that direction || different from RunState because the force is lowered by the airMultiplier
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
        character.moveDirection = character.orientation.forward * character.verticalInput + character.orientation.right * character.horizontalInput;
        character.rb.AddForce(character.moveDirection.normalized * character.movementSpeed * 10f * character.airMultiplier, ForceMode.Force);
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
