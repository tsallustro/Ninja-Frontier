using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.ComponentModel;
using System.Threading;

namespace TeamNinja
{
    public class StoryLineUI : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _continueButton;
        private VisualElement _burntImageContainer;
        private Label _storyLabel;

        BackgroundWorker bw;
        [SerializeField] GameObject endGoalUI;
        [SerializeField] int textDisplayWaitTimeMilliSeconds = 250;
        [SerializeField] string storyLineText = "Please edit the story line text on the serialize field on the story line text UI object";
        [SerializeField] Texture2D burntLetter1;
        [SerializeField] Texture2D burntLetter2;
        [SerializeField] Texture2D burntLetter3;

        private void OnEnable()
        {

            _doc = GetComponent<UIDocument>();
            _continueButton = _doc.rootVisualElement.Q<Button>("ContinueButton");
            _continueButton.clicked += OnContinueButtonClick;

            _burntImageContainer = _doc.rootVisualElement.Q<VisualElement>("BurntImage");
            _storyLabel = _doc.rootVisualElement.Q<Label>("StoryLineText");

            _continueButton.Focus();

            bw = new BackgroundWorker();
            SetupBackgroundWorker();

        }

        private void SetupBackgroundWorker()
        {

            // this allows our worker to report progress during work
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;

            // what to do in the background thread
            bw.DoWork += new DoWorkEventHandler(
            delegate (object o, DoWorkEventArgs args)
            {
                BackgroundWorker b = o as BackgroundWorker;

                // do some simple processing for 10 seconds
                for (int i = 0; i <= storyLineText.Length; i++)
                {
                    if (!bw.CancellationPending)
                    {
                        // report the progress in percent
                        b.ReportProgress(i);
                        Thread.Sleep(textDisplayWaitTimeMilliSeconds);
                    }
                    else
                    {
                        _storyLabel.text = storyLineText;
                    }
                }

            });

            // what to do when progress changed (update the progress bar for example)
            bw.ProgressChanged += new ProgressChangedEventHandler(
            delegate (object o, ProgressChangedEventArgs args)
            {
                _storyLabel.text = storyLineText.Substring(0, args.ProgressPercentage);
            });


            bw.RunWorkerAsync();

        }

        private void OnContinueButtonClick()
        {
            bw.CancelAsync();
            if (_storyLabel.text.Length < storyLineText.Length)
            {
                _storyLabel.text = storyLineText;
            }
            else
            {
                endGoalUI.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        public void SetNoteBackgroundImage(int medal)
        {
            if(medal == 0)
            {
                _burntImageContainer.style.backgroundImage = burntLetter3;
            }
            else if(medal == 1)
            {
                _burntImageContainer.style.backgroundImage = burntLetter3;
            }
            else if(medal == 2)
            {
                _burntImageContainer.style.backgroundImage = burntLetter2;
            }
            else if(medal == 3)
            {
                _burntImageContainer.style.backgroundImage = burntLetter1;
            }
        }
    }
}
