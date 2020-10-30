using UnityEngine;
using Game.UI;
using Game.GameCore; 
using Game.Settings;
using UnityEngine.UI;
using Game; 

public class UIPauseButton : UIBase
{
    [SerializeField]
    private Button pauseButton;
    public override void Init(LevelSettings levelSettings, ElementsSettings elSettings = null)
    {
        base.Init(levelSettings);
        CommandKeeper.Instance.LockGameUI += LockButton;
        pauseButton.onClick.AddListener(PauseButton);
        pauseButton.interactable = false;
    }

    
    private void LockButton(bool b)
    {
        pauseButton.interactable = !b; 
    }

    private void PauseButton()
    {
        CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.PauseGame);
    }

    private void OnDestroy()
    {
        CommandKeeper.Instance.LockGameUI -= LockButton;
    }
}
