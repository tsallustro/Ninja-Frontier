using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Barrel : MonoBehaviour
    {

        [SerializeField] float explosionTimer = 4f;
        [SerializeField] GameObject trigger;
        [SerializeField] ParticleSystem smoke;
        [SerializeField] ParticleSystem explosion;

        private void Start()
        {
            trigger.SetActive(false);
         
        }

        public void HitBarrel(Vector3 dir, float speed)
        {
            smoke.Play();
            gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * speed, ForceMode.Impulse);
            StartCoroutine(ExplosionTimer());

        }

        public IEnumerator ExplosionTimer()
        {

            yield return new WaitForSeconds(explosionTimer);
            trigger.SetActive(true);
            explosion.Play();

            yield return new WaitForSeconds(.75f);
            gameObject.SetActive(false);

        }

    }
}
