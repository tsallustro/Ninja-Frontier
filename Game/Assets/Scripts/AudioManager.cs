using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class AudioManager : MonoBehaviour
    {

        public void PlayAudioClip(AudioClip clip)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(clip);
        }

    }
}
