using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Game.Tools;
using Game.Settings;
namespace Game.UI
{
    public class UIScore : UIBase
    {
        [SerializeField]
        protected Text Score;

        [SerializeField]
        protected Text ShowAddScoreText;
        [SerializeField]
        protected List<Color> colors;
        protected float scale;

        protected const float SCALE_TIME = 0.1f;
        protected const float STOP_SHOW_ADD_SCORE = 0.3f;
        protected int colorNumber = -1;
        protected Timer stopShowScoreTimer;

        protected float previousScore = -1;


        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            stopShowScoreTimer = gameObject.AddComponent<Timer>();
            CommandKeeper.Instance.UpdateScore += UpdateScore;
            CommandKeeper.Instance.ShowAddScore += ShowAddScore;
            scale = ShowAddScoreText.transform.localScale.x;
            ShowAddScoreText.transform.localScale = Vector3.zero;  
        }

        
        protected virtual void UpdateScore(int score)
        {
            Score.text = score.ToString();
        }

        protected void ShowAddScore(int score)
        {
            ShowAddScoreText.text = "+" + score.ToString();
            ShowAddScoreText.transform.DOScale(1, SCALE_TIME);
            stopShowScoreTimer.StopTimer();
            stopShowScoreTimer.StartTimer(STOP_SHOW_ADD_SCORE, StopScaling);
            if(previousScore < score)
            {
                colorNumber++; 
            }
            ShowAddScoreText.color = colors[colorNumber];
            previousScore = score; 
        }

        private void StopScaling()
        {
            previousScore = -1;
            colorNumber = -1;

            ShowAddScoreText.transform.localScale = Vector3.zero;
        }

        protected void OnDestroy()
        {
            CommandKeeper.Instance.UpdateScore -= UpdateScore;
            CommandKeeper.Instance.ShowAddScore -= ShowAddScore;
        }
    }
}