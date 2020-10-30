using System.Collections.Generic;
using UnityEngine;
namespace Game.Settings
{
    public class LevelsSettings : ScriptableObject
    {
        [SerializeField]
        private List<LevelSettings> levels;

        public LevelSettings GetLevelSetting(int ind)
        {
            if(ind < levels.Count)
            {
                return levels[ind];
            }
            return levels[0];
        }

        public int GetLevelsCount()
        {
            return levels.Count;
        }

    }
}
