using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("GAME TIME");
    }

    public override void UpdateState(CharacterStateManager character){
        //debug
        character.SwitchState(character.RunState);
    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
