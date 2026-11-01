using UnityEngine;

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

    [SerializeField] private GameObject[] splashScreen;

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
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
        ChangeState(GameState.MainMenu);
        animator.SetBool("playSplash", false);

    }

}
