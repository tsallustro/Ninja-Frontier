using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TeamNinja
{
    public class BasicItem : MonoBehaviour
    {
        
        //[SerializeField] protected AudioSource basic_item_pick_up;
        [SerializeField] protected bool isEnemyItem = false;
        [SerializeField] private float time = 2.0f;
        [SerializeField] private float transformSpeed = 1.0f;
        [SerializeField] private float scale = 5.0f;
        [SerializeField] private Vector3 PickupRotation;
        private Vector3 targetPosition;
        private float height = 1.0f;
        protected bool beginAnimation = false;
        protected bool canPickUp = true;
        protected Rigidbody Body;
        
        public virtual void UseItem(Vector3 Position, Quaternion Rotation)
        {
            gameObject.SetActive(true);
            canPickUp = false;
            transform.rotation = Rotation;
            transform.position = Position + transform.forward;
        }
        public virtual void Awake()
        {
            Body = gameObject.GetComponent<Rigidbody>();
            if(!isEnemyItem)
                StartCoroutine(GetBeeg(transform.localScale * scale));
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if(!isEnemyItem)
            {
                if (other.gameObject.CompareTag("Player") && canPickUp)
                {
                    other.gameObject.GetComponentInParent<Inventory>().AddItem(this);
                    gameObject.SetActive(false);
                }
                else if(other.gameObject.CompareTag("Enemy") && !canPickUp)
                {
                    other.gameObject.GetComponentInParent<Health>().DamageTakenEvent.Invoke();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponentInParent<Health>().DamageTakenEvent.Invoke();
                    gameObject.SetActive(false);
                }
            }
        }

        protected virtual void DisableHitbox()
        {
            Body.useGravity = false;
            Body.isKinematic = true;
        }

        IEnumerator GetBeeg(Vector3 targetScale)
        {
            while (!beginAnimation)
            {
                yield return new WaitForSeconds(1);
                if(Body.velocity.magnitude == 0)
                {
                    this.DisableHitbox();
                    beginAnimation = true;
                    targetPosition = transform.position + Vector3.up * height;
                }
                yield return null;
            }
            yield return new WaitForSeconds(time);
            canPickUp = true;
            if (gameObject.TryGetComponent<Animator>(out Animator IdleAnimation))
                IdleAnimation.enabled = true;
            while (transform.localScale != targetScale && transform.localPosition != targetPosition && transform.rotation != Quaternion.Euler(PickupRotation))
            {
                transform.localScale = Vector3.Lerp(transform.localScale, targetScale, transformSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPosition, transformSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(PickupRotation), transformSpeed * Time.deltaTime);
                yield return null;
            }

        }
    }
}
