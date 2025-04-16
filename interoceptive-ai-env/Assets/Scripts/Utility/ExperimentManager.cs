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
    public InteroceptiveAgent targetAgent;

    [Header("Metrics Configuration")]
    public string outputDirectory = "ExperimentData";
    public string fileNamePrefix = "experiment_";
    public string resourceOffered = "";

    private ExperimentMetrics metrics;

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
        if (metrics != null)
        {
            metrics.RecordAction(action);
            metrics.RecordStep(action); // Record the step with the action
        }
    }

    public void RecordCollision()
    {
        if (metrics != null)
        {
            metrics.RecordCollision();
        }
    }

    public void RecordStep()
    {
        if (metrics != null)
        {
            metrics.RecordStep(); // Record step without action (for steps where no action was taken)
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
            // Increment episode number at the start of each episode
            episodeNumber++;
            metrics.episodeNumber = episodeNumber;
            
            metrics.InitializeEpisode(resourceOffered);
            metrics.ResetMetrics();
        }
    }

    public void RecordResourceChoice(string resourceChosen)
    {
        if (metrics != null)
        {
            metrics.currentEpisode.resourceChosen = resourceChosen;
            metrics.currentEpisode.correctChoice = resourceChosen == resourceOffered;
            // metrics.currentEpisode.decisionStep = metrics.stepData.Count;
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
}