using UnityEngine;
using System.Collections;
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
    public string prefabName;
    public int count;
    public AreaRange area;
    public RotationRange rotationRange;
    public ScaleRange scaleRange;
}

[System.Serializable]
public class ObstacleConfig
{
    public List<ObstacleGroup> groups;
}

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public string configFileName = "obstacleConfig.json";
    public string prefabFolder = "Obstacles";

    private ObstacleConfig obstacleConfig; // Holds parsed obstacle configuration
    private List<GameObject> spawnedObstacles = new List<GameObject>(); // Tracks generated obstacles
    private Transform courtTransform; // Reference to dynamically generated court

    void Start()
    {
        LoadConfig();
        StartCoroutine(WaitForCourtAndGenerateObstacles());
    }

    private IEnumerator WaitForCourtAndGenerateObstacles()
    {
        // Wait for the CourtSpawner to generate the court
        CourtSpawner courtSpawner = FindObjectOfType<CourtSpawner>();
        while (courtSpawner == null || courtSpawner.CourtTransform == null)
        {
            yield return null; // Wait for the next frame
        }

        courtTransform = courtSpawner.CourtTransform;
        GenerateObstacles();
    }

    public void ResetObstacles()
    {
        ClearObstacles();
        GenerateObstacles();
    }

    private void LoadConfig()
    {
        string configFolderPath = Application.isEditor
            ? Path.Combine(Application.dataPath, "../Config")
            : Path.Combine(Directory.GetCurrentDirectory(), "Config");

        string configFilePath = Path.Combine(configFolderPath, configFileName);

        if (!File.Exists(configFilePath))
        {
            Debug.LogError($"Config file not found: {configFilePath}");
            return;
        }

        string jsonContent = File.ReadAllText(configFilePath);
        obstacleConfig = JsonUtility.FromJson<ObstacleConfig>(jsonContent);

        if (obstacleConfig == null || obstacleConfig.groups == null)
        {
            Debug.LogError("Invalid or empty obstacle configuration.");
        }
    }

    private void GenerateObstacles()
    {
        if (obstacleConfig == null || obstacleConfig.groups == null)
        {
            Debug.LogError("Obstacle configuration is not loaded.");
            return;
        }

        foreach (var group in obstacleConfig.groups)
        {
            SpawnObstacleGroup(group);
        }
    }

    private void ClearObstacles()
    {
        foreach (var obstacle in spawnedObstacles)
        {
            if (obstacle != null) Destroy(obstacle);
        }
        spawnedObstacles.Clear();
    }

    private void SpawnObstacleGroup(ObstacleGroup group)
    {
        GameObject prefab = Resources.Load<GameObject>($"{prefabFolder}/{group.prefabName}");
        if (prefab == null)
        {
            Debug.LogError($"Prefab not found: {group.prefabName}");
            return;
        }

        for (int i = 0; i < group.count; i++)
        {
            Vector3 position = RandomPosition(group.area);
            Quaternion rotation = RandomRotation(group.rotationRange);
            Vector3 scale = RandomScale(group.scaleRange);

            GameObject obstacle = Instantiate(prefab, position, rotation);

            if (courtTransform != null)
            {
                obstacle.transform.SetParent(courtTransform);
            }

            obstacle.transform.localScale = scale;
            spawnedObstacles.Add(obstacle);
        }
    }

    private Vector3 RandomPosition(AreaRange area) =>
        new Vector3(
            Random.Range(area.xMin, area.xMax),
            Random.Range(area.yMin, area.yMax),
            Random.Range(area.zMin, area.zMax)
        );

    private Quaternion RandomRotation(RotationRange rotationRange) =>
        Quaternion.Euler(
            rotationRange.x,
            Random.Range(0, rotationRange.y),
            rotationRange.z
        );

    private Vector3 RandomScale(ScaleRange scaleRange) =>
        new Vector3(
            Random.Range(scaleRange.xMin, scaleRange.xMax),
            Random.Range(scaleRange.yMin, scaleRange.yMax),
            Random.Range(scaleRange.zMin, scaleRange.zMax)
        );
}

// using UnityEngine;
// using System.Collections.Generic;
// using System.IO;

// [System.Serializable]
// public class AreaRange
// {
//     public float xMin, xMax, yMin, yMax, zMin, zMax;
// }

// [System.Serializable]
// public class RotationRange
// {
//     public float x, y, z;
// }

