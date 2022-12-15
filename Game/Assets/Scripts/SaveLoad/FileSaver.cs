using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TeamNinja
{
    public class FileSaver
    {
        private readonly static string save_path = Application.persistentDataPath + "/savedata";
        
        public static FileSaver Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new FileSaver();
                return _instance;
            }
        }
        private static FileSaver _instance;
        private SaveFile activeFile;
        List<SaveFile> files = new();

        public void LoadFiles()
        {
            System.IO.Directory.CreateDirectory(save_path);
            files.Clear();
            foreach (string file in System.IO.Directory.GetFiles(save_path))
            {
                StreamReader reader = new(file);
                SaveFile saveFile = JsonUtility.FromJson<SaveFile>(reader.ReadToEnd());
                files.Add(saveFile);
                reader.Close();
            }

        }

        public List<SaveFile> GetLoadedFiles()
        {
            return files;
        }

        public void SelectActiveFile(SaveFile activeFile)
        {
            this.activeFile = activeFile;
        }
        public void WriteActiveFile()
        {
            System.IO.Directory.CreateDirectory(save_path);
            StreamWriter writer = new(save_path+"/"+activeFile.fileName+".json");
            string activeAsJson = JsonUtility.ToJson(activeFile);
            writer.Write(activeAsJson);
            writer.Close();
            Debug.Log("WROTE SAVE " + activeFile.fileName + " TO FILE");

        }

        public SaveFile GetActiveFile()
        {
            return activeFile;
        }

        public void SaveToActiveAndWrite(int levelNum, int medal, string timer)
        {
            Debug.Log("WRITING LEVEL DATA L:" + levelNum + " M:" + medal);
            switch (levelNum)
            {
                case 0:
                    activeFile.tutorialComplete = true;
                    activeFile.tutorialTime = timer;
                    activeFile.tutorialMedal = medal;
                    break;
                case 1:
                    activeFile.levelOneComplete = true;
                    activeFile.levelOneTime = timer;
                    activeFile.levelOneMedal = medal;

                    break;
                case 2:
                    activeFile.levelTwoComplete = true;
                    activeFile.levelTwoTime = timer;
                    activeFile.levelTwoMedal = medal;

                    break;
                case 3:
                    activeFile.levelThreeComplete = true;
                    activeFile.levelThreeTime = timer;
                    activeFile.levelThreeMedal = medal;

                    break;
            }

            activeFile.lastSaveDate = System.DateTime.UtcNow.Date.ToString("dd/MM/yyyy");
            WriteActiveFile();
        }

        public int[] GetActiveLevelMedals()
        {
            int[] medals = new[] {activeFile.tutorialMedal,activeFile.levelOneMedal, activeFile.levelTwoMedal , activeFile.levelThreeMedal };
            return medals;
        }
    }
}
