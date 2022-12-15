using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamNinja
{
    public class NewLevelSelectManager : MonoBehaviour
    {
       
        [SerializeField] Sprite bronzeMedalImage;
        [SerializeField] Sprite silverMedalImage;
        [SerializeField] Sprite goldMedalImage;
        private UIDocument _doc;

        private Button _tutorialButton, _level1Button, _returnButton, _level2Button, _level3Button;
        private void OnEnable()
        {
            _doc = GetComponent<UIDocument>();

            _tutorialButton = _doc.rootVisualElement.Q<Button>("TutorialButton");
            _tutorialButton.clicked += OnTutorialButtonClick;

            _level1Button = _doc.rootVisualElement.Q<Button>("Level1Button");
            _level1Button.clicked += OnLevel1ButtonClick;

            _returnButton = _doc.rootVisualElement.Q<Button>("BackButton");
            _returnButton.clicked += OnBackButtonClick;

            _level2Button = _doc.rootVisualElement.Q<Button>("Level2Button");
            _level2Button.clicked += OnLevelTwoButtonClick;

            _level3Button = _doc.rootVisualElement.Q<Button>("Level3Button");
            _level3Button.clicked += OnLevelThreeButtonClick;

            SaveFile activeFile = FileSaver.Instance.GetActiveFile();
            CheckLocks(activeFile);
            CheckMedals(activeFile);
        }

        private void OnLevel1ButtonClick()
        {
            if (FileSaver.Instance.GetActiveFile().tutorialComplete) GameStateManager.Instance.LoadLevel(GameStateManager.Instance.levelOneSceneNum);
        }

        private void OnTutorialButtonClick()
        {
            GameStateManager.Instance.LoadLevel(GameStateManager.Instance.tutorialSceneNum);
        }

        private void OnBackButtonClick()
        {
            NewMainMenuManager.HideLevelSelect();
            NewMainMenuManager.ShowMainMenu();
        }
        private void OnLevelTwoButtonClick()
        {
            if (FileSaver.Instance.GetActiveFile().levelOneComplete) GameStateManager.Instance.LoadLevel(GameStateManager.Instance.levelTwoSceneNum);

        }

        private void OnLevelThreeButtonClick()
        {
            if (FileSaver.Instance.GetActiveFile().levelTwoComplete) GameStateManager.Instance.LoadLevel(GameStateManager.Instance.levelThreeSceneNum);

        }

        private void CheckLocks(SaveFile activeFile)
        {
            if (activeFile.tutorialComplete) _doc.rootVisualElement.Q<VisualElement>("Level1Lock").style.backgroundImage = null;
            if (activeFile.levelOneComplete) _doc.rootVisualElement.Q<VisualElement>("Level2Lock").style.backgroundImage = null;
            if (activeFile.levelTwoComplete) _doc.rootVisualElement.Q<VisualElement>("Level3Lock").style.backgroundImage = null;
        }

        private void CheckMedals(SaveFile activeFile)
        {
            _doc.rootVisualElement.Q<VisualElement>("TutorialMedal").style.backgroundImage =CheckSingleMedal(activeFile.tutorialMedal);
            _doc.rootVisualElement.Q<VisualElement>("Level1Medal").style.backgroundImage = CheckSingleMedal(activeFile.levelOneMedal);
            _doc.rootVisualElement.Q<VisualElement>("Level2Medal").style.backgroundImage = CheckSingleMedal(activeFile.levelTwoMedal);
            _doc.rootVisualElement.Q<VisualElement>("Level3Medal").style.backgroundImage = CheckSingleMedal(activeFile.levelThreeMedal);
            

        }

        private StyleBackground CheckSingleMedal(int value) => (value) switch
        {
            (int)MEDAL.NOMEDAL => null,
            (int)MEDAL.BRONZE => new StyleBackground(bronzeMedalImage),
            (int)MEDAL.SILVER => new StyleBackground(silverMedalImage),
            (int)MEDAL.GOLD => new StyleBackground(goldMedalImage),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
