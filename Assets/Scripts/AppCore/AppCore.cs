using UnityEngine;
using Game.AppCore;
using Game.Tools;
using Game.Settings;
using Game;
using Game.UI; 
using UnityEngine.SceneManagement;
using Game.GameCore;
using Game.Data; 
public class AppCore : MonoBehaviour
{
    private static bool exist;

    [SerializeField]
    private Scene mainMenuScene;
    [SerializeField]
    private Scene gameScene;
    [SerializeField]
    private LevelsSettings levelsSettings;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private UIMenuLevels levelsMenuPrefab;
    [SerializeField]
    private UIMenuStartApp startAppMenuPrefab;


    private Scene currentScene;
    private AppMachine appMachine;
    private Fade fade;
    private DataController data; 

    void Awake()
    {
        if (!exist)
        {
            Init();
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        exist = true; 
    }

    private void Init()
    {
        currentScene = mainMenuScene;
        DontDestroyOnLoad(gameObject);
        CreateStateMachine();
        CreateFade();
        fade.FadeOut();
        gameObject.AddComponent<DataController>().Init(); 
        CreateUI();
        CommandKeeper.Instance.LoadGameScene += LoadGameScene;
        CommandKeeper.Instance.LoadMenu += LoadMenu;
        appMachine.StartMachine();
    }

 
    private void CreateUI()
    {
        UIMenuLevels levelsMenu = Instantiate(levelsMenuPrefab, mainCanvas.transform);
        levelsMenu.Init(levelsSettings);

        UIMenuStartApp startAppMenu = Instantiate(startAppMenuPrefab, mainCanvas.transform);
        startAppMenu.Init();
    }

    private void CreateStateMachine()
    {
        GameObject stateMachine = CreateSpecialGameObject("AppMachine");
        appMachine = stateMachine.AddComponent<AppMachine>();
    }

    private GameObject CreateSpecialGameObject(string name)
    {
        GameObject obj = new GameObject();
        obj.transform.parent = gameObject.transform;
        obj.gameObject.name = name;
        return obj;
    }


    private void CreateFade()
    {
        if (fade == null)
        {
            GameObject obj = new GameObject("Fade");
            fade = obj.AddComponent<Fade>();
        }
    }

    private void OnDestroy()
    {
        CommandKeeper.Instance.LoadGameScene -= LoadGameScene;
        CommandKeeper.Instance.LoadMenu -= LoadMenu;
    }

    private void LoadGameScene(LevelSettings setting, int level)
    {
        mainCanvas.gameObject.SetActive(false);
        GameCore.levelSettings = setting;
        GameCore.levelNumber = level;
        CommandKeeper.Instance.MakeTransitionInApp(AppMachine.Transition.StartPlayingGame);
        LoadScene(gameScene); 
    }

    private void LoadMenu()
    {
        if (currentScene != mainMenuScene)
        {
            SceneManager.sceneLoaded += LoadMainMenuFromScene;
            LoadScene(mainMenuScene);
        }
        else
        {
            CommandKeeper.Instance.ShowLevelsMenu();
        }
    }
    private void LoadMainMenuFromScene(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        mainCanvas.gameObject.SetActive(true);
        CommandKeeper.Instance.ShowLevelsMenu();
        SceneManager.sceneLoaded -= LoadMainMenuFromScene;
    }

    private void LoadScene(Scene scene)
    {
        currentScene = scene;
        SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Single);
    }

}
