using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Mine : MonoBehaviour
    {
        [SerializeField] AudioSource explode;
        [SerializeField] float explosionTimer = .05f;
        [SerializeField] GameObject trigger;
        [SerializeField] ParticleSystem explosion;
        [SerializeField] float launchForce;

        private void Start()
        {
            trigger.SetActive(false);

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(ExplosionTimer());
                Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>();
                Vector3 forceDir = other.gameObject.transform.position-transform.position;
                rb.AddForce(forceDir.normalized*launchForce, ForceMode.Impulse);
            }
        }

        public IEnumerator ExplosionTimer()
        {
            yield return new WaitForSeconds(explosionTimer);

            trigger.SetActive(true);
            explode.Play();
            explosion.Play();

            yield return new WaitForSeconds(.75f);
            gameObject.SetActive(false);

        }

    }
}
