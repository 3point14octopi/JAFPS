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
        //fires our secondary
        SecondaryFire(character);
        
        //Idle Transition
        character.SwitchState(character.IdleState);
    }
    public void SecondaryFire(CharacterStateManager character){      
        //If the timer is done you can use your secondary
        if(character.secondaryTimer <= 0){
            Debug.Log("BOOM"); //THIS IS WHERE SECONDARY WILL GO
            character.secondary = character.secondary - 1;
            character.secondaryTimer = character.secondaryFireRate;
        }
        else if(character.secondaryTimer > 0){
            Debug.Log("Not charged yet");
        }
    }
}
