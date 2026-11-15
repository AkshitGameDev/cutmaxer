
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset levelDataJson;

    public LevelData[] levelData { get; private set; }

    void Awake()
    {
        if (levelDataJson == null)
        {
            Debug.LogError("Assign LevelData.json to DataManager.", this);
            return;
        }

        LevelDataCollection collection =
            JsonUtility.FromJson<LevelDataCollection>(
                levelDataJson.text
            );

        if (collection == null ||
            collection.levels == null ||
            collection.levels.Length == 0)
        {
            Debug.LogError("Failed to load level data.", this);
            return;
        }

        levelData = collection.levels;

        Debug.Log($"Loaded {levelData.Length} levels.", this);
    }
}

[Serializable]
public class LevelDataCollection
{
    public LevelData[] levels;
}

[Serializable]
public class LevelData
{
    public int levelId;
    public string levelName;
    public string[] segments;
}
