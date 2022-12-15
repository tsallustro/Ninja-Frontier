using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TeamNinja
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] GameObject objectToFollow;
        private bool follow = true;

        [SerializeField] UnityEvent changeObjectToFollow;
        [SerializeField] private float transitionTime = 1f;

        private void FixedUpdate()
        {
            if (follow)
            {
                gameObject.transform.position = objectToFollow.transform.position;
            }
        }

        public void ChangeObjectToFollow(GameObject go)
        {
            StartCoroutine(ChangingObjectToFollow(go));
        }

        public IEnumerator ChangingObjectToFollow(GameObject go)
        {
            Vector3 oldPosition = objectToFollow.transform.position;
            follow = false;
            float t = 0f;

            while (gameObject.transform.position != go.transform.position)
            {
                Debug.Log(gameObject.transform.position);

                t += Time.deltaTime / transitionTime;

                gameObject.transform.position = Vector3.Lerp(oldPosition, go.transform.position, t);

                yield return null;
            }

            objectToFollow = go;

            follow = true;
        }
    }
}
