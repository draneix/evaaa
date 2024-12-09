using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawner References")]
    public CourtSpawner courtSpawner;
    public ObstacleSpawner obstacleSpawner;
    public ResourceSpawner resourceSpawner;
    public ThermoGridSpawner thermoGridSpawner;

    public void InitializeSpawners()
    {
        // Step 1: Spawn the court
        if (courtSpawner != null)
        {
            courtSpawner.ReloadConfig();
            courtSpawner.InitializeCourt();
            Debug.Log("CourtSpawner Initialized.");
        }
        else
        {
            Debug.LogError("CourtSpawner is not assigned.");
            return;
        }

        // Step 2: Spawn obstacles
        if (obstacleSpawner != null)
        {
            obstacleSpawner.ReloadConfig();
            obstacleSpawner.InitializeObstacles(courtSpawner.CourtTransform);
            Debug.Log("ObstacleSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ObstacleSpawner is not assigned.");
        }

        // Step 3: Spawn resources
        if (resourceSpawner != null)
        {
            resourceSpawner.ReloadConfig();
            resourceSpawner.InitializeResources(courtSpawner.CourtTransform);
            Debug.Log("ResourceSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ResourceSpawner is not assigned.");
        }

        // Step 4: Initialize the thermal grid
        if (thermoGridSpawner != null)
        {
            thermoGridSpawner.ReloadConfig(); // Load configuration
            thermoGridSpawner.InitializeGrid(courtSpawner.CourtTransform); // Initialize based on court
            Debug.Log("ThermoGridSpawner Initialized.");
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }
    }

    public void ResetAllSpawners()
    {
        if (courtSpawner != null)
        {
            courtSpawner.ReloadConfig();
        }
        else
        {
            Debug.LogError("CourtSpawner is not assigned.");
        }

        if (obstacleSpawner != null)
        {
            obstacleSpawner.ResetObstacles();
        }
        else
        {
            Debug.LogError("ObstacleSpawner is not assigned.");
        }

        if (resourceSpawner != null)
        {
            resourceSpawner.ResetResources();
        }
        else
        {
            Debug.LogError("ResourceSpawner is not assigned.");
        }

        if (thermoGridSpawner != null)
        {
            thermoGridSpawner.ReloadConfig();
            thermoGridSpawner.ResetGrid();
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }

        Debug.Log("All spawners reset.");
    }
}
