using UnityEngine;
using System; 
using DG.Tweening;
using Game.Settings; 
 
namespace Game.UI
{
    public class UIFullScreenPanel : UIBase
    {
        [SerializeField]
        private CanvasGroup allObjects;

        private const float FADE_TIME = 0.4f;


        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            allObjects.gameObject.SetActive(false);
            base.Init(levelSettings);  
        }

        public override void Init()
        {
            allObjects.gameObject.SetActive(false);
            base.Init();
        }

        protected void ShowPanel(Action afterFade)
        {
            allObjects.gameObject.SetActive(true);
            allObjects.alpha = 0;  
            allObjects.DOFade(1, FADE_TIME).OnComplete(() =>
            {
                afterFade(); 
            });
        }

        protected void HidePanel(Action callback)
        {         
            allObjects.alpha = 1;
            allObjects.DOFade(0, FADE_TIME).OnComplete(() =>
            {
                allObjects.gameObject.SetActive(false);
                callback();
            });
        }
    }
}