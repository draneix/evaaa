using SpawnerUtilities;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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