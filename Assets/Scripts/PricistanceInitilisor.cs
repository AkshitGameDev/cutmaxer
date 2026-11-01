using UnityEngine;

public class PricistanceInitilisor : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    void Start()
    {
        if (GameManager.instance == null)
        {
            DontDestroyOnLoad(gameManager);
        }   
    }
}
