using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

public class ExperimentMetrics : MonoBehaviour
{
    [Header("Experiment Configuration")]
    public string experimentType;
    public int episodeNumber;
    public bool isActive = true;
    public InteroceptiveAgent targetAgent;
    private bool recordEnable;

    [Header("Step-level Metrics")]
    public List<StepData> stepData = new List<StepData>();
    private string stepDataFileName;

    [Header("Episode-level Metrics")]
    public List<EpisodeData> episodeData = new List<EpisodeData>();
    private string episodeDataFileName;
    public EpisodeData currentEpisode;

    [Header("Data Export")]
    private string outputDirectory;
    private string baseFolderName;
    private string fileNamePrefix;
    private MainConfig mainConfig;

    public class StepData
    {
        public int stepNumber;
        public float foodLevel;
        public float waterLevel;
        public float thermoLevel;
        public float healthLevel;
        public Vector3 position;
        public string action;
        public float reward;
        public float distanceTraveled;
        public bool isEpisodeEnd;  // Flag to indicate if this step ended the episode
        public bool hasCollision;  // New field to track collision in each step
        public bool resourceConsumed;  // New field to track if any resource was consumed in this step
        public string consumedResourceType;  // Track which resource was consumed (using actual tag from eatenResourceTag)
    }

    public class EpisodeData
    {
        public int episodeNumber;
        public int totalSteps;
        public float averageReward;
        public float maxReward;
        public float minReward;
        public int foodConsumed;
        public int waterConsumed;
        public float finalFoodLevel;
        public float finalWaterLevel;
        public float finalHealthLevel;
        public int collisions;
        public Dictionary<string, float> actionPercentages;
        public string episodeEndType;  // Track how the episode ended
    }

    public void Initialize(InteroceptiveAgent agent)
    {
        targetAgent = agent;
        if (targetAgent == null)
        {
            Debug.LogError("ExperimentMetrics: Target agent not assigned!");
            isActive = false;
            return;
        }

        // Get ConfigLoader and mainConfig
        ConfigLoader configLoader = FindObjectOfType<ConfigLoader>();
        if (configLoader != null)
        {
            mainConfig = configLoader.mainConfig;
            if (mainConfig != null)
            {
                baseFolderName = mainConfig.experimentData.baseFolderName;
                fileNamePrefix = mainConfig.experimentData.fileNamePrefix;
                recordEnable = mainConfig.experimentData.recordEnable;
                // Debug.Log($"ExperimentMetrics: Using baseFolderName={baseFolderName}, fileNamePrefix={fileNamePrefix}");
            }
            else
            {
                Debug.LogError("ExperimentMetrics: mainConfig or experimentData is null");
                isActive = false;
                return;
            }
        }
        else
        {
            Debug.LogError("ExperimentMetrics: ConfigLoader not found");
            isActive = false;
            return;
        }

        if (recordEnable)
        {
            // Create timestamp-based directory structure
            string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
            outputDirectory = Path.Combine(Application.dataPath, "..",  baseFolderName);
            Debug.Log($"ExperimentMetrics: Using outputDirectory={outputDirectory}");
            
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            InitializeDataFiles();
        }
        isActive = true;
        episodeNumber = 1; // Start episode number from 1
    }

    public void InitializeEpisode()
    {
        currentEpisode = new EpisodeData
        {
            episodeNumber = this.episodeNumber,
            totalSteps = 0,
            averageReward = 0f,
            maxReward = float.MinValue,
            minReward = float.MaxValue,
            foodConsumed = 0,
            waterConsumed = 0,
            finalFoodLevel = 0f,
            finalWaterLevel = 0f,
            finalHealthLevel = 0f,
            collisions = 0,
            actionPercentages = new Dictionary<string, float>(),
            episodeEndType = "Unknown"  // Initialize with unknown
        };
    }

