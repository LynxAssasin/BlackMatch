using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Mode;

namespace Game.Settings
{
    [Serializable]
    public class ModeSettings
    {
        [SerializeField]
        private GameMode gameModeType;

        public bool CheckMode(GameMode gameModeType)
        {
            return gameModeType == this.gameModeType;
        }

        [SerializeField]
        private GameModeBase gameModeObject;
        public GameModeBase GameModeObject { get { return gameModeObject; } }

        [SerializeField]
        private List<GameObject> gameModeUI;
        public List<GameObject> GameModeUI { get { return gameModeUI; } }


        [SerializeField]
        private int scoreForElement; 

        [SerializeField]
        private List<ScoreCoef> scoreCoefs;  

        public int GetScoreByNum(int num)
        {
            float mult = 1; 
            for(int i = 0; i < scoreCoefs.Count; i++)
            {
                if(num < scoreCoefs[i].FromNumber)
                {
                    break; 
                }
                else
                {
                    mult = scoreCoefs[i].Multiplier;
                }
            }
            return (int)(mult * scoreForElement);
        }



        [Serializable]
        public class ScoreCoef
        {
            [SerializeField]
            private int fromNumber;
            [SerializeField]
            private float multiplier;

            public int FromNumber { get { return fromNumber; } }
            public float Multiplier { get { return multiplier; } }
        }
    }

 
}