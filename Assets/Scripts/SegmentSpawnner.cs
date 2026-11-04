using Unity.VisualScripting;
using UnityEngine;

public class SegmentSpawnner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoint;
    [SerializeField] private GameObject obstacle;
    
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject ObstacleSpawnparent;

    public string direction = "horizontal";
    public float obsCount = 3;




    void Start()
    {
       if(direction == "horizontal")
        {
            
        }
        else if(direction == "vertical")
        {
            
        }

    }




}
