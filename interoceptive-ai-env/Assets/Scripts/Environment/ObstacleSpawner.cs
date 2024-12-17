using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Utility;

[System.Serializable]
public class ObstacleGroup
{
    public string prefabName;
    public int count;
    public PositionRange position;
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

    private ConfigLoader configLoader; // Reference to ConfigLoader

    public void InitializeObstacleSpawner(ConfigLoader loader)
    {
        configLoader = loader;
        if (configLoader == null)
        {
            Debug.LogError("ConfigLoader is not set. Ensure ConfigLoader is initialized.");
            return;
        }

        LoadConfig();
    }

    public void ReloadConfig()
    {
        LoadConfig();
    }

    public void InitializeObstacles(Transform court)
    {
        if (obstacleConfig == null)
        {
            Debug.LogError("Obstacle configuration is not loaded. Call ReloadConfig() before InitializeObstacles().");
            return;
        }

        courtTransform = court;
        GenerateObstacles();
    }

    private void LoadConfig()
    {
        if (configLoader == null)
        {
            Debug.LogError("ConfigLoader is not set. Ensure ConfigLoader is initialized.");
            return;
        }

        obstacleConfig = configLoader.LoadConfig<ObstacleConfig>(configFileName);

        if (obstacleConfig == null || obstacleConfig.groups == null)
        {
            Debug.LogError("Invalid or empty obstacle configuration.");
        }
    }

    public void ResetObstacles()
    {
        ClearObstacles();
        StartCoroutine(GenerateObstaclesWithDelay());
    }

    // Coroutine to delay the generation of obstacles to ensure old obstacles are fully destroyed
    private IEnumerator GenerateObstaclesWithDelay()
    {
        // Wait for the end of the frame to ensure objects are fully destroyed
        yield return new WaitForEndOfFrame(); // Wait for the end of the frame to ensure objects are fully destroyed
        GenerateObstacles();
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
        Resources.UnloadUnusedAssets(); // Ensure unused assets are unloaded
        Debug.Log("ObstacleSpawner: Old obstacles cleared.");
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
            Vector3 position = RandomPosition(group.position);
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

    private Vector3 RandomPosition(PositionRange position) =>
        new Vector3(
            Random.Range(position.xMin, position.xMax),
            Random.Range(position.yMin, position.yMax),
            Random.Range(position.zMin, position.zMax)
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
