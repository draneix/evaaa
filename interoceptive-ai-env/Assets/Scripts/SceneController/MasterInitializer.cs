using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.MLAgents;
// using Assets.Scripts.Utility;

public class MasterInitializer : MonoBehaviour
{
    private Academy academy;
    public ConfigLoader configLoader;
    public SpawnerManager spawnerManager;
    public NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component
    public PredatorSpawner predatorSpawner; // Reference to the PredatorSpawner component
    
    public ThermoGridSpawner thermoGridSpawner;
    public InteroceptiveAgent agent; // Single agent reference
    public HeatMap heatMap;
    public DayAndNight dayAndNight;
    public AgentFollowCamera agentFollowCamera;
    // public CaptureScreenShot captureScreenShot;

    private void Start()
    {
        academy = Academy.Instance;
        InitializeScene();
    }

    private void InitializeScene()
    {
        // Step 1: Pause ML-Agents Academy
        academy.AutomaticSteppingEnabled = false;

        // Step 2: Load Configuration
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

        // Step 3: Initialize Spawners
        if (spawnerManager != null)
        {
            spawnerManager.InitializeSpawners(configLoader);
        }
        else
        {
            Debug.LogError("SpawnerManager is not assigned.");
            return;
        }

        // Step 4: Bake NavMesh
        if (navMeshSurface != null)
        {
            Debug.Log("MasterInitializer: Baking NavMesh...");
            navMeshSurface.BuildNavMesh(); // Dynamically bake the NavMesh
            Debug.Log("MasterInitializer: NavMesh baked successfully.");
        }
        else
        {
            Debug.LogError("NavMeshSurface is not assigned. NavMesh cannot be baked.");
        }

        // Step 5: Initialize PredatorSpawner
        if (predatorSpawner != null)
        {
            predatorSpawner.InitializePredatorSpawner(configLoader, spawnerManager.courtSpawner.CourtTransform);
            Debug.Log("MasterInitializer: PredatorSpawner initialized.");
        }
        else
        {
            Debug.LogError("PredatorSpawner is not assigned.");
        }

        // Step 6: Initialize ThermoGridSpawner
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

        // Step 7: Initialize Agent
        if (agent != null)
        {
            agent.InitializeAgent(configLoader);
            Debug.Log("MasterInitializer: Agent initialized.");
        }
        else
        {
            Debug.LogError("Agent is not assigned.");
            return;
        }

        // Step 8: Initialize HeatMap
        if (heatMap != null)
        {
            heatMap.InitializeHeatMap();
            if (heatMap.isInitialized)
            {
                Debug.Log("MasterInitializer: HeatMap initialized.");
            }
            else
            {
                Debug.LogError("MasterInitializer: HeatMap failed to initialize.");
            }
            heatMap.EpisodeHeatMap();
        }
        else
        {
            Debug.LogError("HeatMap is not assigned.");
            return;
        }

        // Step 9: Initialize Day/Night Cycle
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

        // Step 10: Initialize Camera
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

        // Step 11: Initialize Systems
        InitializeSystems();

        // Step 12: Resume ML-Agents Academy
        InteroceptiveAgent.isEnvironmentReady = true;
        academy.AutomaticSteppingEnabled = true;
        Debug.Log("MasterInitializer: Scene fully initialized, ML-Agents enabled.");
    }

    private void InitializeSystems()
    {
        // Initialize the experiment system
        ExperimentManager experimentManager = FindObjectOfType<ExperimentManager>();
        if (experimentManager != null)
        {
            experimentManager.Initialize(agent);
        }
        else
        {
            Debug.LogWarning("ExperimentManager not found in the scene. Experiment metrics will not be recorded.");
        }

        CaptureScreenShot captureScreenShot = FindObjectOfType<CaptureScreenShot>();
        // Initialize screen capture system
        if (captureScreenShot != null)
        {
            captureScreenShot.Initialize();
            Debug.Log("MasterInitializer: CaptureScreenShot initialized.");
        }
        else
        {
            Debug.LogError("CaptureScreenShot is not assigned.");
        }

        // Initialize other systems...
    }

    public void ResetScene()
    {
        StartCoroutine(ResetSceneInOrder());
    }

    private IEnumerator ResetSceneInOrder()
    {
        // Step 1: Pause the Academy/Agent updates during reset
        academy.AutomaticSteppingEnabled = false;

        // Step 2: Reset Spawners
        if (spawnerManager != null)
        {
            // Start the ResetAllSpawnersCoroutine and wait for it to complete
            yield return StartCoroutine(spawnerManager.ResetAllSpawnersCoroutine());
            Debug.Log("MasterInitializer: SpawnerManager reset.");
        }
        else
        {
            Debug.LogError("SpawnerManager is not assigned.");
        }

        // Step 3: Bake the NavMesh again
        if (navMeshSurface != null)
        {
            Debug.Log("MasterInitializer: Baking NavMesh after reset...");
            navMeshSurface.BuildNavMesh(); // Dynamically bake the NavMesh
            Debug.Log("MasterInitializer: NavMesh baked successfully after reset.");
        }
        else
        {
            Debug.LogError("NavMeshSurface is not assigned. NavMesh cannot be baked after reset.");
        }

        // Step 4: Update HeatMap
        if (heatMap != null)
        {
            heatMap.EpisodeHeatMap(); // Reset the heatmap for the new episode
            Debug.Log("MasterInitializer: HeatMap reset.");
        }
        else
        {
            Debug.LogError("HeatMap is not assigned.");
        }

        // Step 5: Resume the Academy/Agent updates
        academy.AutomaticSteppingEnabled = true;
        Debug.Log("MasterInitializer: Scene reset complete, ML-Agents enabled.");
    }

    private bool IsHeatMapReady()
    {
        var thermoGridSpawner = FindObjectOfType<ThermoGridSpawner>();
        return thermoGridSpawner != null && heatMap != null && heatMap.enabled;
    }
}