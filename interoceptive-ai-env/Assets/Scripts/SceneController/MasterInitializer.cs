using UnityEngine;
using System.Collections;
using Unity.MLAgents;

public class MasterInitializer : MonoBehaviour
{
    private Academy academy;
    public ConfigLoader configLoader;
    public SpawnerManager spawnerManager;
    public ThermoGridSpawner thermoGridSpawner;
    public InteroceptiveAgent agent; // Single agent reference
    public HeatMap heatMap;
    public DayAndNight dayAndNight;
    public AgentFollowCamera agentFollowCamera;

    private void Start()
    {
        academy = Academy.Instance;
        InitializeScene();
    }

    private void InitializeScene()
    {
        // Pause the Academy/Agent updates
        academy.AutomaticSteppingEnabled = false;

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
        if (thermoGridSpawner != null)
        {
            thermoGridSpawner.InitializeThermoGridSpawner(configLoader);
            if (thermoGridSpawner == null || !thermoGridSpawner.isThermalGridReady)
            {
                Debug.LogError("ThermoGridSpawner is not ready. Initialization failed.");
                return;
            }
        }
        else
        {
            Debug.LogError("ThermoGridSpawner is not assigned.");
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
            if (heatMap.isInitialized)
            {
                Debug.Log("MasterInitializer: heatmap initialized.");
            }
            else
            {
                Debug.LogError("MasterInitializer: HeatMap failed to initialize.");
                // return;
            }
            heatMap.EpisodeHeatMap();
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
        academy.AutomaticSteppingEnabled = true;
        Debug.Log("MasterInitializer: Scene fully initialized, ML-Agents enabled.");
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneInOrder());
    }

    private IEnumerator ResetSceneInOrder()
    {
        // Pause the Academy/Agent updates during reset
        academy.AutomaticSteppingEnabled = false;
        // Step 1: Reset CourtSpawner
        if (spawnerManager != null)
        {
            // Start the ResetAllSpawnersCoroutine and wait for it to complete
            yield return StartCoroutine(spawnerManager.ResetAllSpawnersCoroutine());
            // This line will only execute after ResetAllSpawnersCoroutine completes
            Debug.Log("MasterInitializer: spawnerManager reset.");
        }
        else
        {
            Debug.LogError("spawnerManager is not assigned.");
        }

        // Step 2: Update HeatMap
        if (heatMap != null)
        {
            // This is a synchronous method call
            heatMap.EpisodeHeatMap();
            // This line will only execute after EpisodeHeatMap completes
            Debug.Log("MasterInitializer: HeatMap reset.");
        }
        else
        {
            Debug.LogError("HeatMap is not assigned.");
        }

        academy.AutomaticSteppingEnabled = true;
        Debug.Log("MasterInitializer: Scene reset complete, ML-Agents enabled.");
    }

    private bool IsHeatMapReady()
    {
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        return thermoGridSpawner != null && heatMap != null && heatMap.enabled;
    }
}