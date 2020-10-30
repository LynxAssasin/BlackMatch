using UnityEngine;
using UnityEngine.UI;
using Game.AppCore;
using Game.Settings;
namespace Game.UI
{
    public class UILose : UIFullScreenPanel
    {

        [SerializeField]
        private Button restart;

        [SerializeField]
        private Button toMainMenu;

        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            restart.onClick.AddListener(Restart);
            toMainMenu.onClick.AddListener(ToMainMenu);
            CommandKeeper.Instance.GameLoseUI += Invoke;
        }

        private void Invoke()
        {
            restart.enabled = false;
            toMainMenu.enabled = false;
            ShowPanel(() =>
            {
                restart.enabled = true;
                toMainMenu.enabled = true;
            });
        }
       

        private void Restart()
        {
            CommandKeeper.Instance.RestartGame(); 
        }

        private void ToMainMenu()
        {
            CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.ToMainMenu);
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.GameLoseUI -= Invoke;
        }
    }
}