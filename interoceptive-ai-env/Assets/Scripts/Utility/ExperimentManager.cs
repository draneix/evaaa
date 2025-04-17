using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public class ExperimentManager : MonoBehaviour
{
    [Header("Experiment Configuration")]
    public string experimentType;
    public int episodeNumber;
    public bool isActive = true;
    public InteroceptiveAgent targetAgent;

    [Header("Metrics Configuration")]
    public string outputDirectory = "ExperimentData";
    public string fileNamePrefix = "experiment_";
    public string resourceOffered = "";

    private ExperimentMetrics metrics;
    private string currentAction;  // Store the current action

    public void Initialize(InteroceptiveAgent agent)
    {
        targetAgent = agent;
        metrics = GetComponent<ExperimentMetrics>();
        if (metrics == null)
        {
            metrics = gameObject.AddComponent<ExperimentMetrics>();
        }
        metrics.Initialize(agent);
        episodeNumber = 0; // Start episode number from 0
        metrics.episodeNumber = episodeNumber;
    }

    public void RecordAction(string action)
    {
        if (!isActive) return;
        currentAction = action;  // Store the action
        if (metrics != null)
        {
            metrics.RecordAction(action);
        }
    }

    public void RecordStep()
    {
        if (!isActive) return;
        if (metrics != null)
        {
            metrics.RecordStep(currentAction);  // Use the stored action
        }
    }

    public void RecordCollision()
    {
        if (metrics != null)
        {
            metrics.RecordCollision();
        }
    }

    public void RecordFoodConsumed()
    {
        if (metrics != null)
        {
            metrics.RecordFoodConsumed();
            metrics.RecordAction("Eat_Food");
        }
    }

    public void RecordWaterConsumed()
    {
        if (metrics != null)
        {
            metrics.RecordWaterConsumed();
            metrics.RecordAction("Drink_Water");
        }
    }

    public void RecordFinalStep()
    {
        if (!isActive) return;
        if (metrics != null)
        {
            metrics.RecordFinalStep(currentAction);  // Use the stored action
        }
    }

    public void ExportData()
    {
        if (metrics != null)
        {
            metrics.ExportEpisodeSummary();
        }
    }

    public void SetResourceOffered(string resource)
    {
        resourceOffered = resource;
    }

    public void ResetExperiment()
    {
        if (metrics != null)
        {
            metrics.ExportEpisodeSummary();
            metrics.ResetMetrics();
        }
    }

    public void OnEpisodeBegin()
    {
        if (metrics != null)
        {
            episodeNumber++;
            metrics.episodeNumber = episodeNumber;
            metrics.InitializeEpisode();
            metrics.ResetMetrics();
        }
    }

    public void RecordResourceChoice(string resourceChosen)
    {
        if (metrics != null)
        {
            // No longer tracking resource choice and correctness
            metrics.RecordStep();
        }
    }

    public void OnEpisodeEnd()
    {
        if (metrics != null)
        {
            metrics.CalculateFinalMetrics();
            metrics.RecordFinalStep(); // Record final step
            metrics.ExportEpisodeSummary();
        }
    }
}