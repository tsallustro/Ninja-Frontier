using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

namespace TeamNinja
{
    public class HUDManager : MonoBehaviour
    {

        [SerializeField] private GameObject healthBar, staminaBar,RShurikenBackground, EShurikenBackground, RShurikenCount, EShurikenCount,medalObj;
        [SerializeField] private Sprite shurikenUnselected, shurikenSelected;

        [SerializeField] private Timer timer;
        [SerializeField] private GoalTrigger trigger;
        [SerializeField] private Sprite goldMedal, silverMedal, bronzeMedal;
        private int selected = 1; //1 = regular, -1 = exploding

        private int[] times;

        public void OnEnable()
        {
            times = trigger.GetMedalTimes();
        }

        public void Update()
        {
            int currentTime = timer.GetTimeAsSeconds();

            if (currentTime < times[0] && medalObj.GetComponent<Image>().sprite != goldMedal) medalObj.GetComponent<Image>().sprite = goldMedal;
            else if (currentTime >= times[0] && currentTime < times[1] && medalObj.GetComponent<Image>().sprite != silverMedal) medalObj.GetComponent<Image>().sprite = silverMedal;
            else if (currentTime >= times[1] && medalObj.GetComponent<Image>().sprite != bronzeMedal) medalObj.GetComponent<Image>().sprite = bronzeMedal;
        
        }

        public void SetHealthBar(float pctHealthRemaining)
        {
            healthBar.GetComponent<Image>().fillAmount = pctHealthRemaining;
        }

        public void SetStaminaBar(float pctStaminaRemaining)
        {
            staminaBar.GetComponent<Image>().fillAmount = pctStaminaRemaining;
        }


        public void SwitchShuriken()
        {
            selected = -selected;
            if (selected == 1)
            {
                RShurikenBackground.GetComponent<Image>().sprite = shurikenSelected;
                EShurikenBackground.GetComponent<Image>().sprite = shurikenUnselected;
            }

            else
            {
                EShurikenBackground.GetComponent<Image>().sprite = shurikenSelected;
                RShurikenBackground.GetComponent<Image>().sprite = shurikenUnselected;
            }

        }

        public void ResetItemCounts(Dictionary<string,int> inventory)
        {


           RShurikenCount.GetComponent<TMP_Text>().text = inventory["Shuriken"].ToString();
           EShurikenCount.GetComponent<TMP_Text>().text = inventory["ExplosiveShuriken"].ToString();

        }


    }
}
