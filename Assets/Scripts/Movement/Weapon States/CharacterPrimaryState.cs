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
        
        //Idle Transition
        //Debug.Log("PEW");
        if(Input.GetKeyUp (character.primaryKey)){
            character.SwitchState(character.IdleState);
        }

        //Run and Gun Transition
        character.horizontalInput = Input.GetAxisRaw("Horizontal");
        character.verticalInput = Input.GetAxisRaw("Vertical");
        if(character.horizontalInput > 0f || character.verticalInput > 0f){
            character.SwitchState(character.RunAndGunState);
        }
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
