
using UnityEngine;

public abstract class CharacterBaseState
{
    //Used when a new state is entered
   public abstract void EnterState(CharacterStateManager character);

    //Called every mono update while in that state
   public abstract void UpdateState(CharacterStateManager character);
    /*
    //Called when a collision happens while in that state
   public abstract void OnCollisionEnter(CharacterStateManager character, Collision Collision);

    //Called when a collision stops while in that state
   public abstract void OnCollisionExit(CharacterStateManager character, Collision Collision);
   */
}
