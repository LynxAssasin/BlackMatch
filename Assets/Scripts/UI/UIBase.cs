using UnityEngine;
using Game.Settings;

namespace Game.UI
{
    public class UIBase : MonoBehaviour
    {
        protected LevelSettings levelSettings;

        public virtual void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            this.levelSettings = levelSettings; 
        }
        public virtual void Init()
        {

        }
    }
}