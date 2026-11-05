
using UnityEngine;

public class SegmentSpawnner : MonoBehaviour
{
    [Header("Spawn References")]
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject plane;
    [SerializeField] private Transform ObstacleSpawnparent;

    [Header("Segment Settings")]
    public string direction = "horizontal";
    public int obsCount = 3;

    [Header("Spacing Settings")]
    [SerializeField] private float spacing = 10f;

    void Start()
    {
        if (direction == "horizontal")
        {
            plane.transform.localScale = new Vector3(obsCount, 1, 1);

            SpawnObstacles(Vector3.right);
        }
        else if (direction == "vertical")
        {
            ObstacleSpawnparent.localRotation = Quaternion.Euler(-90, 0, 0);
            plane.transform.localScale = new Vector3(1, 1, obsCount);

            SpawnObstacles(Vector3.forward);
        }
    }

    void SpawnObstacles(Vector3 spawnDirection)
    {
        float initialSpawnPoint =
            (obsCount * spacing / 2f) - (spacing / 2f);

        for (int i = 0; i < obsCount; i++)
        {
            float offset = initialSpawnPoint - (i * spacing);

            Vector3 spawnPos = spawnDirection * offset;

            SpawnObstacle(spawnPos);
        }
    }

    void SpawnObstacle(Vector3 localPosition)
    {
        GameObject obs = Instantiate(
            obstacle,
            ObstacleSpawnparent
        );

        if(direction == "horizontal")
        {
            obs.transform.localPosition = new Vector3(localPosition.x, -2.5f, -2.5f);
        }
        else if(direction == "vertical")
        {
            obs.transform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z);
        }
        obs.transform.localRotation = Quaternion.identity;
    }
}
