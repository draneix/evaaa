using UnityEngine;
using System.Collections;

public class MasterInitializer : MonoBehaviour
{
    public ConfigLoader configLoader;
    public SpawnerManager spawnerManager;
    public InteroceptiveAgent agent; // Single agent reference
    public HeatMap heatMap;
    public DayAndNight dayAndNight;
    public AgentFollowCamera agentFollowCamera;

    private void Start()
    {
        InitializeScene();
    }

    private void InitializeScene()
    {
        // Step 1: Initialize ConfigLoader
        if (configLoader != null)
        {
            configLoader.InitializeConfigLoader(); // Ensure the configuration folder is set
            Debug.Log("MasterInitializer: ConfigLoader initialized.");
        }
        else
        {
            Debug.LogError("ConfigLoader is not assigned.");
            return;
        }

        // Step 2: Initialize SpawnerManager
        if (spawnerManager != null)
        {
            spawnerManager.InitializeSpawners(configLoader);
        }
        else
        {
            Debug.LogError("SpawnerManager is not assigned.");
            return;
        }

        // Step 3: Ensure ThermoGridSpawner is ready
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        if (thermoGridSpawner == null || !thermoGridSpawner.isThermalGridReady)
        {
            Debug.LogError("ThermoGridSpawner is not ready. Initialization failed.");
            return;
        }

        // Step 4: Initialize Agent
        if (agent != null)
        {
            agent.InitializeAgent(configLoader);
            Debug.Log("MasterInitializer: agent initialized.");
        }
        else
        {
            Debug.LogError("Agent is not assigned.");
            return;
        }

        // Step 5: Initialize HeatMap
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

        // Step 6: Initialize DayAndNight
        if (dayAndNight != null)
        {
            dayAndNight.InitializeDayAndNight(configLoader);
            Debug.Log("MasterInitializer: DayAndNight initialized.");
        }
        else
        {
            Debug.LogError("DayAndNight is not assigned.");
            return;
        }

        // Step 7: Initialize AgentFollowCamera
        if (agentFollowCamera != null)
        {
            agentFollowCamera.InitializeCamera(configLoader);
            Debug.Log("MasterInitializer: AgentFollowCamera initialized.");
        }
        else
        {
            Debug.LogError("AgentFollowCamera is not assigned.");
            return;
        }

        // Mark environment as ready
        InteroceptiveAgent.isEnvironmentReady = true;
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneInOrder());
    }

    private IEnumerator ResetSceneInOrder()
    {
        // Step 1: Reset CourtSpawner
        if (spawnerManager != null)
        {
            yield return StartCoroutine(spawnerManager.ResetAllSpawners());
            Debug.Log("MasterInitializer: spawnerManager reset.");
        }
        else
        {
            Debug.LogError("spawnerManager is not assigned.");
        }

        // Step 2: Update HeatMap
        if (heatMap != null)
        {
            heatMap.EpisodeHeatMap();
            Debug.Log("MasterInitializer: HeatMap reset.");
        }
        else
        {
            Debug.LogError("HeatMap is not assigned.");
        }
    }

    private bool IsHeatMapReady()
    {
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        return thermoGridSpawner != null && heatMap != null && heatMap.enabled;
    }
}