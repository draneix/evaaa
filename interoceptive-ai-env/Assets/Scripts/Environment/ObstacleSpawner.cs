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
    public float temperature;
    public PositionRange position;
    public RotationRange rotationRange;
    public ScaleRange scaleRange;
}

public class ObstacleTemperature : MonoBehaviour
{
    public float temperature;
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

    // public void ResetObstacles()
    // {
    //     StartCoroutine(ClearAndGenerateObstacles());
    // }

    public IEnumerator ClearAndGenerateObstacles()
    {
        ClearObstacles();
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds to ensure obstacles are cleared
        GenerateObstacles();
        Debug.Log("ObstacleSpawner: New obstacles generated.");
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
        Debug.Log("ObstacleSpawner: Old obstacles have been cleared.");
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
            Vector3 position;
            Quaternion rotation;
            Vector3 scale;
            int attempts = 0;
            bool validPosition = false;

            do
            {
                position = RandomPosition(group.position);
                rotation = RandomRotation(group.rotationRange);
                scale = RandomScale(group.scaleRange);
                attempts++;
                validPosition = !OverlapUtility.IsOverlapping(position, prefab, scale);
            } while (!validPosition && attempts < 10);

            if (validPosition)
            {
                GameObject obstacle = Instantiate(prefab, position, rotation);

                if (courtTransform != null)
                {
                    obstacle.transform.SetParent(courtTransform);
                }

                obstacle.transform.localScale = scale;
                obstacle.AddComponent<ObstacleTemperature>().temperature = group.temperature; // Add temperature component
                spawnedObstacles.Add(obstacle);
                // Debug.Log($"Obstacle group {group.prefabName}: {obstacle.name} spawned at {position}.");
            }
            else
            {
                Debug.LogWarning($"Could not find a valid position for obstacle {group.prefabName} after {attempts} attempts.");
            }
        }
    }

    public List<GameObject> GetSpawnedObstacles()
    {
        return spawnedObstacles;
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
