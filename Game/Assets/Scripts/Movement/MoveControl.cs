using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TeamNinja
{
    public class MoveControl : MonoBehaviour
    {
        [SerializeField] Rigidbody character;
        [SerializeField] float sprintMultiplier = 1.5f;
        [SerializeField] float frictionCoeff = 0.005f;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float groundAcceleration = 10f;
        [SerializeField] float groundMaxSpeed = 5f;
        [SerializeField] float airAcceleration = 15f;
        [SerializeField] float airMaxSpeed = 30f;
        [SerializeField] float heightForRespawn = -10;
        [SerializeField] Animator ThirdPersonNinja;
        [SerializeField] Animator FirstPersonNinja;
        [SerializeField] GameObject respawnPoint;
        [SerializeField] CustomGravity customGravity;
        [SerializeField] GameObject cap;

        private InputAction moveAction;
        private InputAction sprintAction;
        private InputAction jumpAction;
        private InputAction crouchAction;
        //private GameObject currentWall = null;

        private bool isSprinting = false;
        private bool isCrouching = false;
        public bool isGrounded = true;
        public bool isWallrunning = false;
        public bool hasDoubleJump = true;

        public void Initialize(InputAction moveAction, InputAction sprintAction, InputAction jumpAction, InputAction crouchAction)
        {
            this.moveAction = moveAction;
            this.sprintAction = sprintAction;
            this.jumpAction = jumpAction;
            this.crouchAction = crouchAction;

            this.moveAction.Enable();

            this.sprintAction.performed += toggleSprint;
            this.sprintAction.Enable();

            this.jumpAction.performed += tryJump;
            this.jumpAction.Enable();

            this.crouchAction.performed += toggleCrouch;
            this.crouchAction.Enable();
        }

        private void OnDestroy()
        {
            sprintAction.performed -= toggleSprint;
            jumpAction.performed -= tryJump;
            crouchAction.performed -= toggleCrouch;
        }

        private void toggleSprint(InputAction.CallbackContext obj)
        {
            isSprinting = !isSprinting;
            ThirdPersonNinja.SetBool("IsSprinting", isSprinting);
            FirstPersonNinja.SetBool("IsSprinting", isSprinting);
            //No, you can't sprint while in the air.
            groundAcceleration = isSprinting ? (groundAcceleration * sprintMultiplier) : (groundAcceleration / sprintMultiplier);
        }

        private void toggleCrouch(InputAction.CallbackContext obj)
		{
            isCrouching = !isCrouching;
            cap.transform.localScale = new(cap.transform.localScale.x, 
                isCrouching ? 0.5f : 1f,
                cap.transform.localScale.z);
		}

        private void tryJump(InputAction.CallbackContext obj)
		{
            //Probably don't need to update isGrounded/isWallrunning, but better safe than sorry.
            if (isGrounded)
            {
                character.velocity += new Vector3(0f, jumpForce, 0f);
                isGrounded = false;
                ThirdPersonNinja.SetBool("IsJumping", true);
                FirstPersonNinja.SetBool("IsJumping", true);
                return;
            }
			if (isWallrunning)
			{
                //TODO: Make this launch at an angle relative to the gameobject.
                character.velocity += new Vector3(0f, jumpForce / 2, jumpForce / 2);
                isWallrunning = false;
                return;
			}
			if (hasDoubleJump)
			{
                character.velocity += new Vector3(0f, jumpForce, 0f);
                hasDoubleJump = false;
                ThirdPersonNinja.SetBool("IsJumping", true);
                FirstPersonNinja.SetBool("IsJumping", true);
                return;
			}
            
		}

        private void FixedUpdate()
        {
            Vector2 input = moveAction.ReadValue<Vector2>();
            ThirdPersonNinja.SetBool("IsMoving", input.x != 0 || input.y != 0);
            FirstPersonNinja.SetBool("IsMoving", input.x != 0 || input.y != 0);

            UpdateVelocity(input);            
            UpdateRespawn();
        }

        private void UpdateVelocity(Vector2 input)
        {
            Vector3 wishdir = new(input.x, 0, input.y);
            // Update to use local rotation.
            wishdir = character.gameObject.transform.localRotation * wishdir;

            // Update the velocity
            character.velocity = (isGrounded || isWallrunning) ? MoveGround(wishdir, character.velocity) : MoveAir(wishdir, character.velocity);
        }

        private Vector3 MoveGround(Vector3 wishdir, Vector3 prevDir)
        {
            // Apply friction
            Vector3 horizontal = new(prevDir.x, 0, prevDir.z);  // Get 2D plane.
            float speed = horizontal.magnitude;                 // Speed for friction calculation

            if(speed != 0)
            {
                float drop = speed * frictionCoeff * Time.fixedDeltaTime;
                prevDir *= Mathf.Max(speed - drop, 0) / speed;  // Scale by how much friction dropped it off.
            }

            return Accelerate(wishdir, prevDir, groundAcceleration, groundMaxSpeed);
        }

        private Vector3 MoveAir(Vector3 wishdir, Vector3 prevDir)
        {
            // Assume air resistance is negligible and cow is spherical.
            return Accelerate(wishdir, character.velocity, airAcceleration, airMaxSpeed);
        }

        // "Cite your source" is a bit too literal with this one. Modified slightly.
        // https://github.com/ValveSoftware/source-sdk-2013/blob/56accfdb9c4abd32ae1dc26b2e4cc87898cf4dc1/sp/src/game/shared/gamemovement.cpp#L1822
        private Vector3 Accelerate(Vector3 wishdir, Vector3 prevDir, float accel, float maxSpeed)
        {
            //Not actually current speed, but instead the projection in the direction they want to go.
            float curSpeed = Vector3.Dot(prevDir, wishdir);
            float addSpeed = accel * Time.fixedDeltaTime; //Todo: Figure out a better name.

            //Cap speed.
            if(curSpeed+addSpeed > maxSpeed) addSpeed = maxSpeed - curSpeed;

            return prevDir + addSpeed * wishdir;
        }

        private void UpdateRespawn()
        {
            if (character.transform.position.y < heightForRespawn) character.GetComponent<Respawner>().RespawnEventStart();
        }
    }

}
