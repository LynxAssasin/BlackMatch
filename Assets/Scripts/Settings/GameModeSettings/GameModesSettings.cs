using System.Collections.Generic;
using UnityEngine;

namespace Game.Settings
{
    public class GameModesSettings : ScriptableObject
    {
        [SerializeField]
        List<ModeSettings> modeSettings;

        public ModeSettings GetModeSettings(GameMode mode)
        {
            foreach(ModeSettings ms in modeSettings)
            {
                if (ms.CheckMode(mode))
                {
                    return ms; 
                }
            }
            return null;
        }
    }
}