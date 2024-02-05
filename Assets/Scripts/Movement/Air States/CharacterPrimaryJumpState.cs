using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPrimaryJumpState : CharacterBaseState
{
    public override void EnterState(CharacterStateManager character){
        //debug
        Debug.Log("PRIMARY JUMP TIME");
        
        //Long Jumps and resets our long jump
        if(character.readyToJump){
            character.rb.AddForce(character.orientation.up * character.primaryJumpForce, ForceMode.Impulse);
            character.readyToJump = false;
            character.jumpTimer = character.jumpCooldown;
        }
   }

    public override void UpdateState(CharacterStateManager character){
        
        //After force is applied we go right to falling state
        character.SwitchState(character.FallingPrimaryState);   
    }
}
