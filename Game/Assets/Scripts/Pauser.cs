using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class Pauser
    {
        private static Pauser Instance = new();

        private bool paused = false;


        public static void TogglePause()
        {
            Instance.paused = !(Instance.paused);
            if (!Instance.paused)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            Debug.Log("PAUSED: " + Instance.paused);
        }

        public static void AbsolutePause()
        {
            if (!Instance.paused) TogglePause();
        }

        public static void AbsoluteUnpause()
        {
            if (Instance.paused) TogglePause();
        }
        public static bool GetPauseState()
        {
            return Instance.paused;
        }
    }
}
