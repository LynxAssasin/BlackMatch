using UnityEngine;
using UnityEngine.UI;
using Game.Settings; 
namespace Game.UI
{
    public class UISteps : UIBase
    {
        [SerializeField]
        protected Text Steps;
        private const string STEPS_WORD = "Steps";

        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            CommandKeeper.Instance.UpdateSteps += UpdateSteps; 
            int startSteps = levelSettings.MaxSteps;
            Steps.text = STEPS_WORD + " " + startSteps.ToString();
        }

        private void UpdateSteps(int st)
        {
            Steps.text = STEPS_WORD + " " + st.ToString();
        }

        void OnDestroy()
        {
            CommandKeeper.Instance.UpdateSteps -= UpdateSteps;
        }
    }
}