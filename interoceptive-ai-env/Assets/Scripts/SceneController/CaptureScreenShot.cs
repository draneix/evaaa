using System.IO;
using UnityEngine;
using Unity.MLAgents;

public class CaptureScreenShot : MonoBehaviour
{
        private int takes = 0;
        public bool recordEnable;
        private string recordingFolderName;
        private MainConfig mainConfig;
        private EnvironmentParameters m_ResetParams;
        private string mediaOutputFolder;

        public void Awake()
        {
                Academy.Instance.OnEnvironmentReset += SetParameters;
        }

        // Use this for initialization
        void Start()
        {
                // SetParameters();
                // Setting parameters from python
                m_ResetParams = Academy.Instance.EnvironmentParameters;

                ConfigLoader configLoader = FindObjectOfType<ConfigLoader>();
                if (configLoader != null)
                {
                mainConfig = configLoader.mainConfig;
                if (mainConfig != null)
                {
                        if (!string.IsNullOrEmpty(mainConfig.recordingFolderName))
                        {
                                recordingFolderName = mainConfig.recordingFolderName;
                                Debug.Log("mainConfig.recordingFolderName: " + mainConfig.recordingFolderName);
                        }
                        else{
                                recordingFolderName = "SampleRecordings";
                                Debug.Log("Use default recordingFolderName: " + recordingFolderName);
                        }
                        if (!string.IsNullOrEmpty(mainConfig.recordEnable))
                        {
                                // The mainConfig.recordEnable is string, which need to be transformed to bool
                                recordEnable = System.Convert.ToBoolean(mainConfig.recordEnable);

                                // recordEnable = mainConfig.recordEnable;
                                Debug.Log("Use mainConfig.recordEnable: " + recordEnable);
                        }
                        else
                        {
                                recordEnable = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("recordEnable", 0));
                                Debug.Log("Use m_ResetParams.GetWithDefault: " + recordEnable);
                        }
                }
                else
                {
                        Debug.LogError("mainConfig is null.");
                }
                }
                else
                {
                Debug.LogError("ConfigLoader not found.");
                }

                if (recordEnable)
                {
                        CreateRecordDirectory();
                }
        }

        void SetParameters()
        {
        }
        public void CreateRecordDirectory()
        {
                mediaOutputFolder = Path.Combine(Application.dataPath, "..", "Recordings", recordingFolderName);
                Debug.Log("mediaOutputFolder: " + mediaOutputFolder);

                DirectoryInfo directoryInfo = new DirectoryInfo(mediaOutputFolder);
                if (!directoryInfo.Exists)
                {
                        directoryInfo.Create();
                }
        }

        // This will be used in the InteroceptiveAgent.OnActionReceived() method
        public void CaptureImage()
        {
                takes = takes + 1;

                string s_takes = takes.ToString();
                ScreenCapture.CaptureScreenshot(Path.Combine(mediaOutputFolder, "record_") + s_takes.PadLeft(5, '0') + ".png");
        }

}
