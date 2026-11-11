using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    SplashScreen,
    MainMenu,
    Playing,
    Paused,
    GameOver,
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameState currentState;
    public static GameManager instance;

    [SerializeField] private Animator animator;

    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LevelSpawnner levelSpawnner;

    [SerializeField] private GameObject[] splashScreen;

    private LevelData[] levelData;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }

    public void GetData(LevelData[] data)
    {
        Debug.Log($"Received {data.Length} levels from DataManager.", this);
        levelData = data;
        processLevelData();
    }


    void processLevelData()
    {
        int curLevel = 4;

        string[] segments = levelData[curLevel - 1].segments;

        // Create arrays based on number of segments
        string[] direction = new string[segments.Length];
        int[] obstacleType = new int[segments.Length];
        int[] count = new int[segments.Length];

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];

            // Extract data from 5-character string
            direction[i] = segment.Substring(0, 1);
            obstacleType[i] = int.Parse(segment.Substring(1, 2));
            count[i] = int.Parse(segment.Substring(3, 2));

            Debug.Log(
                $"Segment {i + 1}: Direction = {direction[i]}, " +
                $"Obstacle Type = {obstacleType[i]}, Count = {count[i]}"
            );
        }

        
        levelSpawnner.DataProccessing(direction, obstacleType, count);
    }

    

    void Start()
    {
        currentState = GameState.SplashScreen;
        stateController();
        animator = this.GetComponent<Animator>();
    }
    void stateController()
    {
        switch (currentState)
        {
            case GameState.SplashScreen:
                SplashScreen();
                break;

            case GameState.MainMenu:
                    SplashScreen();
                break;
            case GameState.Playing:
                // Handle gameplay logic
                break;
            case GameState.Paused:
                // Handle paused logic
                break;
            case GameState.GameOver:
                // Handle game over logic
                break;
            default:
                break;
        }
    }
    void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"Game state changed to: {currentState}");
        juggleScreens((int)currentState);

    }

    private void juggleScreens(int state)
    {
        for (int i = 0; i < splashScreen.Length; i++)
        {
            if (i == state)
            {
                splashScreen[i].SetActive(true);
            }
            else
            {
                splashScreen[i].SetActive(false);
            }
        }
    }

    void SplashScreen()
    {
        animator.SetBool("playSplash", true);
    }


    public void FinishSplashScreen()
    {
        animator.SetBool("playSplash", false);
        ChangeState(GameState.MainMenu);
        SceneManager.LoadScene(1);
        MainMenu();
    }

    public void MainMenu()
    {
        
    }

    public void StartGame()
    {
        ChangeState(GameState.Playing);
    }

}
