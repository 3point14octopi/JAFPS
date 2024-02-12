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
    public CharacterFallingState FallingState = new CharacterFallingState();

    public CharacterSecondaryState SecondaryState = new CharacterSecondaryState();

    public CharacterPrimaryState PrimaryState = new CharacterPrimaryState();
    public CharacterRunAndGunState RunAndGunState = new CharacterRunAndGunState();
    public CharacterPrimaryJumpState JumpPrimaryState = new CharacterPrimaryJumpState();
    public CharacterFallingPrimaryState FallingPrimaryState = new CharacterFallingPrimaryState();

    public CharacterReloadState ReloadState = new CharacterReloadState();
    public CharacterRunReloadState RunReloadState = new CharacterRunReloadState(); 
    public CharacterReloadJumpState JumpReloadState = new CharacterReloadJumpState();
    public CharacterFallingReloadState FallingReloadState = new CharacterFallingReloadState();

    [Header("Player Stats\n")]
    public float health = 100;
    public float maxHealth = 100;

    [Header("Keybinds")]
    public float horizontalInput; //for W && S
    public float verticalInput; //for A && D
    public KeyCode jumpKey = KeyCode.Space; //for jump
    public KeyCode primaryKey = KeyCode.Mouse0; //for primary weapomn
    public KeyCode secondaryKey = KeyCode.Mouse1;//for secondary gun
    public KeyCode reloadKey = KeyCode.R;//for reloading the primary gun
   
    [Header("Running")]
    public Transform orientation; //spins with camera to know what is forward
    public Rigidbody rb; //player rigidbody
    public float groundDrag;//drag strength when running
    public Vector3 moveDirection; //movement angle
    public float movementSpeed;//run speed
    public Vector3 speedCap; //used with movement speed

    [Header("Primary")]
    public int primary; //current primary ammo
    public int primaryMax; //max primary ammo
    public float primaryFireRate; //seconds between shots of primary
    public float primaryTimer; //timer used for firerate
    public float primaryReload; //time it takes to reload the primary
    public float primaryWalkSpeed; //how fast you can walk while shooting

    [Header("Secondary")]
    public int secondary; //current secondary ammo
    public int secondaryMax; //max secondary ammo
    public float secondaryFireRate; //seconds between shots of secondary
    public float secondaryTimer; //timer used for firerate

    [Header("Jumping")]
    public float airDrag;//drag strength when in air
    public float jumpForce;// jump power/height
    public float longJumpForce;// jump power/height for long jump
    public float primaryJumpForce;// jump power/height for long jump
    public float reloadJumpForce;// jump power/height for long jump

    public float airMultiplier;//how much less speed you have in the air
    public float jumpCooldown;//minimum time between jump in seconds
    public float jumpTimer;//used to measuer the cooldown
    public bool readyToJump;//reset after jumpCooldown

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
        //checks WASD every function except secondary does this so may as well do it here    
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        //checking if we are grounded because all states have a falling transition
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        //Manages the timers of primary, secondary and jump
        primaryTimer -= Time.deltaTime;
        secondaryTimer -= Time.deltaTime;
        jumpTimer -= Time.deltaTime;

        if(health <=0)
        {
            //kill player here/swap to death state more likely make it a state for better control
        }

        currentState.UpdateState(this);
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.CompareTag("Bullet"))
        {
           health -= other.gameObject.GetComponent<Bullet>().bulletDamage;
        }
    }

    void OnCollisionExit(Collision other){

    }

    //is called when a transition condition in our current states update is met
    public void SwitchState(CharacterBaseState state){
        //switches to the correct state and calls its enter function
        currentState = state;
        state.EnterState(this);
    }

}
