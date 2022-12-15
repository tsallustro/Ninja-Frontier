using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TeamNinja
{
    public class Knight : MonoBehaviour
    {
        [SerializeField]
        private float walkSpeed = 3f;
        [SerializeField]
        private float runSpeed = 5f;
        [SerializeField]
        private float attackSpeed = 7f;


        private LookToward lookToward;
        private Animator anim;

        private string currentState = "isIdle";

        private Dictionary<string, bool> stateMap =
            new Dictionary<string, bool> { { "isIdle", true }, { "isWalking", false }, { "isCharging", false }, { "isAttacking", false }, { "isDead", false } };

        private bool isInCombat = false;

        [SerializeField]
        private GameObject player;
        [SerializeField]
        private BoxCollider swordCollider;

        [SerializeField]
        private float time = 1f;

        private Rigidbody rb;


        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, 0);
            anim = gameObject.GetComponent<Animator>();
            lookToward = gameObject.GetComponent<LookToward>();
        }

        private IEnumerator CombatLoop()
        {
            while (isInCombat)
            {
                CheckState();

                yield return new WaitForFixedUpdate();
            }

            yield return null;

        }

        private void CheckState()
        {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if (distance < 10)
            {
                ChangeState("isAttacking");
                ChangeVelocity(attackSpeed);
            }
            else if (distance < 15)
            {
                ChangeState("isCharging");
                ChangeVelocity(runSpeed);
            }
            else if (distance < 40)
            {
                ChangeState("isWalking");
                ChangeVelocity(walkSpeed);
            }
        }

        private void ChangeState(string newState)
        {
            if (newState != currentState)
            {
                stateMap[currentState] = false;
                stateMap[newState] = true;

                anim.SetBool(currentState, false);
                anim.SetBool(newState, true);

                currentState = newState;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && !isInCombat && !currentState.Equals("isDead"))
            {
                lookToward.SetLooking(true);
                isInCombat = true;
                StartCoroutine(CombatLoop());
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && isInCombat && Vector3.Distance(gameObject.transform.position, player.transform.position) > 20)
            {
                //TODO: Figure out how to avoid this being called when the enemy hits the player
                isInCombat = false;
                ChangeState("isIdle");
                ChangeVelocity(0);
                lookToward.SetLooking(false);
            }
        }

        public void Die()
        {
            if(!currentState.Equals("isDead"))
            {
                swordCollider.enabled = false;
                isInCombat = false;
                ChangeState("isDead");
                ChangeVelocity(0);
                lookToward.SetLooking(false);
                ItemManager.Instance.DropItem(this.gameObject.transform.position);
                StartCoroutine(Despawn());
            }
        }

        IEnumerator Despawn()
        {
            yield return new WaitForSeconds(time);
            this.gameObject.SetActive(false);
        }

        public void ChangeVelocity(float speed)
        {
            rb.velocity = transform.forward * speed;
        }
    }
}
