using UnityEngine;
using System.Collections;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawner References")]
    public CourtSpawner courtSpawner;
    public ObstacleSpawner obstacleSpawner;
    public ResourceSpawner resourceSpawner;
    public ThermoGridSpawner thermoGridSpawner;

    public void InitializeSpawners(ConfigLoader configLoader)
    {
        Debug.Log("SpawnerManager: Initializing spawners...");
        // Step 1: Initialize CourtSpawner
        if (courtSpawner != null)
        {
            courtSpawner.InitializeCourt(configLoader);
            Debug.Log("CourtSpawner Initialized.");
        }
        else
        {
            Debug.LogError("CourtSpawner is not assigned.");
            return;
        }

        // Step 2: Initialize ObstacleSpawner
        if (obstacleSpawner != null)
        {
            obstacleSpawner.InitializeObstacleSpawner(configLoader, courtSpawner.CourtTransform);
            Debug.Log("ObstacleSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ObstacleSpawner is not assigned.");
        }

        // Step 3: Initialize ResourceSpawner
        if (resourceSpawner != null)
        {
            resourceSpawner.InitializeResourceSpawner(configLoader);
            resourceSpawner.InitializeResources(courtSpawner.CourtTransform);
            Debug.Log("ResourceSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ResourceSpawner is not assigned.");
        }

        // Step 4: Initialize ThermoGridSpawner
        if (thermoGridSpawner != null)
        {
            thermoGridSpawner.InitializeThermoGridSpawner(configLoader);
            thermoGridSpawner.InitializeGrid(courtSpawner.CourtTransform);
            Debug.Log("ThermoGridSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }
    }

    public void ResetAllSpawners()
    {
        StartCoroutine(ResetAllSpawnersCoroutine());
    }

    public IEnumerator ResetAllSpawnersCoroutine()
    {
        // Step 1: Reset CourtSpawner
        if (courtSpawner != null)
        {
            courtSpawner.ReloadConfig();
            yield return null; // Wait for one frame to ensure the operation is completed
        }
        else
        {
            Debug.LogError("CourtSpawner is not assigned.");
        }

        // Step 2: Reset ObstacleSpawner
        if (obstacleSpawner != null)
        {
            yield return StartCoroutine(obstacleSpawner.ClearAndGenerateObstacles());
        }
        else
        {
            Debug.LogError("ObstacleSpawner is not assigned.");
        }

        // Step 3: Reset ResourceSpawner
        if (resourceSpawner != null)
        {
            resourceSpawner.ResetResources();
            yield return null; // Wait for one frame to ensure the operation is completed
        }
        else
        {
            Debug.LogError("ResourceSpawner is not assigned.");
        }

        // Step 4: Reset ThermoGridSpawner
        if (thermoGridSpawner != null)
        {
            thermoGridSpawner.ResetGrid();
            yield return null; // Wait for one frame to ensure the operation is completed
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }

        Debug.Log("SpawnerManager: All spawners reset.");
    }
}
