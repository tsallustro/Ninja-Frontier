using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace TeamNinja
{
    public class EndGoal : MonoBehaviour
    {

        private UIDocument _doc;
        private Button _continueButton, _replayutton, _quitButton;

        private void OnEnable()
        {
            Debug.Log("END GOAL: ENABLED");
            _doc = GetComponent<UIDocument>();

            _continueButton = _doc.rootVisualElement.Q<Button>("ContinueButton");
            _continueButton.clicked += OnContinueButtonClick;

            _replayutton = _doc.rootVisualElement.Q<Button>("ReplayButton");
            _replayutton.clicked += OnReplayButtonClick;

            _quitButton = _doc.rootVisualElement.Q<Button>("QuitButton");
            _quitButton.clicked += OnQuitButtonClick;

            _continueButton.Focus();
            Pauser.AbsolutePause();
        }

        private void OnReplayButtonClick()
        {
            Pauser.AbsoluteUnpause();
            GameStateManager.Instance.LoadLevel(GameStateManager.Instance.currentLevel);

        }

        private void OnQuitButtonClick()
        {
            Pauser.AbsoluteUnpause();

            GameStateManager.Instance.LoadLevel(GameStateManager.Instance.mainMenuSceneNum);

        }

        private void OnContinueButtonClick()
        {
            Pauser.AbsoluteUnpause();

            GameStateManager.Instance.LoadLevel(GameStateManager.Instance.currentLevel + 1);
        }
    }
}
