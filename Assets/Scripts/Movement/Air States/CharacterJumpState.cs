using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterJumpState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("Jump TIME");
                
        //Long Jumps and resets our jump
        if(character.readyToJump){
            character.rb.AddForce(character.orientation.up * character.jumpForce, ForceMode.Impulse);
            character.readyToJump = false;
            character.jumpTimer = character.jumpCooldown;
        }
    }

    public override void UpdateState(CharacterStateManager character){  

        //After force is applied we go right to falling state
        character.SwitchState(character.FallingState);   
    }
}
