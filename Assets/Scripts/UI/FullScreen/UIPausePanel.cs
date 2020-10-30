using UnityEngine;
using UnityEngine.UI;
using Game.GameCore;
using Game.Settings;
using Game.AppCore;

namespace Game.UI
{
    public class UIPausePanel : UIFullScreenPanel
    {

        [SerializeField]
        private Button play;

        [SerializeField]
        private Button toMainMenu;

        public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
        {
            base.Init(levelSettings);
            play.onClick.AddListener(Play);
            toMainMenu.onClick.AddListener(ToMainMenu);
            CommandKeeper.Instance.PauseUI += Invoke;
        }

        private void Invoke()
        {
            play.enabled = false;
            toMainMenu.enabled = false;
            ShowPanel(() =>
            {
                play.enabled = true;
                toMainMenu.enabled = true;
            });
        }
       

        private void Play()
        {
            HidePanel(() =>
            {
                CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.BackToIdle); 
            });
        }

        private void ToMainMenu()
        {
            CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.ToMainMenu);
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.PauseUI -= Invoke;
        }
    }
}