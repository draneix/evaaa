using UnityEngine;

public class MasterInitializer : MonoBehaviour
{
    public SpawnerManager spawnerManager;
    public InteroceptiveAgent agent; // Single agent reference
    public HeatMap heatMap;

    private void Start()
    {
        InitializeScene();
    }

    private void InitializeScene()
    {
        // Step 1: Initialize SpawnerManager
        if (spawnerManager != null)
        {
            spawnerManager.InitializeSpawners();
        }
        else
        {
            Debug.LogError("SpawnerManager is not assigned.");
            return;
        }

        // Step 2: Ensure ThermoGridSpawner is ready
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        if (thermoGridSpawner == null || !thermoGridSpawner.isThermalGridReady)
        {
            Debug.LogError("ThermoGridSpawner is not ready. Initialization failed.");
            return;
        }

        // Step 3: Initialize Agent
        if (agent != null)
        {
            agent.Initialize();
            Debug.Log("MasterInitializer: agent initialized.");
        }
        else
        {
            Debug.LogError("Agent is not assigned.");
            return;
        }

        // Step 4: Initialize HeatMap
        if (heatMap != null)
        {
            heatMap.InitializeHeatMap();
            if (!IsHeatMapReady())
            {
                Debug.Log("HeatMap failed to initialize.");
                return;
            }
            Debug.Log("MasterInitializer: heatmap initialized.");
        }
        else
        {
            Debug.LogError("HeatMap is not assigned.");
            return;
        }

        // Mark environment as ready
        InteroceptiveAgent.isEnvironmentReady = true;
        Debug.Log("MasterInitializer: All components initialized.");
    }

    private bool IsHeatMapReady()
    {
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        return thermoGridSpawner != null && heatMap != null && heatMap.enabled;
    }
}