using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class CustomGravity : MonoBehaviour
    {
        [SerializeField] private Rigidbody character;
        [SerializeField] private NewMovement nm;
        [SerializeField] private float gravityScale = 1.0f;
        private static float globalGravity = -9.18f;

        private float gravity = globalGravity;
        private bool hasHeightSet = false;
        private float playerHeight;

        void FixedUpdate()
        {
            if (!hasHeightSet) playerHeight = nm.getPlayerHeight();
            if(character.useGravity)character.velocity += gravity * gravityScale * Vector3.up * Time.fixedDeltaTime * playerHeight;
        }

        public void setGravity(float gravity)
        {
            this.gravity = gravity;
        }

        public void resetGravity()
        {
            gravity = globalGravity;
        }
    }
}