    private void InitializeDataFiles()
    {
        if (!recordEnable) return;

        // Generate filenames with timestamp
        string timestamp = System.DateTime.Now.ToString("yyyyMMdd_HHmmss");
        stepDataFileName = Path.Combine(outputDirectory, $"{fileNamePrefix}{experimentType}_steps_{timestamp}.csv");
        episodeDataFileName = Path.Combine(outputDirectory, $"{fileNamePrefix}{experimentType}_episodes_{timestamp}.csv");

        // Initialize step data file header
        using (StreamWriter writer = new StreamWriter(stepDataFileName))
        {
            writer.WriteLine("Episode," +
                "Step," +
                "FoodLevel," +
                "WaterLevel," +
                "ThermoLevel," +
                "HealthLevel," +
                "PositionX," +
                "PositionY," +
                "PositionZ," +
                "Action," +
                "Reward," +
                "DistanceTraveled," +
                "IsEpisodeEnd," +
                "HasCollision," +
                "ResourceConsumed," +
                "ConsumedResourceType");
        }

        // Initialize episode data file header
        using (StreamWriter writer = new StreamWriter(episodeDataFileName))
        {
            writer.WriteLine("Episode," +
                "TotalSteps," +
                "AvgReward," +
                "MaxReward," +
                "MinReward," +
                "FoodConsumed," +
                "WaterConsumed," +
                "FinalFoodLevel," +
                "FinalWaterLevel," +
                "FinalHealthLevel," +
                "Collisions," +
                "None%," +
                "Forward%," +
                "Left%," +
                "Right%," +
                "Eat%," +
                "EpisodeEndType");
        }

        // Initialize current episode data
        currentEpisode = new EpisodeData
        {
            episodeNumber = this.episodeNumber,
            totalSteps = 0,
            averageReward = 0f,
            maxReward = float.MinValue,
            minReward = float.MaxValue,
            foodConsumed = 0,
            waterConsumed = 0,
            finalFoodLevel = 0f,
            finalWaterLevel = 0f,
            finalHealthLevel = 0f,
            collisions = 0,
            actionPercentages = new Dictionary<string, float>(),
            episodeEndType = "Unknown"  // Initialize with unknown
        };
    }

