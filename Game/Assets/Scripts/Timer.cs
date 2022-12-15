using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

namespace TeamNinja
{
    public class Timer : MonoBehaviour
    {
        private float currentTime;
        private TextMeshProUGUI currentTimeText;
        // Start is called before the first frame update
        void Start()
        {
            currentTimeText = this.GetComponent<TextMeshProUGUI>();
            currentTime = 0;
        }

        // Update is called once per frame
        void Update()
        {
            currentTime += Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            currentTimeText.text = "Time: " + time.ToString(@"mm\:ss\:fff");
        }

        public string GetTimeAsString()
        {
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            return time.ToString(@"mm\:ss\:fff");
        }
        public int GetTimeAsSeconds()
        {
            return (int)currentTime;
        }
    }
}
