using UnityEngine;
using Game.Settings;
using Game.GameCore;
namespace Game.Mode
{
    public class GameModeBase : MonoBehaviour
    {
        protected int userScore;
        protected LevelSettings lvSettings;
        protected ModeSettings modeSettings;
        private bool gameFinished; 

        public virtual void StartGameMode(LevelSettings lvSettings, ModeSettings modeSettings)
        {
            this.lvSettings = lvSettings;
            this.modeSettings = modeSettings;
            CommandKeeper.Instance.ScoreDeletedElement += ElementDeleted;
            CommandKeeper.Instance.CheckGameFinish += CheckGameFinish;
        }

        public virtual void ElementDeleted(int num, ColorElementType elementType)
        {
            int score = modeSettings.GetScoreByNum(num);
            userScore += score;
            CommandKeeper.Instance.UpdateScore(userScore);
            CommandKeeper.Instance.ShowAddScore(score); 
        }

        public virtual bool CheckGameFinish()
        {
            return false; 
        }

        public virtual void GameWin()
        {
            CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.WinGame);
        }

        public virtual void GameLose()
        {           
            CommandKeeper.Instance.MakeTransitionInGame(GameMachine.Transition.LoseGame);
        }

        protected void OnDestroy()
        {
            CommandKeeper.Instance.ScoreDeletedElement -= ElementDeleted;
            CommandKeeper.Instance.CheckGameFinish -= CheckGameFinish;
        }
 
    }
}
