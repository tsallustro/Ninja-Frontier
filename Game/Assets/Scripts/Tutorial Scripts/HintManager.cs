using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace TeamNinja
{
    public class HintManager : MonoBehaviour
    {
        [SerializeField] private int secondsToDisplay = 5;
        [SerializeField] private GameObject hintPopUp;
        [SerializeField] private GameObject hintText;
        private float secondsRemaining;
        private TextMeshProUGUI hintTMP;
        private bool isShowing = false;
        // Start is called before the first frame update
        void Start()
        {
            hintPopUp.SetActive(false);
            hintTMP = hintText.GetComponent<TextMeshProUGUI>();
        }

        // Update is called once per frame
        void Update()
        {
            if(isShowing)
            {
               
                secondsRemaining -= Time.deltaTime;
                if (secondsRemaining <= 0) HideHint();
                
            }
        }

        public void ShowHint(string message)
        {
           
            isShowing = true;
            secondsRemaining = secondsToDisplay;
            hintTMP.text = message;
            hintPopUp.SetActive(true);
        }

        private void HideHint()
        {
           
            isShowing = false;
            hintPopUp.SetActive(false);
        }
    }
}
