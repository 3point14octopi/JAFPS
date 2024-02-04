using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReloadState : CharacterBaseState
{
    private float timer; //local timer

    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("RELOAD TIME");

        //sets the timer to the reload time
        timer = character.primaryReload;
    }

    public override void UpdateState(CharacterStateManager character){

        //starts counting down the reload time
        timer -= Time.deltaTime;

        //reloads then sends you back to idle
        if(timer <=0){
            character.primary = character.primaryMax;
            character.SwitchState(character.IdleState);
        }
    }
    public override void OnCollisionEnter(CharacterStateManager character, Collision Collision){

    }

    public override void OnCollisionExit(CharacterStateManager character, Collision Collision){

    }
}
