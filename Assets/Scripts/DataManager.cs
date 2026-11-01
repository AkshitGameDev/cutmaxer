using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    [SerializeField] private TextAsset levelDataJson;
    public LevelData levelData { get; private set; }

    void Awake()
    {
        if (levelDataJson == null)
        {
            Debug.LogError("Assign LevelData.json to DataManager in the Inspector.", this);
            return;
        }

        levelData = JsonUtility.FromJson<LevelData>(levelDataJson.text);
        Debug.Log($"Loaded {levelData.levelName} with {levelData.segments.Length} segments.", this);
    }


}

[Serializable]
public class LevelData
{
    public int levelId;
    public string levelName;
    public string[] segments;
}
