using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TeamNinja;
using System.Runtime.InteropServices;

namespace TeamNinja
{
    public class CombatControl : MonoBehaviour
    {

        [SerializeField] private Transform cameraTransform;
        private float initialSwordDistance;
        [SerializeField] private float SwordDistance;
        [SerializeField] private float RangeDistance;
        [SerializeField] private Inventory PlayerInventory;
        [SerializeField] private ThrowShuriken PlayerShuriken;
        [SerializeField] Animator RightHand;
        [SerializeField] Animator LeftHand;
        [SerializeField] AudioSource swordSound;
        [SerializeField] AudioSource shurikenSound;
        private InputAction swingSwordInput;
        private InputAction useSecondaryItem;

        public void Initialize(InputAction swingSwordInput, InputAction useSecondaryItem)
        {
            this.swingSwordInput = swingSwordInput;
            this.useSecondaryItem = useSecondaryItem;
            this.swingSwordInput.performed += SwordAction;
            this.swingSwordInput.Enable();
            this.useSecondaryItem.performed += ThrowItem;
            this.useSecondaryItem.Enable();

        }
        public void Start()
        {
            initialSwordDistance = SwordDistance;
        }
        public void OnDestroy()
        {
            swingSwordInput.performed -= SwordAction;
            useSecondaryItem.performed -= ThrowItem;
        }
        private void SwordAction(InputAction.CallbackContext obj)
        {
            if(obj.performed)
            {
                swordSound.Play();
                RightHand.Play("SwordAttack");
            }
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);

            if(localVelocity.z > 0)
            {
                SwordDistance = initialSwordDistance + localVelocity.z / 2;
            }


            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * SwordDistance, Color.cyan);
            RaycastHit[] hitObjects;
            hitObjects = Physics.RaycastAll(cameraTransform.position, cameraTransform.forward, SwordDistance, ~LayerMask.GetMask("Player", "HintTrigger"));

            for(int i = 0; i < hitObjects.Length; i++)
            {
                RaycastHit ObjectHit = hitObjects[i];

                Debug.Log(ObjectHit.transform.name);
                if (ObjectHit.transform.CompareTag("Destructable"))
                {
                    ObjectHit.transform.GetComponent<DestructableObject>().ShatterObject(ObjectHit.point);
                }
                else if (ObjectHit.transform.CompareTag("Barrel"))
                {
                    // hit the barrel based on the players velocity
                    ObjectHit.transform.GetComponent<Barrel>().HitBarrel(ObjectHit.point - gameObject.transform.position, SwordDistance + localVelocity.z / 2);
                }
                else if (ObjectHit.transform.CompareTag("Enemy") && !ObjectHit.collider.isTrigger)
                {
                    ObjectHit.transform.gameObject.GetComponentInParent<Health>().DamageTakenEvent.Invoke();
                }

            }
        }

        private void ThrowItem(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                LeftHand.Play("ShurikenThrow");
            }
            PlayerInventory.UseItem();
        }
    }
}
