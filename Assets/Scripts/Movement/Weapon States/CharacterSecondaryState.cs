using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSecondaryState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("SECONDARY TIME");
    }

    public override void UpdateState(CharacterStateManager character){

        Debug.Log("BOOM");
        //Idle Transition
        character.SwitchState(character.IdleState);

    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
