using Game.Settings;
using System.Collections.Generic;

namespace Game.Mode
{
    public class LimitedStepsDifferentColors : GameModeBase
    {
        private int startStepsCount;
        private int stepsCount;
        private List<LevelTask> tasks;
        private int currentWave; 

        public override void StartGameMode(LevelSettings lvSettings, ModeSettings settings)
        {
            base.StartGameMode(lvSettings, settings);
            stepsCount = lvSettings.MaxSteps;
            startStepsCount = stepsCount;
            tasks = new List<LevelTask>();
            List<LevelTask> waveTasks = new List<LevelTask>();
            foreach (var task in lvSettings.LevelTasks)
            {
                LevelTask lTask = new LevelTask() { count = task.count, element = task.element, wave = task.wave };
                tasks.Add(lTask);
                if (task.wave == 0)
                {
                    waveTasks.Add(task);
                }
            }
            CommandKeeper.Instance.StartNewWave(waveTasks);
        }

        public override void ElementDeleted(int num, ColorElementType elementType)
        {
            base.ElementDeleted(num, elementType);
            int leftCountForWave = 0;
            List<LevelTask> waveTasks = new List<LevelTask>();
            List<LevelTask> nextWaveTasks = new List<LevelTask>();
            foreach (var task in tasks)
            {
                if (task.wave == currentWave)
                {
                    if (task.element == elementType)
                    {
                        task.count = System.Math.Max(0, task.count -1);
                    }
                    leftCountForWave += task.count;
                    waveTasks.Add(task);
                }
                if (task.wave == currentWave + 1)
                {
                    nextWaveTasks.Add(task);  
                }
            }

            CommandKeeper.Instance.UpdateTask(waveTasks);
            if (leftCountForWave == 0)
            {
                currentWave++;
                if (nextWaveTasks.Count > 0)
                {
                    CommandKeeper.Instance.StartNewWave(nextWaveTasks);
                }
            }
        }

        public override bool CheckGameFinish()
        {
            stepsCount--;
            CommandKeeper.Instance.UpdateSteps(stepsCount);

            int maxWave = 0; 
            foreach (var task in tasks)
            {
                maxWave = System.Math.Max(maxWave, task.count);
            }

            if (currentWave >= maxWave)
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