// [System.Serializable]
// public class ScaleRange
// {
//     public float xMin, xMax, yMin, yMax, zMin, zMax;
// }

// [System.Serializable]
// public class ObstacleGroup
// {
//     public string prefabName;         // Name of the prefab
//     public int count;                 // Number of obstacles
//     public AreaRange area;            // Spawn area
//     public RotationRange rotationRange; // Rotation range
//     public ScaleRange scaleRange;     // Scale range
// }

// [System.Serializable]
// public class ObstacleConfig
// {
//     public List<ObstacleGroup> groups; // List of obstacle groups
// }

// public class ObstacleSpawner : MonoBehaviour
// {
//     public string configFileName = "obstacleConfig.json"; // Config file name
//     public string prefabFolder = "Obstacles";            // Folder under Resources

//     private ObstacleConfig obstacleConfig; // Loaded obstacle configuration
//     private List<GameObject> spawnedObstacles = new List<GameObject>(); // Track spawned obstacles

//     void Start()
//     {
//         LoadConfig();
//         GenerateObstacles();
//     }

//     void LoadConfig()
//     {
//         string configFolderPath;

//         #if UNITY_EDITOR
//         // In Unity Editor, use the project-relative path
//         configFolderPath = Path.Combine(Application.dataPath, "../Config");
//         #else
//         // For standalone builds, use the current working directory for relative paths
//         configFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Config");
//         #endif

//         string configFilePath = Path.Combine(configFolderPath, configFileName);

//         // Debug.Log($"Resolved Config Folder Path: {configFolderPath}");
//         // Debug.Log($"Resolved Config File Path: {configFilePath}");

//         if (!File.Exists(configFilePath))
//         {
//             Debug.LogError($"Config file not found at: {configFilePath}");
//             return;
//         }

//         string jsonContent = File.ReadAllText(configFilePath);
//         obstacleConfig = JsonUtility.FromJson<ObstacleConfig>(jsonContent);

//         if (obstacleConfig != null && obstacleConfig.groups != null)
//         {
//             Debug.Log($"Loaded Obstacle Config from: {configFilePath}");
//         }
//         else
//         {
//             Debug.LogError("Obstacle configuration is empty or invalid.");
//         }
//     }

//     public void ResetObstacles()
//     {
//         // Clear existing obstacles
//         foreach (GameObject obstacle in spawnedObstacles)
//         {
//             if (obstacle != null)
//             {
//                 Destroy(obstacle);
//             }
//         }
//         spawnedObstacles.Clear();

//         // Regenerate obstacles
//         GenerateObstacles();
//         Debug.Log("Obstacles reset and regenerated.");
//     }

//     void GenerateObstacles()
//     {
//         if (obstacleConfig == null)
//         {
//             Debug.LogError("Obstacle configuration is not loaded. Aborting obstacle generation.");
//             return;
//         }

//         foreach (ObstacleGroup group in obstacleConfig.groups)
//         {
//             SpawnObstacleGroup(group);
//         }
//     }

//     void SpawnObstacleGroup(ObstacleGroup group)
//     {
//         GameObject prefab = Resources.Load<GameObject>($"{prefabFolder}/{group.prefabName}");
//         if (prefab == null)
//         {
//             Debug.LogError($"Prefab {group.prefabName} not found in Resources/{prefabFolder}");
//             return;
//         }

//         for (int i = 0; i < group.count; i++)
//         {
//             float x = Random.Range(group.area.xMin, group.area.xMax);
//             float y = Random.Range(group.area.yMin, group.area.yMax);
//             float z = Random.Range(group.area.zMin, group.area.zMax);
//             Vector3 position = new Vector3(x, y, z);

//             Quaternion rotation = Quaternion.Euler(
//                 group.rotationRange.x,
//                 Random.Range(0, group.rotationRange.y),
//                 group.rotationRange.z
//             );

//             Vector3 scale = new Vector3(
//                 Random.Range(group.scaleRange.xMin, group.scaleRange.xMax),
//                 Random.Range(group.scaleRange.yMin, group.scaleRange.yMax),
//                 Random.Range(group.scaleRange.zMin, group.scaleRange.zMax)
//             );

//             GameObject obstacle = Instantiate(prefab, position, rotation);
//             obstacle.transform.localScale = scale;
//             spawnedObstacles.Add(obstacle);

//             Debug.Log($"Spawned {group.prefabName} at {position} with scale: {scale}");
//         }
//     }
// }

// // using UnityEngine;
// // using System.Collections.Generic;
// // using System.IO;

