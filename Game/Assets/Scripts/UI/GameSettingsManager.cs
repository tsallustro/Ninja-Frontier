using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    public class GameSettingsManager : NewSettingsManager
    {
        
        protected override void OnReturnButtonClick()
        {
            Debug.Log("GAME SETTINGS MENU: RETURN CLICKED");
            GameUIManager.HideSettings();
            GameUIManager.ShowPauseMenu();
        }
    }
}
