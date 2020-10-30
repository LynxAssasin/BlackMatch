using UnityEngine.UI;
using UnityEngine;
using Game.AppCore;

namespace Game.UI
{
    public class UIMenuStartApp : UIFullScreenPanel
    {
        [SerializeField]
        private Button goToLevelsMenu;

        public override void Init()
        {
            base.Init(levelSettings);
            CommandKeeper.Instance.ShowStartAppMenu += Invoke;
            CommandKeeper.Instance.HideOtherMenus += HideOtherMenus;
            goToLevelsMenu.onClick.AddListener(GoToLevelMenu);

        }

        private void Invoke()
        {
            goToLevelsMenu.interactable = false;
            ShowPanel(() =>
            {
                goToLevelsMenu.interactable = true;
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

        private void GoToLevelMenu()
        {
            CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.ToMainMenu);
        }
        private void OnDestroy()
        {
            CommandKeeper.Instance.ShowStartAppMenu -= Invoke;
            CommandKeeper.Instance.HideOtherMenus -= HideOtherMenus;
        }

    }
}