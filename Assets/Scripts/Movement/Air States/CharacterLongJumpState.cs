using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLongJumpState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("LONG JUMP TIME");
        
        //Long Jumps and resets our long jump
        if(character.readyToJump){
            character.rb.AddForce(character.orientation.up * character.longJumpForce, ForceMode.Impulse);
            character.readyToJump = false;
            character.jumpTimer = character.jumpCooldown;
        }
   }

    public override void UpdateState(CharacterStateManager character){
        
        //After force is applied we go right to falling state
        character.SwitchState(character.FallingState);   
    }
}
