using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace TeamNinja
{
    public class FileSelectController : MonoBehaviour
    {
        public VisualTreeAsset fileSection;
        private UIDocument _doc;

        public void OnEnable()
        {
            _doc = GetComponent<UIDocument>();
            FileSaver.Instance.LoadFiles();
            List<SaveFile> files = FileSaver.Instance.GetLoadedFiles();
            foreach (SaveFile file in files)
            {
                VisualElement section = fileSection.Instantiate();
                section.AddToClassList(".file-section");
                section.Q<Button>("SelectButton").clicked += () =>
                {
                    FileSaver.Instance.SelectActiveFile(file);
                    NewMainMenuManager.HideFileSelect();
                    NewMainMenuManager.ShowLevelSelect();
                };
                
                section.Q<TextElement>("FileName").text = file.fileName;
                section.Q<VisualElement>("DatesSection").Q<TextElement>("CreateDate").text = file.createDate;
                section.Q<VisualElement>("DatesSection").Q<TextElement>("LastSave").text = file.lastSaveDate;

                _doc.rootVisualElement.Q<ScrollView>("FileScrollArea").Add(section);
            }

        }
    }
}
