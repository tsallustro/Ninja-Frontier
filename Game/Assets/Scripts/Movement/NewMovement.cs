using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace TeamNinja
{

    public class NewMovement : MonoBehaviour
    {
        [Header("Audio")]
        public AudioSource constantSource;
        public AudioSource singleSource;

        public AudioClip walking;
        public float minWalkingSpeed = 5f;
        public bool isWalking = false;
        public AudioClip sliding;
        public AudioClip wallRunning;
        public AudioClip jumping;
        public AudioClip slamming;
        public AudioClip slamFinish;
        public AudioClip dashing;

        //Serialised just for debug visibility.
        [Header("General")]
        public Transform character;
        public Transform camera;
        public MovementState state;
        public float desiredSpeed;
        public float moveSpeed;
        public float velocity;
        public bool hasJump;
        public float curStamina;
        public float maxStamina = 3f;
        public float staminaRecoveryTime = 1f;

        [Header("Input")]
        public bool isJumping;
        public bool isSprinting;
        public bool isCrouching;

        //Rework this with foldouts
        [Header("Movement Speeds")]
        public float walkSpeed = 100f;
        public float sprintSpeed = 200f;
        public float airSpeed = 150f;
        public float wallrunSpeed = 300f;

        [Header("Cooldowns")]
        public float jumpCooldown = 0.2f;
        public float curJumpCooldown;
        public float maxWallrunTime = 3f;
        public float curWallrunTime;

        [Header("Misc vars")]
        public LayerMask terrainMask; //Terrain instead of ground/wall so that it works with less effort.
        public float groundDrag = 5f;
        public float airDrag = 1f;
        public float crouchDrag = 4f;
        public float slideDrag = 3f;
        public float wallrunDrag = 3f;
        public float crouchScale = 0.5f;
        public float maxSlope = 45f;
        public Vector3 firstJump;
        public Vector3 secondJump;
        public Vector3 slideJump;
        public Vector3 wallJump;

        [Header("Sliding")]
        public float slideSpeed = 150f;
        public Vector3 slideDirection;
        public float slideTiltAngle = 5f;

        [Header("Dashing")]
        public float dashForce = 300f;
        public Vector3 dashDirection;
        public float dashCooldown = 0.5f;
        public float curDashCooldown = 0f;
        //public float gravityCounter = 1f;
        public float staminaDashCost = 1f;


        [Header("Wallrunning")]
        public float wallDistance = 1.0f;
        public float wallrunTiltAngle = 15f;

        [Header("Slamming")]
        public float slamSpeed = 200f;
        public float staminaSlamCost = 1f;

        [Header("Animation")]
        public Animator ThirdPersonNinja; //Probably not needed.
        public Animator FirstPersonNinja;

        private float playerHeight;
        private float startingScale;
        private CapsuleCollider capsule; //I just realised that we already have a reference to character, which is just this capsule... Oops.

        private RaycastHit floorHit;
        private RaycastHit leftWallHit;
        private RaycastHit rightWallHit;
        private bool wallLeft;
        private bool wallRight;

        private Vector3? previousWallrunNormal;
        private Vector3? currentWallrunNormal;

        private float playerSize;

        [SerializeField] private UnityEvent<float> OnStaminaUse;

        public enum MovementState
        {
            sliding,
            walking,
            sprinting,
            air,
            wallrunning,
            slamming
        }

        //Inputs
        private InputAction moveAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction crouchAction;

        //Things it can get itself to reduce clutter.
        Rigidbody rb;

        //Values that might be useful to share to other scripts
        bool isGrounded;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;

            capsule = GetComponentInChildren<CapsuleCollider>();
            playerHeight = rb.transform.localScale.y * capsule.transform.localScale.y * capsule.height/2;
            Debug.Log(playerHeight);
            startingScale = capsule.transform.localScale.y;

            playerSize = rb.transform.localScale.x * capsule.transform.localScale.x;

            isJumping = false;
            isSprinting = false;
            isCrouching = false;

            curJumpCooldown = 0f;

            previousWallrunNormal = null;
            currentWallrunNormal = null;

            curStamina = maxStamina;
        }

        public void Initialize(InputAction moveAction, InputAction jumpAction,
                                InputAction sprintAction, InputAction crouchAction)
        {
            this.moveAction = moveAction;
            moveAction.Enable();

            this.jumpAction = jumpAction;
            jumpAction.performed += jumpInput;
            jumpAction.Enable();

            this.sprintAction = sprintAction;
            sprintAction.performed += sprintInput;
            sprintAction.Enable();

            this.crouchAction = crouchAction;
            crouchAction.performed += crouchInput;
            crouchAction.Enable();
        }

        public float getPlayerHeight()
        {
            return playerHeight;
        }

        private void OnDestroy()
        {
            jumpAction.performed -= jumpInput;
            sprintAction.performed -= sprintInput;
            crouchAction.performed -= crouchInput;
        }

        //Input handling functions
        private void jumpInput(InputAction.CallbackContext obj)
        {
            isJumping = !isJumping;
        }

        private void sprintInput(InputAction.CallbackContext obj)
        {
            isSprinting = !isSprinting;

            if (isSprinting && curDashCooldown == 0 && curStamina >= staminaDashCost) dash();
        }

        private void crouchInput(InputAction.CallbackContext obj)
        {
            isCrouching = !isCrouching;
        }

        private void Update()
        {
            ReadUpdates();
            MovementState initial = state;
            MovementState previous;
            MovementState current;
            do
            {
                previous = state;
                StateUpdater();
                current = state;
                if(previous.Equals(MovementState.wallrunning)) previousWallrunNormal = currentWallrunNormal;
            } while (previous != current);
            if (initial != state)
            {
                //Debug.Log($"<color=#FFFFFF>State Changed:</color> <color=#FF0000>{initial}</color> <color=#FFFFFF>to</color> <color=#00FF00>{state}</color>");
                if (initial.Equals(MovementState.wallrunning)) previousWallrunNormal = currentWallrunNormal;
                if (state.Equals(MovementState.sliding)) rb.AddForce(Vector3.down*5, ForceMode.Impulse);

                //Constant Audio updates.
                constantSource.Stop();
                switch (state)
                {
                    case MovementState.slamming:
                        if (slamming == null) break;
                        constantSource.clip = slamming;
                        constantSource.Play();
                        break;
                    case MovementState.sliding:
                        if (sliding == null) break;
                        constantSource.clip = sliding;
                        constantSource.Play();
                        break;
                    /*case MovementState.walking:
                        if (walking == null) break;
                        constantSource.clip = walking;
                        constantSource.Play();
                        break;*/
                    case MovementState.wallrunning:
                        if (wallRunning == null) break;
                        constantSource.clip = wallRunning;
                        constantSource.Play();
                        break;
                    default:
                        //No audio by default. This should just be the air case, and air doesn't make noise.
                        //Walking is handled in its moveUpdate, as its sound is dependent on velocity.
                        break;
                }

                //State transition sounds
                if(initial.Equals(MovementState.slamming) && state.Equals(MovementState.walking) && slamFinish != null)
                {
                    singleSource.clip = slamFinish;
                    singleSource.Play();
                }
            }
            UpdateAnimatorFlags();
        }

        private void FixedUpdate()
        {
            velocity = rb.velocity.magnitude;
            MovePlayer();
        }

        private void ReadUpdates()
        {
            CheckGround();
            CheckWalls();
            updateTimers();
        }

        private void CheckGround()
        {
            //Raycast
            isGrounded = Physics.Raycast(transform.position, Vector3.down, out floorHit, playerHeight * 1.5f, terrainMask);
            //Determine if the angle of the potential floor is shallow enough to consider as walking.
            if (isGrounded)
            {
                float angle = Vector3.Angle(floorHit.normal, Vector3.up);
                if (angle > maxSlope) isGrounded = false;
            }
        }

        private void CheckWalls()
        {
            wallRight = Physics.Raycast(transform.position, capsule.transform.right, out rightWallHit, wallDistance*playerSize, terrainMask);
            wallLeft = Physics.Raycast(transform.position, -capsule.transform.right, out leftWallHit, wallDistance*playerSize, terrainMask);
        }
        private void updateTimers()
        {
            //This should be optimised somehow.
            if (curJumpCooldown > 0) curJumpCooldown -= Time.deltaTime;
            if (curJumpCooldown < 0) curJumpCooldown = 0;

            if (curDashCooldown > 0) curDashCooldown -= Time.deltaTime;
            if (curDashCooldown < 0) curDashCooldown = 0;
            
            if (state != MovementState.sliding && curStamina < maxStamina) curStamina += Time.deltaTime / staminaRecoveryTime;
            if (curStamina > maxStamina) curStamina = maxStamina;

            if (state.Equals(MovementState.wallrunning)) curWallrunTime += Time.deltaTime;

            UpdateGUIOnStamina();
        }

        private void StateUpdater()
        {
            //Handle changing states
            switch (state)
            {
                case MovementState.sliding:
                    UpdateSliding();
                    break;
                case MovementState.walking:
                    UpdateWalking();
                    break;
                case MovementState.sprinting:
                    UpdateSprinting();
                    break;
                case MovementState.air:
                    UpdateAir();
                    break;
                case MovementState.wallrunning:
                    UpdateWallrunning();
                    break;
                case MovementState.slamming:
                    UpdateSlamming();
                    break;
                default:
                    //What the hell.
                    Debug.Log("Somehow not in a movementState");
                    break;
            }
        }

        private void UpdateAnimatorFlags()
        {
            if (ThirdPersonNinja != null && FirstPersonNinja != null) { 
                //Sprinting
                ThirdPersonNinja.SetBool("IsSprinting", state == MovementState.sprinting);
                FirstPersonNinja.SetBool("IsSprinting", state == MovementState.sprinting);

                //Jumping
                ThirdPersonNinja.SetBool("IsJumping", state == MovementState.air);
                FirstPersonNinja.SetBool("IsJumping", state == MovementState.air);

                //Moving
                ThirdPersonNinja.SetBool("IsMoving", velocity > 0);
                FirstPersonNinja.SetBool("IsMoving", velocity > 0);
                FirstPersonNinja.SetFloat("Velocity", velocity);

            }
        }

        private void UpdateSliding()
        {
            if (!isCrouching) changeState(MovementState.walking);
            if (!isGrounded) changeState(MovementState.air);
        }

        private void UpdateWalking()
        {
            if (isCrouching) changeState(MovementState.sliding);
            if (isSprinting) changeState(MovementState.sprinting);
            if (!isGrounded) changeState(MovementState.air);
        }

        private void UpdateSprinting()
        {
            if (!isSprinting) changeState(MovementState.walking);
            if (isCrouching) changeState(MovementState.sliding);
            if (!isGrounded) changeState(MovementState.air);
        }

        private void UpdateAir()
        {
            //Check to see if we're grounded.
            if (isGrounded && !isSprinting) changeState(MovementState.walking);
            if (isGrounded && isSprinting) changeState(MovementState.sprinting);
            if (isValidWallrun()) changeState(MovementState.wallrunning);
            if (isCrouching && curStamina >= staminaSlamCost) changeState(MovementState.slamming);
            //if ((wallRight || wallLeft) && !isGrounded) changeState(MovementState.wallrunning);
        }

        private void UpdateWallrunning()
        {
            if (!wallRight && !wallLeft) changeState(MovementState.air);
            if (isGrounded && !isSprinting) changeState(MovementState.walking);
            if (isGrounded && isSprinting) changeState(MovementState.sprinting);
            if (curWallrunTime > maxWallrunTime) changeState(MovementState.air);
        }

        private void UpdateSlamming()
        {
            if (isSprinting) changeState(MovementState.air);
            if (isGrounded) changeState(MovementState.walking);
		}

        private bool isValidWallrun()
        {
            //Non-normal checks
            if (isGrounded) return false;
            if (!wallRight && !wallLeft) return false;

            //Check which wall we hit for use for the rest of it.
            RaycastHit hitToUse = wallRight ? rightWallHit : leftWallHit;

            //Cast to XZ plane (remove Y component)
            //Generate rays using points + normals
            //Return true if those rays intersect (object is concave)



            //Check to see 

            return !hitToUse.normal.Equals(previousWallrunNormal); //This line to allow it to compile.
        }

        private void changeState(MovementState state)
        {
            switch (state)
            {
                case MovementState.sliding:
                    previousWallrunNormal = null;
                    slideDirection = setSlideDirection();
                    rb.drag = slideDrag;
                    hasJump = true; //Also probably not necessary
                    setYScale(crouchScale);
                    rb.useGravity = true;
                    break;
                case MovementState.walking:
                    previousWallrunNormal = null;
                    rb.drag = groundDrag;
                    hasJump = true;
                    camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, 0f);
                    setYScale(startingScale);
                    rb.useGravity = true;
                    isWalking = false;
                    break;
                case MovementState.sprinting:
                    previousWallrunNormal = null;
                    rb.drag = groundDrag;
                    hasJump = true;
                    camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, 0f);
                    setYScale(startingScale);
                    rb.useGravity = true;
                    break;
                case MovementState.air:
                    rb.drag = airDrag;
                    // Allow for double jumps.
                    hasJump = true;
                    camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, 0f);
                    setYScale(startingScale);
                    rb.useGravity = true;
                    break;
                case MovementState.wallrunning:
                    //Stop vertical velocity.
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    //Tilt camera
                    camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, wallRight ? wallrunTiltAngle : -wallrunTiltAngle);
                    rb.drag = wallrunDrag;
                    hasJump = true;
                    setYScale(startingScale); //Not needed.
                    rb.useGravity = false;
                    curWallrunTime = 0;
                    break;
                case MovementState.slamming:
                    //Stop all velocity.
                    rb.velocity = new Vector3();
                    //All of this isn't needed but just to be safe.
                    camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, 0f);
                    rb.drag = airDrag;
                    setYScale(startingScale);
                    rb.useGravity = true;
                    curStamina -= staminaSlamCost;
                    UpdateGUIOnStamina();
                    break;
                default:
                    Debug.Log("Invalid MovementState");
                    break;
            }
            this.state = state;
        }

        private Vector3 setSlideDirection()
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            if (input.x > 0) camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, -slideTiltAngle);
            if (input.x < 0) camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, camera.rotation.eulerAngles.y, slideTiltAngle);
            if (input.magnitude == 0)
            {
                return capsule.transform.forward;
            }
            return character.forward * input.y
                                    + character.right * input.x;
        }

        private void setYScale(float scale)
        {
            capsule.transform.localScale = new(capsule.transform.localScale.x, scale, capsule.transform.localScale.z);
        }

        private void MovePlayer()
        {
            Vector3 moveDirection = character.forward * moveAction.ReadValue<Vector2>().y
                                    + character.right * moveAction.ReadValue<Vector2>().x;
            //Move differently based on playerState.
            switch (state)
            {
                case MovementState.sliding:
                    MoveSliding(moveDirection);
                    break;
                case MovementState.walking:
                    MoveWalking(moveDirection);
                    break;
                case MovementState.sprinting:
                    MoveSprinting(moveDirection);
                    break;
                case MovementState.air:
                    MoveAir(moveDirection);
                    break;
                case MovementState.wallrunning:
                    MoveWallrunning(moveDirection);
                    break;
                case MovementState.slamming:
                    MoveSlamming(moveDirection);
                    break;
                default:
                    //Something went wrong
                    Debug.Log("Somehow not in a movementState");
                    break;
            }
        }

        private void moveSlope(float moveSpeed, Vector3 moveDir)
        {
            //Don't make the player slide around the slope. This needs to be turned back on elsewhere then.
            rb.useGravity = false;

            //Calculate the direction the player wants to go in projected to the slope.
            Vector3 tmp = Vector3.Cross(floorHit.normal, moveDir);
            Vector3 slopeDir = Vector3.Cross(tmp, floorHit.normal);

            //Check to see if the resulting direction is away from moveDir, and if so negate flip it.
            if((moveDir+slopeDir).magnitude < (moveDir - slopeDir).magnitude)
            {
                slopeDir = -slopeDir;
            }

            rb.AddForce(slopeDir.normalized * moveSpeed*playerSize, ForceMode.Force);

        }

        private float slowSpeedGround(float desiredSpeed, float moveSpeed)
        {
            return desiredSpeed;
        }

        private float slowSpeedAir(float desiredSpeed, float moveSpeed)
        {
            return desiredSpeed;
        }

        private float slowSpeedWallrun(float desiredSpeed, float moveSpeed)
        {
            return desiredSpeed;
        }

        private void tryGroundJump()
        {
            if (curJumpCooldown <= 0)
            {
                Vector3 localForce = capsule.transform.right * firstJump.x +
                                    capsule.transform.up * firstJump.y +
                                    capsule.transform.forward * firstJump.z;
                rb.AddForce(localForce*playerHeight, ForceMode.Impulse);
                curJumpCooldown = jumpCooldown;
                // Don't turn off hasJump because if they transition to another state after leaving the ground it should handle it.

                if (jumping != null)
                {
                    singleSource.clip = jumping;
                    singleSource.Play();
                }
            }
        }

        private void trySlideJump()
        {
            if (hasJump && curJumpCooldown <= 0)
            {
                Vector3 localForce = capsule.transform.right * slideJump.x +
                                    capsule.transform.up * slideJump.y +
                                    capsule.transform.forward * slideJump.z;
                rb.AddForce(localForce * playerHeight, ForceMode.Impulse);
                curJumpCooldown = jumpCooldown;
                // No flying.
                hasJump = false;

                if (jumping != null)
                {
                    singleSource.clip = jumping;
                    singleSource.Play();
                }
            }
        }

        private void tryAirJump()
        {
            if (hasJump && curJumpCooldown <= 0)
            {
                Vector3 localForce = capsule.transform.right * secondJump.x +
                                    capsule.transform.up * secondJump.y +
                                    capsule.transform.forward * secondJump.z;
                rb.AddForce(localForce*playerHeight, ForceMode.Impulse);
                curJumpCooldown = jumpCooldown;
                // No flying.
                hasJump = false;

                if (jumping != null)
                {
                    singleSource.clip = jumping;
                    singleSource.Play();
                }
            }
        }

        private void tryWallJump()
        {
            if (hasJump && curJumpCooldown <= 0)
            {
                Vector3 localForce = capsule.transform.right * (wallRight ? -wallJump.x : wallJump.x) +
                                    capsule.transform.up * wallJump.y +
                                    capsule.transform.forward * wallJump.z;
                rb.AddForce(localForce*playerHeight, ForceMode.Impulse);
                curJumpCooldown = jumpCooldown;
                hasJump = false;

                if(jumping != null)
                {
                    singleSource.clip = jumping;
                    singleSource.Play();
                }
            }
        }

        private Vector3 calcWallrunDirection(Vector3 moveDirection)
        {
            //TODO: Move maths from the movewallrunning function into here to increase readability.
            return moveDirection;
        }

        private bool movingTowardsWall(Vector3 moveDirection, Vector3 wallNormal)
        {
            return (moveDirection - wallNormal).magnitude > (moveDirection + wallNormal).magnitude;
        }

        private void MoveSliding(Vector3 moveDirection)
        {
            moveSpeed = slideSpeed;
            if (!floorHit.normal.Equals(Vector3.up)) moveSlope(moveSpeed, slideDirection);
            else rb.AddForce(slideDirection.normalized * moveSpeed* playerSize, ForceMode.Force);
            if (isJumping) trySlideJump();
        }

        private void MoveWalking(Vector3 moveDirection)
        {
                rb.useGravity = true; //What a fine plate of pasta.
                desiredSpeed = walkSpeed;
                moveSpeed = slowSpeedGround(desiredSpeed, moveSpeed);
                if (!floorHit.normal.Equals(Vector3.up)) moveSlope(moveSpeed, moveDirection); 
                else rb.AddForce(moveDirection.normalized * moveSpeed* playerSize, ForceMode.Force);
            
            if (isJumping) tryGroundJump();

            if (!isWalking)
            {
                //Going from not walking to walking fast enough for steps.
                if (rb.velocity.magnitude >= minWalkingSpeed)
                {
                    isWalking = true;
                    if(walking != null)
                    {
                        constantSource.clip = walking;
                        constantSource.Play();
                    }
                }
            }
            else
            {
                if(rb.velocity.magnitude < minWalkingSpeed)
                {
                    isWalking = false;
                    constantSource.Stop();
                }
            }
        }

        private void dash()
        {
            curStamina -= staminaDashCost;
            //Stop movement to make it feel slighlty more consistent
            rb.velocity = new Vector3();
            Vector2 input = moveAction.ReadValue<Vector2>();
            Vector3 moveDirection = character.forward * input.y
                                    + character.right * input.x;
            if (input.magnitude == 0) moveDirection = character.forward;

            //if (!isGrounded) moveDirection += Vector3.up * gravityCounter;
            rb.AddForce(moveDirection.normalized * dashForce * playerSize, ForceMode.Impulse);
            
            //Playsound
            if(dashing != null)
            {
                singleSource.clip = dashing;
                singleSource.Play();
            }
            UpdateGUIOnStamina();
        }

        private void MoveSprinting(Vector3 moveDirection)
        {
            desiredSpeed = sprintSpeed;
            moveSpeed = slowSpeedGround(desiredSpeed, moveSpeed);
            if (!floorHit.normal.Equals(Vector3.up)) moveSlope(moveSpeed, moveDirection);
            else rb.AddForce(moveDirection.normalized * moveSpeed* playerSize, ForceMode.Force);
            if (isJumping) tryGroundJump();
        }

        private void MoveAir(Vector3 moveDirection)
        {
            desiredSpeed = airSpeed;
            // Don't slow the speed mid air.
            moveSpeed = slowSpeedAir(desiredSpeed, moveSpeed);
            rb.AddForce(moveDirection.normalized * moveSpeed* playerSize, ForceMode.Force);
            if (isJumping) tryAirJump();
        }

        private void MoveWallrunning(Vector3 moveDirection)
        {
            //Get wall normal
            Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;
            currentWallrunNormal = wallNormal; //Update while wallrunning.
            Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up); //Rather than transform.up use this because the transform should skew.

            //Figure out which way we're facing along the wall
            if ((capsule.transform.forward - wallForward).magnitude > (capsule.transform.forward + wallForward).magnitude)
            {
                wallForward = -wallForward;
            }

            rb.AddForce(wallForward * wallrunSpeed* playerSize, ForceMode.Force);

            //Apply force to keep playerOnWall.
            if (movingTowardsWall(moveDirection, wallNormal))
            {
                rb.AddForce(-wallNormal * 10f* playerSize, ForceMode.Force);
            }
            else
            {
                rb.AddForce(wallNormal * 10f* playerSize, ForceMode.Force);
            }

            if (isJumping) tryWallJump(); //Replace with walljump soon(tm)
        }

        private void MoveSlamming(Vector3 moveDirection)
		{
            rb.AddForce(new Vector3(0, -slamSpeed, 0) * playerSize, ForceMode.Force);
		}

        private void UpdateGUIOnStamina()
        {
            float pct = curStamina / maxStamina;
            OnStaminaUse.Invoke(pct);

        }
    }
}
