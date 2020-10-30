using System.Collections.Generic;
using UnityEngine;
using Game.Settings;
using Game.Mode;
using Game.UI;
using Game.Tools;
namespace Game.GameCore
{
    public class GameCore : MonoBehaviour
    {    

        public static LevelSettings levelSettings;
        public static int levelNumber;

        [SerializeField]
        private ElementsSettings elementsSettings;
        [SerializeField]
        private GameModesSettings gameModesSettings;

        [SerializeField]
        private GameObject back;
        [SerializeField]
        private Canvas mainCanvas;

        private Fade fade;

        private GameMode gameMode;
        private GameMachine gameMachine; 
        private Board board;

        private int unusedSteps;
        void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (levelSettings != null)
            {
                gameMode = levelSettings.GameMode;
                CreateBoard();
                CreateBoardPainting();
                CreateTouchController();
                CreateGameModeObject();

                CreateStateMachine();
                gameMachine.StartMachine();
                CommandKeeper.Instance.RestartGame += RestartGame;
                CommandKeeper.Instance.SaveLevelProgress += SaveLevelProgress;
                CommandKeeper.Instance.UpdateSteps += UpdateSteps;
                MakeFade();
            }
        }

        private void CreateStateMachine()
        {
            GameObject stateMachine = CreateSpecialGameObject("GameMachine");
            gameMachine = stateMachine.AddComponent<GameMachine>();
        }

        private void CreateBoard()
        {
            GameObject boardObject = CreateSpecialGameObject("Board");
            boardObject.transform.position = back.transform.position;
            BoardFabric bFabric = new BoardFabric();
            board = bFabric.CreateBoard(levelSettings, elementsSettings, back, boardObject);
            bFabric = null; 
        }

        private void CreateBoardPainting()
        {
            GameObject boardFinger = CreateSpecialGameObject("BoardPainting");
            boardFinger.AddComponent<BoardPainting>().Init(elementsSettings, levelSettings.Board.XSize, levelSettings.Board.YSize);
        }


        private void CreateTouchController()
        {
            GameObject touchController = CreateSpecialGameObject("TouchController");
            touchController.AddComponent<TouchController>();
        }

        private void CreateGameModeObject()
        {
            ModeSettings mSettings = gameModesSettings.GetModeSettings(gameMode);
            CreateUIObject(mSettings.GameModeUI);
            GameObject gameModeObj = (GameObject)Instantiate(mSettings.GameModeObject.gameObject);
            gameModeObj.transform.parent = gameObject.transform;
            gameModeObj.transform.name = "GameModeObject";
            gameModeObj.GetComponent<GameModeBase>().StartGameMode(levelSettings, mSettings);

        }

        private void CreateUIObject(List<GameObject> ui)
        {
            foreach (GameObject go in ui)
            {
                GameObject gameUI = (GameObject)Instantiate(go, mainCanvas.transform);
                foreach(UIBase uiBase in gameUI.GetComponentsInChildren<UIBase>())
                {
                    uiBase.Init(levelSettings, elementsSettings);
                }
            }
        }

        private GameObject CreateSpecialGameObject(string name)
        {
            GameObject obj = new GameObject();
            obj.transform.parent = gameObject.transform;
            obj.gameObject.name = name;
            return obj;
        }

        private void UpdateSteps(int st)
        {
            unusedSteps = st;
        }

        private void SaveLevelProgress()
        {
            int t = Mathf.Min(3, unusedSteps / levelSettings.StepsForStar + 1);
            CommandKeeper.Instance.SaveStars(levelNumber, t);
        }

        private void RestartGame()
        {
            foreach(Transform tr in gameObject.transform)
            {
                GameObject.Destroy(tr.gameObject);
            }
            foreach (Transform tr in mainCanvas.transform)
            {
                GameObject.Destroy(tr.gameObject);
            }
            System.GC.Collect();
            Init();
        }

        private void MakeFade()
        {
            if (fade == null)
            {
                fade = gameObject.AddComponent<Fade>();
            }
            fade.FadeOut();
        }

        private void OnDestroy()
        {
            CommandKeeper.Instance.RestartGame -= RestartGame;
            CommandKeeper.Instance.SaveLevelProgress -= SaveLevelProgress;
            CommandKeeper.Instance.UpdateSteps -= UpdateSteps;
        }

    }
}
