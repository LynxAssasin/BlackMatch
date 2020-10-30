using UnityEngine;
using System;
using Game.GameCore;
using Game.Settings;
using Game.AppCore;
using System.Collections.Generic;

namespace Game
{
    public class CommandKeeper
    {
        private static CommandKeeper instance;

        public static CommandKeeper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommandKeeper();
                }
                return instance;
            }
        }

        #region InApp
        public Action<AppMachine.Transition> MakeTransitionInApp;

        public Action<GameObject> HideOtherMenus;
        public Action ShowStartAppMenu;
        public Action ShowLevelsMenu;

        public Action<LevelSettings, int> LoadGameScene;
        public Action LoadMenu;

        public Action<int, int> SaveStars;
        public Func<int, int> GetStars;
        public Func<int> GetAllStars;
        #endregion

        #region InGame
        public Action<Action> CreateNewElements;
        public Action<GameMachine.Transition> MakeTransitionInGame;

        public Action<bool> EnablePainting;
        public Action<bool> EnableToTouchBoard;

        public Func<bool> CheckToDeleteElements;
        public Action StopPainting;
        public Action<Action> DeletePaintedElements;
        public Action<int, int> DeleteElement;
        public Action<int, ColorElementType> ScoreDeletedElement;
        public Func<bool> CheckGameFinish;

        public Func<Vector2,bool, Square> AskSquareUnderFinger;
        public Func<int,int, Square> GetSquareByIndex;

        public Action BoardFingerDown = () => { };
        public Action BoardFingerUp = () => { };

        public Action GameWinUI = () => { };
        public Action SaveLevelProgress = () => { };

        public Action GameLoseUI = () => { };

        public Action<bool> LockGameUI = (a) => { };
        public Action PauseUI = () => { };


        public Action RestartGame = () => { };


		public Action StarForLevelCollected = () => { };

        public Action<string> WriteState = (a) => { };

        //UI
        public Action<int> UpdateTimer = a => { };
        public Action<int> UpdateScore = a => { };
        public Action<int> UpdateSteps = a => { };
        public Action<int> ShowAddScore = a => { };

        public Action<List<LevelTask>> StartNewWave = a => { };
        public Action<List<LevelTask>> UpdateTask = a => { };
        #endregion
    }
}
