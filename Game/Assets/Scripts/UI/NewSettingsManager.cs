using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
namespace TeamNinja
{
    public class NewSettingsManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        private UIDocument _doc;
        private Button _returnButton;
        private Toggle _fullscreenToggle;
        private Slider _volumeSlider;
        private Slider _senSlider;
        private void OnEnable()
        {
            Debug.Log("SETTINGS: ENABLED");
            _doc = GetComponent<UIDocument>();
            _returnButton = _doc.rootVisualElement.Q<Button>("ReturnButton");
            _returnButton.clicked += OnReturnButtonClick;

            _fullscreenToggle = _doc.rootVisualElement.Q<Toggle>("FullscreenToggle");
            _fullscreenToggle.RegisterValueChangedCallback(evnt => { Debug.Log("SETTINGS: FULLSCREEN TOGGLED"); Screen.fullScreen = evnt.newValue; });

            _volumeSlider = _doc.rootVisualElement.Q<Slider>("VolumeSlider");
            _volumeSlider.RegisterValueChangedCallback(evnt =>
            {

                Debug.Log("SETTINGS: VOLUME CHANGED "); audioMixer.SetFloat("Volume", evnt.newValue);
            });
            _senSlider = _doc.rootVisualElement.Q<Slider>("SensitivitySlider");
            _senSlider.RegisterValueChangedCallback(evnt =>
            {

                Debug.Log("SETTINGS: SEN CHANGED "); GameStateManager.Instance.sensitivity = evnt.newValue;
            });

            audioMixer.GetFloat("Volume", out float volume);
            _senSlider.value = GameStateManager.Instance.sensitivity;
            _volumeSlider.value = volume;
            _fullscreenToggle.value = Screen.fullScreen;
            _returnButton.Focus();


        }
        protected virtual void OnReturnButtonClick()
        {
            Debug.Log("SETTINGS MENU: RETURN CLICKED");
            NewMainMenuManager.HideSettingsMenu();
            NewMainMenuManager.ShowMainMenu();
        }

    }
}
