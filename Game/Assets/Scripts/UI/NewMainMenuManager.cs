using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamNinja
{


    public class NewMainMenuManager : MonoBehaviour
    {
        [SerializeField] GameObject MainMenuObj, SettingsMenuObj, LevelSelectObj, QuitCanvasObj, FileSelectObj;


        private static GameObject _MainMenu, _SettingsMenu, _LevelSelectMenu, _QuitCanvas, _FileSelect;
        private UIDocument _doc;
        private Button _newButton, _loadButton, _settingsButton, _quitButton;
        private VisualElement _newFilePopup;

        private void OnEnable()
        {
            _MainMenu = MainMenuObj;
            _SettingsMenu = SettingsMenuObj;
            _LevelSelectMenu = LevelSelectObj;
            _QuitCanvas = QuitCanvasObj;
            _FileSelect = FileSelectObj;

            _doc = GetComponent<UIDocument>();
            _loadButton = _doc.rootVisualElement.Q<Button>("LoadButton");
            _loadButton.clicked += OnLoadButtonClick;

            _newButton = _doc.rootVisualElement.Q<Button>("NewButton");
            _newButton.clicked += OnNewButtonClick;

            _settingsButton = _doc.rootVisualElement.Q<Button>("SettingsButton");
            _settingsButton.clicked += OnSettingsButtonClick;

            _quitButton = _doc.rootVisualElement.Q<Button>("QuitButton");
            _quitButton.clicked += OnQuitButtonClick;

            _newFilePopup = _doc.rootVisualElement.Q<VisualElement>("NewFileSection");
            _newFilePopup.style.display = DisplayStyle.None;

            _newFilePopup.Q<Button>("CancelButton").clicked += () =>
            {
                _newFilePopup.style.display = DisplayStyle.None;
            };

            _newFilePopup.Q<Button>("OKButton").clicked += () =>
            {
                string fileName = _newFilePopup.Q<TextField>("FileNameField").text;

                if (CheckSaveName(fileName))
                {
                    SaveFile file = new();
                    file.fileName = fileName;
                    file.createDate = DateTime.UtcNow.Date.ToString("dd/MM/yyyy");
                    file.lastSaveDate = file.createDate;
                    _newFilePopup.style.display = DisplayStyle.None;
                    FileSaver.Instance.SelectActiveFile(file);
                    FileSaver.Instance.WriteActiveFile();

                    ShowLevelSelect();
                    HideMainMenu();
                }

            };
            _loadButton.Focus();
        }

        private void OnLoadButtonClick()
        {
            HideMainMenu();
            ShowFileSelect();

        }
        private void OnNewButtonClick()
        {
            _newFilePopup.style.display = DisplayStyle.Flex;


        }
        private void OnSettingsButtonClick()
        {
            HideMainMenu();
            ShowSettingsMenu();
        }
        private void OnQuitButtonClick()
        {
            Quitter.QuitApplication();
            HideMainMenu();
            ShowQuitCanvas();

        }

        public static void HideMainMenu()
        {
            _MainMenu.SetActive(false);
        }
        public static void ShowMainMenu()
        {
            _MainMenu.SetActive(true);
        }
        public static void HideSettingsMenu()
        {
            _SettingsMenu.SetActive(false);
        }
        public static void ShowSettingsMenu()
        {
            _SettingsMenu.SetActive(true);
        }
        public static void HideLevelSelect()
        {
            _LevelSelectMenu.SetActive(false);
        }
        public static void ShowLevelSelect()
        {
            _LevelSelectMenu.SetActive(true);
        }

        public static void HideQuitCanvas()
        {
            _QuitCanvas.SetActive(false);
        }

        public static void ShowQuitCanvas()
        {
            _QuitCanvas.SetActive(true);
        }

        public static void ShowFileSelect()
        {
            _FileSelect.SetActive(true);
        }

        public static void HideFileSelect()
        {
            _FileSelect.SetActive(false);

        }

        private static bool CheckSaveName(string name)
        {
            bool isValid = true;
            if (name.Trim() != "") isValid = false;
            if (name.Contains('.')) isValid = false;

            return isValid;
        }
    }
}

