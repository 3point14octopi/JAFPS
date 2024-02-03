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
    public CharacterLongJumpState LongJumpState = new CharacterLongJumpState();


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space; //for jump
    public float horizontalInput; //for W && S
    public float verticalInput; //for A && D


   
    [Header("Running")]
    public Transform orientation; //spins with camera to know what is forward
    public Rigidbody rb; //player rigidbody
    public float groundDrag;//drag strength when running
    public Vector3 moveDirection; //movement angle
    public float movementSpeed;//run speed
    public Vector3 speedCap; //used with movement speed



    [Header("Jumping")]
    public float airDrag;//drag strength when in air
    public float jumpForce;// jump power/height
    public float airMultiplier;//how much less speed you have in the air
    public float jumpCooldown;//minimum time between jump in seconds
    public bool readyToJump;//reset after jumpCooldown

    [Header("Long Jumping")]
    public float longJumpForce;// jump power/height for long jump
    public float longAirMultiplier;//how much less speed you have in the air on long jump

    [Header("Raycast")]
    public float playerHeight;//used to raycast the correct distance downward
    public bool grounded;//stores if we are on the floor 
    public LayerMask whatIsGround;//layer for objects that trigger the grounded bool


    void Start()
    {
        //sets our rigidbody
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        //Sets our current state to the most boring state we own
        currentState = IdleState;

        //Calls the enter state function of the current state.
        currentState.EnterState(this);
    }

    //Update and the collisions basically offload their work to the current state
    void Update()
    {
        currentState.UpdateState(this);
    }

    void OnCollisionEnter(Collision Collision){
        currentState.OnCollisionEnter(this, Collision);
    }

        void OnCollisionExit(Collision Collision){
        currentState.OnCollisionExit(this, Collision);
    }

    //is called when a transition condition in our current states update is met
    public void SwitchState(CharacterBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }

}
