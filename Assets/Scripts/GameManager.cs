
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    SplashScreen,
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [Header("Game State")]
    [SerializeField] private GameState currentState;

    public static GameManager instance;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LevelSpawnner levelSpawnner;

    [Header("UI Screens")]
    [SerializeField] private GameObject[] splashScreen;

    [Header("Level Settings")]
    [SerializeField] private int currentLevel = 3;

    private DataManager dataManager;
    private LevelData[] levelData;

    void Awake()
    {
        // Prevent duplicate GameManagers
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        dataManager = GetComponent<DataManager>();

        if (dataManager == null)
        {
            Debug.LogError(
                "DataManager component is missing!",
                this
            );
        }

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void DebugUI()
{
    var eventSystem = UnityEngine.EventSystems.EventSystem.current;

    if (eventSystem == null)
    {
        Debug.LogError("No active EventSystem!");
        return;
    }

    Debug.Log("EventSystem: " + eventSystem.gameObject.name);
    Debug.Log("Input Module: " + eventSystem.currentInputModule);
    Debug.Log("Cursor State: " + Cursor.lockState);

    var mouse = UnityEngine.InputSystem.Mouse.current;

    if (mouse == null)
    {
        Debug.LogError("Mouse not detected!");
        return;
    }

    var pointer = new UnityEngine.EventSystems.PointerEventData(eventSystem);
    pointer.position = mouse.position.ReadValue();

    var results = new System.Collections.Generic.List<
        UnityEngine.EventSystems.RaycastResult>();

    eventSystem.RaycastAll(pointer, results);

    Debug.Log("Objects detected: " + results.Count);

    foreach (var result in results)
    {
        Debug.Log("Hit: " + result.gameObject.name);
    }
}


void Update()
{
    if (UnityEngine.InputSystem.Mouse.current != null &&
        UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
    {
        DebugUI();
    }
}
    void Start()
    {
        // DataManager.Awake() has already loaded the JSON
        if (dataManager != null)
        {
            GetData(dataManager.levelData);
        }

        // If gameplay scene was launched directly
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            currentState = GameState.Playing;
            InitializeGameplay();
        }
        else
        {
            StateController();
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (instance == this)
        {
            instance = null;
        }
    }

    public void GetData(LevelData[] data)
    {
        if (data == null || data.Length == 0)
        {
            Debug.LogError("Invalid level data received!");
            return;
        }

        levelData = data;

        Debug.Log(
            $"Received {data.Length} levels from DataManager.",
            this
        );
    }

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        if (scene.buildIndex == 2)
        {
            currentState = GameState.Playing;

            InitializeGameplay();
        }
    }

    private void InitializeGameplay()
    {
        Debug.Log("Initializing Gameplay...");

        // Find the spawner in the gameplay scene
        levelSpawnner =
            FindFirstObjectByType<LevelSpawnner>();

        if (levelSpawnner == null)
        {
            Debug.LogError(
                "LevelSpawnner not found in gameplay scene!"
            );
            return;
        }

        // Get level data directly from DataManager
        if (dataManager != null)
        {
            GetData(dataManager.levelData);
        }

        ProcessLevelData();
    }

    private void ProcessLevelData()
    {
        if (levelData == null ||
            currentLevel < 1 ||
            currentLevel > levelData.Length)
        {
            Debug.LogError(
                $"Invalid level data! Current level: {currentLevel}"
            );
            return;
        }

        string[] segments =
            levelData[currentLevel - 1].segments;

        if (segments == null || segments.Length == 0)
        {
            Debug.LogError("Level contains no segments!");
            return;
        }

        string[] direction =
            new string[segments.Length];

        int[] obstacleType =
            new int[segments.Length];

        int[] count =
            new int[segments.Length];

        for (int i = 0; i < segments.Length; i++)
        {
            string segment = segments[i];

            if (string.IsNullOrEmpty(segment) ||
                segment.Length != 5)
            {
                Debug.LogError(
                    $"Invalid segment at index {i}: {segment}"
                );
                return;
            }

            direction[i] = segment.Substring(0, 1);

            if (!int.TryParse(
                    segment.Substring(1, 2),
                    out obstacleType[i]) ||
                !int.TryParse(
                    segment.Substring(3, 2),
                    out count[i]))
            {
                Debug.LogError(
                    $"Invalid numeric data in segment: {segment}"
                );
                return;
            }

            Debug.Log(
                $"Segment {i + 1}: " +
                $"Direction = {direction[i]}, " +
                $"Obstacle Type = {obstacleType[i]}, " +
                $"Count = {count[i]}"
            );
        }

        if (levelSpawnner != null)
        {
            levelSpawnner.DataProccessing(
                direction,
                obstacleType,
                count
            );

            Debug.Log(
                $"Level {currentLevel} processed successfully!"
            );
        }
        else
        {
            Debug.LogError(
                "LevelSpawnner reference is missing!"
            );
        }
    }

    private void StateController()
    {
        switch (currentState)
        {
            case GameState.SplashScreen:
                SplashScreen();
                break;

            case GameState.MainMenu:
                MainMenu();
                break;

            case GameState.Playing:
                Debug.Log("Game is Playing");
                StartGame();
                break;

            case GameState.Paused:
                Debug.Log("Game Paused");
                break;

            case GameState.GameOver:
                Debug.Log("Game Over");
                break;
        }
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;

        Debug.Log(
            $"Game state changed to: {currentState}"
        );

        StateController();

        JuggleScreens((int)currentState);
    }

    private void JuggleScreens(int state)
    {
        for (int i = 0; i < splashScreen.Length; i++)
        {
            if (splashScreen[i] != null)
            {
                splashScreen[i].SetActive(i == state);
            }
        }
    }

    private void SplashScreen()
    {
        Debug.Log("Splash Screen");

        if (animator != null)
        {
            animator.SetBool("playSplash", true);
        }

        JuggleScreens((int)GameState.SplashScreen);
    }

    // Called by Animation Event
    public void FinishSplashScreen()
    {
        Debug.Log("Splash Screen Finished");

        if (animator != null)
        {
            animator.SetBool("playSplash", false);
        }

        ChangeState(GameState.MainMenu);

        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        Debug.Log("Main Menu");

        JuggleScreens((int)GameState.MainMenu);
    }

    public void PlayButtonPressed()
    {
        ChangeState(GameState.Playing);
    }

    public void StartGame()
    {
        Debug.Log("Game Started");

        // Gameplay initialization happens in OnSceneLoaded
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            SceneManager.LoadScene(2);
        }
    }
}
