using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TeamNinja
{
    public class Health : MonoBehaviour
    {
        [SerializeField] AudioSource HitSound;
        [SerializeField] AudioSource DeathSound;
        [SerializeField]
        private float hp;

        [SerializeField]
        private GameObject healthBar;

        public UnityEvent DamageTakenEvent;

        [SerializeField]
        private HUDManager hudManager;
        
        public UnityEvent DeathEvent;

        private float startingHp;
        void Start()
        {
            startingHp = hp;
        }


        public void DecreaseHealth()
        {
            hp--;
            if (hp == 0)
            {
                DeathSound.Play();
                DeathEvent.Invoke();
            }
            else if(hp > 0)
            {
                HitSound.Play();
                if (hudManager != null)
                    hudManager.SetHealthBar(hp / startingHp);
            }
        }

        public void ResetHealth()
        {
            hp = startingHp;
            if(healthBar != null)
                healthBar.GetComponent<Image>().fillAmount = 1;
        }
        public void IncreaseHealth(float healthUp)
        {
            hp = hp + healthUp;
            if (hp > startingHp)
                hp = startingHp;
            hudManager.SetHealthBar(hp / startingHp);
        }
    }
}
