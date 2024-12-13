using System.IO;
using UnityEngine;

public class ConfigLoader : MonoBehaviour
{
    [Header("Configuration")]
    public string mainConfigFileName = "mainConfig.json"; // Public variable for main config file name

    private string configFolderPath;

    public void InitializeConfigLoader()
    {
        LoadMainConfig();
    }

    private void LoadMainConfig()
    {
        string mainConfigPath = Application.isEditor
            ? Path.Combine(Application.dataPath, "../Config", mainConfigFileName)
            : Path.Combine(Directory.GetCurrentDirectory(), "Config", mainConfigFileName);

        if (!File.Exists(mainConfigPath))
        {
            Debug.LogError($"Main config file not found: {mainConfigPath}");
            return;
        }

        string jsonContent = File.ReadAllText(mainConfigPath);
        MainConfig mainConfig = JsonUtility.FromJson<MainConfig>(jsonContent);

        if (mainConfig == null || string.IsNullOrEmpty(mainConfig.configFolderName))
        {
            Debug.LogError("Invalid main config file.");
            return;
        }

        SetConfigFolder(mainConfig.configFolderName);
        Debug.Log($"ConfigLoader: SetConfigFodler as {configFolderPath}.");
    }

    private void SetConfigFolder(string folderName)
    {
        configFolderPath = Application.isEditor
            ? Path.Combine(Application.dataPath, "../Config", folderName)
            : Path.Combine(Directory.GetCurrentDirectory(), "Config", folderName);

        if (!Directory.Exists(configFolderPath))
        {
            Debug.LogError($"Config folder not found: {configFolderPath}");
        }
    }

    public T LoadConfig<T>(string configFileName) where T : new()
    {
        if (string.IsNullOrEmpty(configFolderPath))
        {
            Debug.LogError("Config folder is not set. Call SetConfigFolder() first.");
            return new T();
        }

        string configFilePath = Path.Combine(configFolderPath, configFileName);

        if (!File.Exists(configFilePath))
        {
            Debug.LogError($"Config file not found: {configFilePath}");
            return new T();
        }

        string jsonContent = File.ReadAllText(configFilePath);
        T config = JsonUtility.FromJson<T>(jsonContent);
        return config;
    }
}

[System.Serializable]
public class MainConfig
{
    public string configFolderName;
}
