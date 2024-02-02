using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRunState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("RUN TIME");
   }

    public override void UpdateState(CharacterStateManager character){

    }

    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
