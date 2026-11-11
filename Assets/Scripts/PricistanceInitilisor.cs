using UnityEngine;

public class PricistanceInitilisor : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    void Start()
    {
        if (GameManager.instance == null)
        {
            var _gameManager = Instantiate(gameManager);
            DontDestroyOnLoad(_gameManager);
        }   
    }
}
