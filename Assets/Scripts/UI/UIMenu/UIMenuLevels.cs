using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Game.Settings;
using Game.AppCore; 

namespace Game.UI
{
    public class UIMenuLevels : UIFullScreenPanel
    {
        [SerializeField]
        private Button backToStartMenu;

        [SerializeField]
        private UILevel levelPref;

        private const int MAX_LEVELS_IN_PAGE = 12;
        private int levelsInPage;
    
        private List<UILevel> uiLevels; 

        public void Init(LevelsSettings levels, ElementsSettings elSettings = null)
        {
            uiLevels = new List<UILevel>();
            levelsInPage = Mathf.Min(MAX_LEVELS_IN_PAGE, levels.GetLevelsCount());
            for(int i = 0; i < levelsInPage; i++)
            {
                UILevel newLevelUI = Instantiate(levelPref.gameObject).GetComponent<UILevel>();
                newLevelUI.Init(levels.GetLevelSetting(i));
                newLevelUI.transform.parent = levelPref.transform.parent;
                uiLevels.Add(newLevelUI);
                //Check Thet Open 
            }
            levelPref.gameObject.SetActive(false); 
            Init();
        }

  
        private void RefreshLevelsUI()
        {
            int allStars = CommandKeeper.Instance.GetAllStars();
            bool unlocked = true; 
            for (int i = 0; i < uiLevels.Count; i++)
            {
                int stars = CommandKeeper.Instance.GetStars(i);
                int sl = uiLevels[i].GetStarsToOpenLevel(); 

                if(sl > allStars)
                {
                    unlocked = false; 
                }

                uiLevels[i].LevelButtonSetup(i, Mathf.Max(0, stars), unlocked);
                if (stars == -1)
                {
                    unlocked = false; 
                }
            }
        }

        public override void Init()
        {
            base.Init();
            CommandKeeper.Instance.ShowLevelsMenu += Invoke;
            CommandKeeper.Instance.HideOtherMenus += HideOtherMenus;
            backToStartMenu.onClick.AddListener(BackToStartMenu);
        }

        private void Invoke()
        {
            backToStartMenu.interactable = false;
            RefreshLevelsUI();
            ShowPanel(() =>
            {
                backToStartMenu.interactable = true;
            });
            CommandKeeper.Instance.HideOtherMenus(gameObject);
        }

        private void HideOtherMenus(GameObject gObj)
        {
            if (gObj != gameObject)
            {
                HidePanel(() => { });
            }
        }
        private void BackToStartMenu()
        {
            CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.ReturnToStartScreen);
        }
        private void OnDestroy()
        {
            CommandKeeper.Instance.ShowLevelsMenu -= Invoke;
            CommandKeeper.Instance.HideOtherMenus -= HideOtherMenus;
        }

    }
}