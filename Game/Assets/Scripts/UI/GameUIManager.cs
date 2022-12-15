using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] GameObject PauseMenuObj, SettingsMenuObj, DeathUIObj, HUDObj, EndGoalObj;
        private static GameObject _Pause, _Settings, _Death, _HUD, _EndGoal;

        private void OnEnable()
        {
            _Pause = PauseMenuObj;
            _Settings = SettingsMenuObj;
            _Death = DeathUIObj;
            _HUD = HUDObj;
            _EndGoal = EndGoalObj;

            //Make sure everything is hidden when level starts
            HidePauseMenu();
            HideSettings();
            ShowHUD();

            Pauser.AbsoluteUnpause();
        }
        public static void ShowPauseMenu()
        {
            _Pause.SetActive(true);
        }
        public static void HidePauseMenu()
        {
            _Pause.SetActive(false);
        }
        public static void ShowSettings()
        {
            _Settings.SetActive(true);
        }
        public static void HideSettings()
        {
            _Settings.SetActive(false);
        }
        public static void ShowDeathUI()
        {
            _Death.SetActive(true);
        }
        public static void HideDeathUI()
        {
            _Death.SetActive(false);
        }
        public static void ShowHUD()
        {
            _HUD.SetActive(true);
        }
        public static void HideHUD()
        {
            _HUD.SetActive(false);
        }
        public static void ShowEndGoal()
        {
            _EndGoal.SetActive(true);
        }
        public static void HideEndGoal()
        {
            _EndGoal.SetActive(false);
            
        }
        public static void HidePauseAndSettings()
        {
            HideSettings();
            HidePauseMenu();
        }
    }
}
