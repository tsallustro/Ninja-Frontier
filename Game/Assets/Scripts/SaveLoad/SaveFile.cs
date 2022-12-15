using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TeamNinja
{
    enum MEDAL
    {
        NOMEDAL = 0, BRONZE = 1, SILVER = 2, GOLD = 3
    }

    [System.Serializable]
    public class SaveFile
    {
        
        public string fileName = "sampleFile";
        public string createDate = "xx/xx/xx";
        public string lastSaveDate = "xx/xx/xx";
        public bool tutorialComplete = false;
        public bool levelOneComplete = false;
        public bool levelTwoComplete = false;
        public bool levelThreeComplete = false;
        public string tutorialTime = "99:59:59";
        public string levelOneTime = "99:59:59";
        public string levelTwoTime = "99:59:59";
        public string levelThreeTime = "99:59:59";
        public int tutorialMedal = 0;
        public int levelOneMedal = 0;
        public int levelTwoMedal = 0;
        public int levelThreeMedal = 0;



    }



}
