using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamNinja
{
    public class PauseMenuController : MonoBehaviour
    {

        private UIDocument _doc;
        private Button _returnButton, _settingsButton, _quitButton;

        private void OnEnable()
        {
            
            _doc = GetComponent<UIDocument>();
            _returnButton = _doc.rootVisualElement.Q<Button>("ReturnButton");
            _returnButton.clicked += OnReturnButtonClick;

            _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
            _settingsButton.clicked += OnSettingsButtonClick;

            _quitButton = _doc.rootVisualElement.Q<Button>("QuitButton");
            _quitButton.clicked += OnQuitButtonClick;

            _returnButton.Focus();

        }
        private void OnReturnButtonClick()
        {
            Debug.Log("SETTINGS MENU: RETURN CLICKED");
            GameUIManager.HidePauseMenu();
            Pauser.TogglePause();
        }

        private void OnSettingsButtonClick()
        {
            GameUIManager.HidePauseMenu();
            GameUIManager.ShowSettings();
        }

        private void OnQuitButtonClick()
        {
            Pauser.TogglePause();
            Quitter.QuitToMainMenu();
        }
    }
}