    private void RecordStepInternal(string action, bool isFinalStep)
    {
        if (!isActive || targetAgent == null) return;

        // Create new step data
        StepData step = new StepData
        {
            stepNumber = stepData.Count + 1,
            foodLevel = targetAgent.resourceLevels[0],
            waterLevel = targetAgent.resourceLevels[1],
            thermoLevel = targetAgent.resourceLevels[2],
            healthLevel = targetAgent.resourceLevels[3],
            position = targetAgent.transform.position,
            reward = targetAgent.currentReward,
            distanceTraveled = Vector3.Distance(targetAgent.transform.position, 
                stepData.Count > 0 ? stepData[stepData.Count - 1].position : targetAgent.transform.position),
            action = action,
            isEpisodeEnd = isFinalStep,
            hasCollision = targetAgent.countCollision > 0,
            resourceConsumed = targetAgent.resourceConsumedInStep,  // Track if any resource was consumed
            consumedResourceType = targetAgent.consumedResourceType  // Track which resource was consumed
        };

        // Add to step data list
        stepData.Add(step);

        if (recordEnable)
        {
            // Write to step data file immediately
            try
            {
                using (StreamWriter writer = new StreamWriter(stepDataFileName, true))
                {
                    writer.WriteLine($"{episodeNumber}," +
                        $"{step.stepNumber}," +
                        $"{step.foodLevel:F2}," +
                        $"{step.waterLevel:F2}," +
                        $"{step.thermoLevel:F2}," +
                        $"{step.healthLevel:F2}," +
                        $"{step.position.x:F2}," +
                        $"{step.position.y:F2}," +
                        $"{step.position.z:F2}," +
                        $"{step.action}," +
                        $"{step.reward:F2}," +
                        $"{step.distanceTraveled:F2}," +
                        $"{step.isEpisodeEnd}," +
                        $"{step.hasCollision}," +
                        $"{step.resourceConsumed}," +
                        $"{step.consumedResourceType}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error writing step data: {e.Message}");
            }
        }
    }

    public void RecordStep(string action = "")
    {
        RecordStepInternal(action, false);
    }

    public void RecordFinalStep(string action = "")
    {
        RecordStepInternal(action, true);
    }

    public void RecordAction(string action)
    {
        if (!isActive) return;

        // Update action counts for current episode
        if (!currentEpisode.actionPercentages.ContainsKey(action))
        {
            currentEpisode.actionPercentages[action] = 0;
        }
        currentEpisode.actionPercentages[action]++;

        // Update last step's action
        if (stepData.Count > 0)
        {
            stepData[stepData.Count - 1].action = action;
        }
    }

    public void RecordCollision()
    {
        if (!isActive) return;
        currentEpisode.collisions++;
    }

    public void ExportEpisodeSummary()
    {
        if (!isActive || !recordEnable) return;

        try
        {
            string episodeSummaryPath = episodeDataFileName;
            bool fileExists = File.Exists(episodeSummaryPath);

            using (StreamWriter writer = new StreamWriter(episodeSummaryPath, true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("Episode,TotalSteps,AvgReward,MaxReward,MinReward,FoodConsumed,WaterConsumed,FinalFoodLevel,FinalWaterLevel,FinalHealthLevel,Collisions,None%,Forward%,Left%,Right%,Eat%,EpisodeEndType");
                }

                writer.WriteLine($"{currentEpisode.episodeNumber}," +
                    $"{currentEpisode.totalSteps}," +
                    $"{currentEpisode.averageReward:F2}," +
                    $"{currentEpisode.maxReward:F2}," +
                    $"{currentEpisode.minReward:F2}," +
                    $"{currentEpisode.foodConsumed}," +
                    $"{currentEpisode.waterConsumed}," +
                    $"{currentEpisode.finalFoodLevel:F2}," +
                    $"{currentEpisode.finalWaterLevel:F2}," +
                    $"{currentEpisode.finalHealthLevel:F2}," +
                    $"{currentEpisode.collisions}," +
                    $"{GetActionPercentage("None"):F2}," +
                    $"{GetActionPercentage("Forward"):F2}," +
                    $"{GetActionPercentage("Left"):F2}," +
                    $"{GetActionPercentage("Right"):F2}," +
                    $"{GetActionPercentage("Eat"):F2}," +
                    $"{currentEpisode.episodeEndType}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error exporting episode summary: {e.Message}");
        }
    }

    public void CalculateFinalMetrics()
    {
        if (stepData.Count == 0) return;

        // Set total steps to the last step number
        currentEpisode.totalSteps = stepData.Count;

        // Calculate reward statistics
        currentEpisode.averageReward = stepData.Average(s => s.reward);
        currentEpisode.maxReward = stepData.Max(s => s.reward);
        currentEpisode.minReward = stepData.Min(s => s.reward);

        // Get final resource levels
        currentEpisode.finalFoodLevel = stepData.Last().foodLevel;
        currentEpisode.finalWaterLevel = stepData.Last().waterLevel;
        currentEpisode.finalHealthLevel = stepData.Last().healthLevel;

        // Calculate action percentages
        float totalActions = currentEpisode.actionPercentages.Values.Sum();
        if (totalActions > 0)
        {
            foreach (var action in currentEpisode.actionPercentages.Keys.ToList())
            {
                currentEpisode.actionPercentages[action] = (currentEpisode.actionPercentages[action] / totalActions) * 100f;
            }
        }

        // Ensure all actions are represented
        string[] allActions = { "None", "Forward", "Backward", "Left", "Right", "Eat", "Drink" };
        foreach (var action in allActions)
        {
            if (!currentEpisode.actionPercentages.ContainsKey(action))
            {
                currentEpisode.actionPercentages[action] = 0f;
            }
        }
    }

    private float GetActionPercentage(string action)
    {
        return currentEpisode.actionPercentages.ContainsKey(action) ? currentEpisode.actionPercentages[action] : 0f;
    }

    public void ResetMetrics()
    {
        // Clear step data
        stepData.Clear();

        // Initialize new episode data
        currentEpisode = new EpisodeData
        {
            episodeNumber = this.episodeNumber,
            actionPercentages = new Dictionary<string, float>()
        };
    }

    public void RecordFoodConsumed()
    {
        if (!isActive) return;
        currentEpisode.foodConsumed++;
    }

    public void RecordWaterConsumed()
    {
        if (!isActive) return;
        currentEpisode.waterConsumed++;
    }

    [System.Serializable]
    private class ConfigData
    {
        public ExperimentDataConfig experimentData;
    }

    [System.Serializable]
    private class ExperimentDataConfig
    {
        public string baseFolderName;
        public string fileNamePrefix;
        public bool recordEnable;
    }
} 