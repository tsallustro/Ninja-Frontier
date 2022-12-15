using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System;
namespace TeamNinja
{
    public class GoalTrigger : MonoBehaviour
    {
        [SerializeField] public SFXPlaying sfxPlaying;
        [SerializeField] GameObject goalObj;
        [SerializeField] int levelNum;
        [SerializeField] Timer timer;
        [SerializeField] int goldTime = 60;
        [SerializeField] int silverTime = 120;



        void OnTriggerEnter(Collider player)
        {
            Pauser.AbsolutePause();
            goalObj.SetActive(true);
            int timer_seconds = timer.GetTimeAsSeconds();
            int medal = CalculateMedal(timer_seconds);
            Debug.Log("timer seconds: " + timer_seconds + "medal: "+ medal);


            goalObj.GetComponent<StoryLineUI>().SetNoteBackgroundImage(medal);
            //sfxPlaying.Play_End_Goal();

            Save();

        }

        protected void Save()
        {
            int timer_seconds = timer.GetTimeAsSeconds();
            string timer_str = timer.GetTimeAsString();
            int medal = CalculateMedal(timer_seconds);
            FileSaver.Instance.SaveToActiveAndWrite(levelNum, medal, timer_str);

        }

        protected int CalculateMedal(int seconds)
        {
            if (seconds <= goldTime) return (int) MEDAL.GOLD;
            else if (seconds <= silverTime) return (int)MEDAL.SILVER;
            else return (int) MEDAL.BRONZE;
        }

        public int[] GetMedalTimes()
        {
            int[] times = new[] { goldTime, silverTime };
            return times;
        }
    }
}
