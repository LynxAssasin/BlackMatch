using Game.Settings;  
namespace Game.Mode
{
    public class LimitedStepsMode : GameModeBase
    {
        private int startStepsCount;
        private int stepsCount;  

        public override void StartGameMode(LevelSettings lvSettings, ModeSettings settings)
        {
            base.StartGameMode(lvSettings, settings);
            stepsCount = lvSettings.MaxSteps;
            startStepsCount = stepsCount;
        }

        public override void ElementDeleted(int num, ColorElementType elementType)
        {
            base.ElementDeleted(num, elementType);          
        }

        public override bool CheckGameFinish()
        {
            stepsCount--;
            CommandKeeper.Instance.UpdateSteps(stepsCount);

            if (userScore >= lvSettings.FinishScore)
            {
                GameWin();
                return true;
            }

            if (stepsCount <= 0)
            {
                GameLose();
                return true;
            }
            return false;
        }

        public override void GameWin()
        {
            base.GameWin();
        }

        public override void GameLose()
        {
            base.GameLose();
        }

    }
}
