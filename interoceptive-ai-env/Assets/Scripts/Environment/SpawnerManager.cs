using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    [Header("Spawner References")]
    public CourtSpawner courtSpawner;
    public ObstacleSpawner obstacleSpawner;
    public ResourceSpawner resourceSpawner;
    public ThermoGridSpawner thermoGridSpawner;

    public void InitializeSpawners(ConfigLoader configLoader)
    {
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
            obstacleSpawner.InitializeObstacleSpawner(configLoader);
            obstacleSpawner.InitializeObstacles(courtSpawner.CourtTransform);
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
            // thermoGridSpawner.ReloadConfig();
            thermoGridSpawner.ResetGrid();
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
        }

        Debug.Log("All spawners reset.");
    }
}