// // [System.Serializable]
// // public class AreaRange
// // {
// //     public float xMin, xMax, yMin, yMax, zMin, zMax;
// // }

// // [System.Serializable]
// // public class RotationRange
// // {
// //     public float x, y, z;
// // }

// // [System.Serializable]
// // public class ScaleRange
// // {
// //     public float xMin, xMax, yMin, yMax, zMin, zMax;
// // }

// // [System.Serializable]
// // public class ObstacleGroup
// // {
// //     public string prefabName;         // Name of the prefab
// //     public int count;                 // Number of obstacles
// //     public AreaRange area;            // Spawn area
// //     public RotationRange rotationRange; // Rotation range
// //     public ScaleRange scaleRange;     // Scale range
// // }

// // [System.Serializable]
// // public class ObstacleConfig
// // {
// //     public List<ObstacleGroup> groups; // List of obstacle groups
// // }

// // public class ObstacleSpawner : MonoBehaviour
// // {
// //     public string configFileName = "obstacleConfig.json"; // Config file name
// //     public string prefabFolder = "Obstacles";            // Folder under Resources

// //     private ObstacleConfig obstacleConfig; // Loaded obstacle configuration

// //     void Start()
// //     {
// //         LoadConfig();
// //         GenerateObstacles();
// //     }

// //     void LoadConfig()
// //     {
// //         string configFolderPath;

// //         #if UNITY_EDITOR
// //         // In the Unity Editor, go one level up to the project root
// //         configFolderPath = Path.Combine(Application.dataPath, "../Config");
// //         #else
// //         // For standalone builds, use the current working directory for relative paths
// //         configFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Config");
// //         #endif
// //         string configFilePath = Path.Combine(configFolderPath, configFileName);

// //         if (!File.Exists(configFilePath))
// //         {
// //             Debug.LogError($"Config file not found at: {configFilePath}");
// //             return;
// //         }

// //         string jsonContent = File.ReadAllText(configFilePath);
// //         obstacleConfig = JsonUtility.FromJson<ObstacleConfig>(jsonContent);

// //         if (obstacleConfig != null && obstacleConfig.groups != null)
// //         {
// //             Debug.Log($"Loaded Obstacle Config from: {configFilePath}");
// //         }
// //         else
// //         {
// //             Debug.LogError("Obstacle configuration is empty or invalid.");
// //         }
// //     }

// //     public void ReloadConfig()
// //     {
// //         LoadConfig();
// //         GenerateObstacles();
// //         Debug.Log("Obstacle configuration reloaded.");
// //     }

// //     void GenerateObstacles()
// //     {
// //         if (obstacleConfig == null)
// //         {
// //             Debug.LogError("Obstacle configuration is not loaded. Aborting obstacle generation.");
// //             return;
// //         }

// //         foreach (ObstacleGroup group in obstacleConfig.groups)
// //         {
// //             SpawnObstacleGroup(group);
// //         }
// //     }

// //     void SpawnObstacleGroup(ObstacleGroup group)
// //     {
// //         GameObject prefab = Resources.Load<GameObject>($"{prefabFolder}/{group.prefabName}");
// //         if (prefab == null)
// //         {
// //             Debug.LogError($"Prefab {group.prefabName} not found in Resources/{prefabFolder}");
// //             return;
// //         }

// //         for (int i = 0; i < group.count; i++)
// //         {
// //             float x = Random.Range(group.area.xMin, group.area.xMax);
// //             float y = Random.Range(group.area.yMin, group.area.yMax);
// //             float z = Random.Range(group.area.zMin, group.area.zMax);
// //             Vector3 position = new Vector3(x, y, z);

// //             Quaternion rotation = Quaternion.Euler(
// //                 group.rotationRange.x,
// //                 Random.Range(0, group.rotationRange.y),
// //                 group.rotationRange.z
// //             );

// //             Vector3 scale = new Vector3(
// //                 Random.Range(group.scaleRange.xMin, group.scaleRange.xMax),
// //                 Random.Range(group.scaleRange.yMin, group.scaleRange.yMax),
// //                 Random.Range(group.scaleRange.zMin, group.scaleRange.zMax)
// //             );

// //             GameObject obstacle = Instantiate(prefab, position, rotation);
// //             obstacle.transform.localScale = scale;

// //             Debug.Log($"Spawned {group.prefabName} at {position} with scale: {scale}");
// //         }
// //     }
// // }
