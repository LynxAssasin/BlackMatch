using UnityEngine;
using Game.Settings;

namespace Game.UI
{
    public class UILimitScore : UIScore
    {
        [SerializeField]
        private Transform progressBar;  

        private int maxScore;

        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            maxScore = levelSettings.FinishScore;
            Score.text = "0/" + maxScore.ToString();
            SetProgressBarScale(0);
        }

        protected override void UpdateScore(int score)
        {
            Score.text = score.ToString() + "/" + maxScore.ToString();
            SetProgressBarScale(Mathf.Min(1, (float)score / (float)maxScore));
        }

        private void SetProgressBarScale(float s)
        {
            Vector3 ls = progressBar.localScale;
            ls.x = s;
            progressBar.localScale = ls;
        }
    }
}