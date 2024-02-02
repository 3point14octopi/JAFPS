using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateManager : MonoBehaviour
{
    //Keeps track of our current state.
    public CharacterBaseState currentState;

    //Catalog of all states
    public CharacterIdleState IdleState = new CharacterIdleState();
    public CharacterRunState RunState = new CharacterRunState();   
    public CharacterJumpState JumpState = new CharacterJumpState();


    void Start()
    {
        //Sets our current state to the most boring state we own
        currentState = IdleState;

        //Calls the enter state function of the current state.
        currentState.EnterState(this);
    }

    void Update()
    {
        //Uses the update fuction of our current state
        currentState.UpdateState(this);
    }

    void OnCollisionEnter(Collision Collision){
        //Passes on our context and collision data to the current states collsion function
        currentState.OnCollisionEnter(this, Collision);
    }

        void OnCollisionExit(Collision Collision){
        //Passes on our context and collision data to the current states collsion function
        currentState.OnCollisionExit(this, Collision);
    }

    //is called when a transition condition in our current states update is met
    public void SwitchState(CharacterBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }
}
