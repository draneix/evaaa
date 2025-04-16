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

    [Header("Step-level Metrics")]
    public List<StepData> stepData = new List<StepData>();
    private string stepDataFileName;

    [Header("Episode-level Metrics")]
    public List<EpisodeData> episodeData = new List<EpisodeData>();
    private string episodeDataFileName;
    public EpisodeData currentEpisode;

    [Header("Data Export")]
    public string outputDirectory = "ExperimentData";
    public string fileNamePrefix = "experiment_";

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
    }

    public class EpisodeData
    {
        public int episodeNumber;
        public float initialFoodNeed;
        public float initialWaterNeed;
        public string resourceOffered;
        public string resourceChosen;
        public bool correctChoice;
        public int totalSteps;
        public float averageReward;
        public float maxReward;
        public float minReward;
        public float finalFoodLevel;
        public float finalWaterLevel;
        public float finalHealthLevel;
        public int collisions;
        public int foodConsumed;
        public int waterConsumed;
        public Dictionary<string, float> actionPercentages;
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

        InitializeDataFiles();
        isActive = true;
        episodeNumber = 1; // Start episode number from 1
    }

    public void InitializeEpisode(string resourceOffered)
    {
        currentEpisode = new EpisodeData
        {
            episodeNumber = this.episodeNumber,
            initialFoodNeed = targetAgent.resourceLevels[0],
            initialWaterNeed = targetAgent.resourceLevels[1],
            resourceOffered = resourceOffered,
            actionPercentages = new Dictionary<string, float>()
        };
    }

    private void InitializeDataFiles()
    {
        // Create output directory if it doesn't exist
        if (!Directory.Exists(outputDirectory))
        {
            Directory.CreateDirectory(outputDirectory);
        }

        // Generate filenames with timestamp
        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        stepDataFileName = $"{fileNamePrefix}{experimentType}_steps_{timestamp}.csv";
        episodeDataFileName = $"{fileNamePrefix}{experimentType}_episodes_{timestamp}.csv";

        // Initialize step data file header
        using (StreamWriter writer = new StreamWriter(Path.Combine(outputDirectory, stepDataFileName)))
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
                "IsEpisodeEnd");
        }

        // Initialize episode data file header
        using (StreamWriter writer = new StreamWriter(Path.Combine(outputDirectory, episodeDataFileName)))
        {
            writer.WriteLine("Episode," +
                "Need_Food," +
                "Need_Water," +
                "Offered," +
                "Chosen," +
                "Correct," +
                "TotalSteps," +
                "AvgReward," +
                "MaxReward," +
                "MinReward," +
                "FinalFood," +
                "FinalWater," +
                "FinalHealth," +
                "Collisions," +
                "FoodConsumed," +
                "WaterConsumed," +
                "Forward%," +
                "Backward%," +
                "Left%," +
                "Right%," +
                "Eat%," +
                "Drink%");
        }

        // Initialize current episode data
        currentEpisode = new EpisodeData
        {
            episodeNumber = this.episodeNumber,
            actionPercentages = new Dictionary<string, float>()
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
            isEpisodeEnd = isFinalStep
        };

        // Add to step data list
        stepData.Add(step);

        // Write to step data file immediately
        try
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(outputDirectory, stepDataFileName), true))
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
                    $"{step.isEpisodeEnd}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error writing step data: {e.Message}");
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
        if (!isActive) return;

        try
        {
            string episodeSummaryPath = Path.Combine(outputDirectory, episodeDataFileName);
            bool fileExists = File.Exists(episodeSummaryPath);

            using (StreamWriter writer = new StreamWriter(episodeSummaryPath, true))
            {
                if (!fileExists)
                {
                    writer.WriteLine("Episode,Need_Food,Need_Water,Offered,Chosen,Correct,TotalSteps,AvgReward,MaxReward,MinReward,FinalFood,FinalWater,FinalHealth,Collisions,FoodConsumed,WaterConsumed,Forward%,Backward%,Left%,Right%,Eat%,Drink%");
                }

                writer.WriteLine($"{currentEpisode.episodeNumber}," +
                    $"{currentEpisode.initialFoodNeed}," +
                    $"{currentEpisode.initialWaterNeed}," +
                    $"{currentEpisode.resourceOffered}," +
                    $"{currentEpisode.resourceChosen}," +
                    $"{currentEpisode.correctChoice}," +
                    $"{currentEpisode.totalSteps}," +
                    $"{currentEpisode.averageReward:F2}," +
                    $"{currentEpisode.maxReward:F2}," +
                    $"{currentEpisode.minReward:F2}," +
                    $"{currentEpisode.finalFoodLevel:F2}," +
                    $"{currentEpisode.finalWaterLevel:F2}," +
                    $"{currentEpisode.finalHealthLevel:F2}," +
                    $"{currentEpisode.collisions}," +
                    $"{currentEpisode.foodConsumed}," +
                    $"{currentEpisode.waterConsumed}," +
                    $"{GetActionPercentage("Forward"):F2}," +
                    $"{GetActionPercentage("Backward"):F2}," +
                    $"{GetActionPercentage("Left"):F2}," +
                    $"{GetActionPercentage("Right"):F2}," +
                    $"{GetActionPercentage("Eat"):F2}," +
                    $"{GetActionPercentage("Drink"):F2}");
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
} 