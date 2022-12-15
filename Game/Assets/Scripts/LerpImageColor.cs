using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TeamNinja;

namespace TeamNinja
{
    public class LerpImageColor : MonoBehaviour
    {

        public IEnumerator LerpingColor(Color startColor, Color endColor, float transitionTime)
        {

            float t = 0f;
            while (gameObject.GetComponent<RawImage>().color != endColor)
            {
                t += Time.deltaTime / transitionTime;

                gameObject.GetComponent<RawImage>().color = Color.Lerp(startColor, endColor, t);

                yield return null;
            }
        }
    }
}
