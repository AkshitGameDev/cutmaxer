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
            plane.transform.localScale = new Vector3(obsCount, 1, 1);
        }
        else if(direction == "vertical")
        {
            plane.transform.localScale = new Vector3(1, 1, obsCount);
        }

    }




}
