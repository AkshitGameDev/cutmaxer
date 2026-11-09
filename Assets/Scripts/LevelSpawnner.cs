using UnityEngine;

public class LevelSpawnner : MonoBehaviour
{

    [Header("Spawn References")]
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject segmentPrefab;
    Vector3 nextSpawnPoint = new Vector3(0, 0, 0);
    float spacing = 10f;

    string[] tDirPS = new string[10];
    int[] obsIndexPS = new int[10];
    int[] obsCountPS = new int[10];
    

//PS means per segment, so tDirPS means segment direction per segment, obsIndexPS means obstacle index per segment, obsCountPS means obstacle count per segment
    public void DataProccessing(string[] _dirPS, int[] _obsIndexPS, int[] _obsCountPS)
    {
        tDirPS = _dirPS;
        obsIndexPS = _obsIndexPS;
        obsCountPS = _obsCountPS;  

        for (int i = 0; i < tDirPS.Length; i++)
        {
            string d = "horizontal";
            float a = 0f;
            if(tDirPS[i] == "V")
            {
                d = "vertical";
                a = 0f;
            }
            else
            {
                d = "horizontal";
                a = 0f;
                if(tDirPS[i] == "A")
                {
                    a = 45f;

                }
                else if(tDirPS[i] == "B")
                {
                    a = -45f;
                }

            }
            Debug.Log("Angle: " + a + " dir: " + d);
            SpawnSegment(d, obsIndexPS[i], obsCountPS[i], a);
        }  
    }

    void SpawnSegment(string dir,int obsIndex, int obsCount, float angle)
    {
        object segment = Instantiate(segmentPrefab, nextSpawnPoint, Quaternion.identity);
        SegmentSpawnner segmentSpawnner = ((GameObject)segment).GetComponent<SegmentSpawnner>();
        segmentSpawnner.getsetData(dir, obsCount, GetObstacle(obsIndex), angle);
        spacing += spacing + obsCount/2;
        nextSpawnPoint = new Vector3(nextSpawnPoint.x + spacing, obsCount/2 + 4, nextSpawnPoint.z );

    }

    GameObject GetObstacle(int index)
    {
        switch (index)
        {
            case 01:
                return obstacles[0];
            case 2:
                return obstacles[1];
            case 3:
                return obstacles[2];
            case 4:
                return obstacles[3];
            default:
                return null;
        }
    }
    


}
