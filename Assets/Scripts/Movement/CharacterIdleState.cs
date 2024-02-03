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
        //Transition for running 
        //if there is any hori or vert input it we enter run
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
        if(character.horizontalInput > 0f || character.verticalInput > 0f){
            character.SwitchState(character.RunState);
        }
        //Transition for jumping
        //triggered by spacebar
        else if(Input.GetKey(character.jumpKey)){
            character.SwitchState(character.JumpState);
        }
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
