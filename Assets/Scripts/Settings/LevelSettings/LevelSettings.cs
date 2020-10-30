using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Settings
{
    [Serializable]
    public class LevelSettings 
    {
        [SerializeField]
        private LevelBoard board;
        [SerializeField]
        private int maxElementTypes;
        [SerializeField]
        private GameMode gameMode;
        [SerializeField]
        private int maxSteps;
        [SerializeField]
        private int finishScore;
        [SerializeField]
        private int stepsForStar;
        [SerializeField]
        private int starsToOpenThisLevel;
        [SerializeField]
        private List<LevelTask> levelTask;

        public int MaxElementTypes { get { return maxElementTypes; }  }
        public LevelBoard Board { get { return board; } }   
        public GameMode GameMode { get { return gameMode; } }
        public int StepsForStar { get { return stepsForStar; } }
        public int MaxSteps { get { return maxSteps; } }
        public int FinishScore { get { return finishScore; } }
        public int StarsToOpenThisLevel { get { return starsToOpenThisLevel; } }
        public List<LevelTask> LevelTasks { get { return levelTask; } }

    }
    [System.Serializable] 
    public class LevelTask
    {
        public ColorElementType element;
        public int count;
        public int wave; 
    }
}
