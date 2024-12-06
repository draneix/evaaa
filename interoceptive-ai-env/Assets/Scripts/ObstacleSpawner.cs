using UnityEngine;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class AreaRange
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

[System.Serializable]
public class RotationRange
{
    public float x, y, z;
}

[System.Serializable]
public class ScaleRange
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

[System.Serializable]
public class ObstacleGroup
{
    public string prefabName;         // Name of the prefab
    public int count;                 // Number of obstacles
    public AreaRange area;            // Spawn area
    public RotationRange rotationRange; // Rotation range
    public ScaleRange scaleRange;     // Scale range
}

[System.Serializable]
public class ObstacleConfig
{
    public List<ObstacleGroup> groups; // List of obstacle groups
}

public class ObstacleSpawner : MonoBehaviour
{
    public string configFileName = "obstacleConfig.json"; // Config file name
    public string prefabFolder = "Obstacles";            // Folder under Resources

    private ObstacleConfig obstacleConfig; // Loaded obstacle configuration

    void Start()
    {
        LoadConfig();
        GenerateObstacles();
    }

    void LoadConfig()
    {
        string configFolderPath;

        #if UNITY_EDITOR
        // In the Unity Editor, go one level up to the project root
        configFolderPath = Path.Combine(Application.dataPath, "../Config");
        #else
        // For standalone builds, use the current working directory for relative paths
        configFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Config");
        #endif
        string configFilePath = Path.Combine(configFolderPath, configFileName);

        Debug.Log($"Resolved Config Folder Path: {configFolderPath}");
        Debug.Log($"Resolved Config File Path: {configFilePath}");


        if (!File.Exists(configFilePath))
        {
            Debug.LogError($"Config file not found at: {configFilePath}");
            return;
        }

        string jsonContent = File.ReadAllText(configFilePath);
        obstacleConfig = JsonUtility.FromJson<ObstacleConfig>(jsonContent);

        if (obstacleConfig != null && obstacleConfig.groups != null)
        {
            Debug.Log($"Loaded Obstacle Config from: {configFilePath}");
        }
        else
        {
            Debug.LogError("Obstacle configuration is empty or invalid.");
        }
    }

    public void ReloadConfig()
    {
        LoadConfig();
        GenerateObstacles();
        Debug.Log("Obstacle configuration reloaded.");
    }

    void GenerateObstacles()
    {
        if (obstacleConfig == null)
        {
            Debug.LogError("Obstacle configuration is not loaded. Aborting obstacle generation.");
            return;
        }

        foreach (ObstacleGroup group in obstacleConfig.groups)
        {
            SpawnObstacleGroup(group);
        }
    }

    void SpawnObstacleGroup(ObstacleGroup group)
    {
        GameObject prefab = Resources.Load<GameObject>($"{prefabFolder}/{group.prefabName}");
        if (prefab == null)
        {
            Debug.LogError($"Prefab {group.prefabName} not found in Resources/{prefabFolder}");
            return;
        }

        for (int i = 0; i < group.count; i++)
        {
            float x = Random.Range(group.area.xMin, group.area.xMax);
            float y = Random.Range(group.area.yMin, group.area.yMax);
            float z = Random.Range(group.area.zMin, group.area.zMax);
            Vector3 position = new Vector3(x, y, z);

            Quaternion rotation = Quaternion.Euler(
                group.rotationRange.x,
                Random.Range(0, group.rotationRange.y),
                group.rotationRange.z
            );

            Vector3 scale = new Vector3(
                Random.Range(group.scaleRange.xMin, group.scaleRange.xMax),
                Random.Range(group.scaleRange.yMin, group.scaleRange.yMax),
                Random.Range(group.scaleRange.zMin, group.scaleRange.zMax)
            );

            GameObject obstacle = Instantiate(prefab, position, rotation);
            obstacle.transform.localScale = scale;

            Debug.Log($"Spawned {group.prefabName} at {position} with scale: {scale}");
        }
    }
}
