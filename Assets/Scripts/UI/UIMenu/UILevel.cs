using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Game.Settings;

namespace Game.UI
{
    public class UILevel : UIBase
    {
        [SerializeField]
        private Text levelNumber; 

        [SerializeField]
        private List<Image> stars;

        [SerializeField]
        private Image locker;

        [SerializeField]
        private Text conditionText;

        [SerializeField]
        private Image conditionStar;

        [SerializeField]
        private Button activeButton;

        private Color starsColor = Color.yellow;
        private int level; 
        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            activeButton.onClick.AddListener(StartLevel);

        }

        public int GetStarsToOpenLevel()
        {
            return levelSettings.StarsToOpenThisLevel;
        }

        public void LevelButtonSetup(int level, int starCount,  bool open)
        {
            levelNumber.text = level.ToString();
            this.level = level;
            activeButton.interactable = open;
            if (open)
            {
                conditionText.gameObject.SetActive(false);
                conditionStar.gameObject.SetActive(false);
                locker.gameObject.SetActive(false);

                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].gameObject.SetActive(true);
                }

                for (int i = 0; i < starCount; i++)
                {
                    stars[i].color = starsColor;
                }
            }
            else
            {
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].gameObject.SetActive(false); 
                }

                int condition = levelSettings.StarsToOpenThisLevel;  
                if(condition!= 0)
                {
                    conditionText.text = condition.ToString();
                }
                else
                {
                    conditionText.gameObject.SetActive(false);
                    conditionStar.gameObject.SetActive(false);
                }
            }
        }
        private void StartLevel()
        {
            CommandKeeper.Instance.LoadGameScene(levelSettings, level);
        }

    }
}