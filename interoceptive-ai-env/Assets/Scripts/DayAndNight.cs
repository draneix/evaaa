using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

// Attach this script to the Sun GameObject (Directional Light)
public class DayAndNight : MonoBehaviour
{
    // Reference to ML-Agents Environment Parameters
    private EnvironmentParameters m_ResetParams;

    [Header("Day/Night Settings")]
    public bool isNight = false;

    [Header("Sun Rotation")]
    public float eulerAnglesSun;
    public float daySpeed = 10f; // Default speed, can be overridden via EnvironmentParameters
    public float nightSpeed = 30f;

    [Header("Fog Settings")]
    public float fogChangeSpeed = 0.1f;
    public float dayFogDensity = 0f;
    public float nightFogDensity = 0.1f;
    private float currentFogDensity = 0.01f;

    [Header("Fog Color Settings")]
    [Tooltip("Color of the fog during the day.")]
    public Color dayFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    [Tooltip("Color of the fog during the night.")]
    public Color nightFogColor = new Color(0.0f, 0.0f, 0.0f, 1f);
    private Color targetFogColor;

    [Header("Camera View Settings")]
    [Tooltip("Reference to the main camera.")]
    public Camera mainCamera;

    [Tooltip("Minimum far clip plane distance during night.")]
    public float nightFarClip = 50f;

    [Tooltip("Maximum far clip plane distance during day.")]
    public float dayFarClip = 1000f;

    [Tooltip("Speed at which the far clip plane transitions.")]
    public float farClipTransitionSpeed = 5f;

    // Initial Camera Settings
    private float initialFarClip;
    private float targetFarClip;

    void Awake()
    {
        Academy.Instance.OnEnvironmentReset += SetParameters;
        eulerAnglesSun = transform.eulerAngles.x;
    }

    // Use this for initialization
    void Start()
    {
        // Ensure fog is enabled
        RenderSettings.fog = true;

        // Initialize Fog Settings
        RenderSettings.fogDensity = dayFogDensity;
        RenderSettings.fogColor = dayFogColor;
        currentFogDensity = dayFogDensity;
        targetFogColor = dayFogColor;

        // Initialize Camera Settings
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera is not assigned and no Camera with tag 'MainCamera' found.");
                return;
            }
        }

        initialFarClip = mainCamera.farClipPlane;
        targetFarClip = initialFarClip;
    }

    void SetParameters()
    {
        // Setting parameters from ML-Agents' Python
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        daySpeed = m_ResetParams.GetWithDefault("daySpeed", daySpeed);
        nightSpeed = m_ResetParams.GetWithDefault("nightSpeed", nightSpeed);
        fogChangeSpeed = m_ResetParams.GetWithDefault("fogChangeSpeed", fogChangeSpeed);

        // Optional: Set initial Fog Densities if provided via EnvironmentParameters
        // dayFogDensity = m_ResetParams.GetWithDefault("dayFogDensity", dayFogDensity);
        // nightFogDensity = m_ResetParams.GetWithDefault("nightFogDensity", nightFogDensity);
    }

    void Update()
    {
        eulerAnglesSun = transform.eulerAngles.x;

        // Determine if it's night based on Sun's angle
        if (eulerAnglesSun >= 170 || eulerAnglesSun <= -10) // Adjusted condition for wrap-around
        {
            if (!isNight)
            {
                isNight = true;
                // Rotate Sun clockwise
                transform.Rotate(Vector3.right, nightSpeed * Time.deltaTime);
            }
            else
            {
                // Continue rotating Sun
                transform.Rotate(Vector3.right, nightSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (isNight)
            {
                isNight = false;
                // Rotate Sun counter-clockwise
                transform.Rotate(Vector3.right, daySpeed * Time.deltaTime);
            }
            else
            {
                // Continue rotating Sun
                transform.Rotate(Vector3.right, daySpeed * Time.deltaTime);
            }
        }

        // Adjust Fog Density and Color
        AdjustFog();

        // Adjust Camera View Distance
        AdjustCameraView();
    }

    /// <summary>
    /// Adjusts the fog density and color based on day/night state.
    /// </summary>
    void AdjustFog()
    {
        if (isNight)
        {
            // Gradually increase fog density towards nightFogDensity
            if (currentFogDensity < nightFogDensity)
            {
                currentFogDensity += fogChangeSpeed * Time.deltaTime;
                currentFogDensity = Mathf.Min(currentFogDensity, nightFogDensity); // Clamp to prevent overshooting
                RenderSettings.fogDensity = currentFogDensity;
            }

            // Gradually change fog color towards nightFogColor
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, nightFogColor, fogChangeSpeed * Time.deltaTime);
        }
        else
        {
            // Gradually decrease fog density towards dayFogDensity
            if (currentFogDensity > dayFogDensity)
            {
                currentFogDensity -= fogChangeSpeed * Time.deltaTime;
                currentFogDensity = Mathf.Max(currentFogDensity, dayFogDensity); // Clamp to prevent undershooting
                RenderSettings.fogDensity = currentFogDensity;
            }

            // Gradually change fog color towards dayFogColor
            RenderSettings.fogColor = Color.Lerp(RenderSettings.fogColor, dayFogColor, fogChangeSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Adjusts the camera's far clip plane based on day/night state.
    /// </summary>
    void AdjustCameraView()
    {
        if (mainCamera == null)
            return;

        if (isNight)
        {
            // Set target far clip for night
            targetFarClip = nightFarClip;
        }
        else
        {
            // Set target far clip for day
            targetFarClip = dayFarClip;
        }

        // Smoothly interpolate the far clip plane
        mainCamera.farClipPlane = Mathf.Lerp(mainCamera.farClipPlane, targetFarClip, Time.deltaTime * farClipTransitionSpeed);
    }

    /// <summary>
    /// Returns whether it is currently night.
    /// </summary>
    public bool GetIsNight()
    {
        return isNight;
    }
}



// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using Unity.MLAgents;

// // GameObject인 Sun에 부착함
// // https://youtu.be/ci6ij1VKBLs
// public class DayAndNight : MonoBehaviour
// {
//         EnvironmentParameters m_ResetParams;
//         // 밤 여부 판단
//         public bool isNight = false;

//         [Header("Observations")]
//         // 현실 세계에서 1초가 지났을 때 게임 세계에서 몇 초가 지나도록 할 것인지 설정하기 위한 변수
//         public float eulerAnglesSun;
//         // public float dayNightSpeed = 0.0f; //원래 private 
//         //                                    // fog 증감량 비율
//         public float daySpeed;
//         public float nightSpeed;
//         public float fogChangeSpeed;
//         // 낮 상태의 fog 밀도.
//         public float dayFogDensity;
//         // 밤 상태의 fog 밀도.
//         public float nightFogDensity;
//         // 계산
//         public float currentFogDensity;
//         // public float dayTemperatureVariance = 0.0f;
//         // public float nightTemperatureVariance = -20.0f;

//         public void Awake()
//         {
//                 Academy.Instance.OnEnvironmentReset += SetParameters;
//                 eulerAnglesSun = transform.eulerAngles.x;
//         }

//         // Use this for initialization
//         void Start()
//         {
//                 // 유니티에 자체적으로 있는 fog 설정
//                 dayFogDensity = RenderSettings.fogDensity;
//                 // Linear로 설정을 바꾸면 연산이 간편해진다는 장점이 있다고 함 추후 고려해볼 것
//                 // RenderSettings.fogMode = FogMode.Linear;
//                 SetParameters();
//         }

//         void SetParameters()
//         {
//                 // Setting parameters from python
//                 m_ResetParams = Academy.Instance.EnvironmentParameters;
//                 // dayNightSpeed = m_ResetParams.GetWithDefault("dayNightSpeed", dayNightSpeed);
//                 daySpeed = m_ResetParams.GetWithDefault("daySpeed", daySpeed);
//                 nightSpeed = m_ResetParams.GetWithDefault("nightSpeed", nightSpeed);
//                 // dayTemperatureVariance = m_ResetParams.GetWithDefault("dayTemperatureVariance", dayTemperatureVariance);
//                 // nightTemperatureVariance = m_ResetParams.GetWithDefault("nightTemperatureVariance", nightTemperatureVariance);
//         }


//         // Update is called once per frame
//         // void Update()
//         void FixedUpdate()
//         {
//                 eulerAnglesSun = transform.eulerAngles.x;
//                 // GameObject인 Sun을 회전시키기 위한 함수
//                 // transform.Rotate(Vector3.right, 0.1f * dayNightSpeed * Time.deltaTime);
//                 // transform.Rotate(Vector3.right, dayNightSpeed * Time.deltaTime);

//                 // GameObject인 Sun의 오일러 각도를 기준으로 낮과 밤을 나눔
//                 if (eulerAnglesSun >= 170)
//                 {
//                         isNight = true;
//                         transform.Rotate(Vector3.right, nightSpeed * Time.deltaTime);
//                 }
//                 else if (eulerAnglesSun >= -10)
//                 {
//                         isNight = false;
//                         transform.Rotate(Vector3.right, daySpeed * Time.deltaTime);
//                 }

//                 if (isNight)
//                 {
//                         // 만약 밤일 경우 현재의 fog 농도를 조금씩 증가시켜 nightFogDensity에 수렴하도록 함
//                         if (currentFogDensity < nightFogDensity)
//                         {
//                                 currentFogDensity += 0.1f * fogChangeSpeed * Time.deltaTime;
//                                 RenderSettings.fogDensity = currentFogDensity;
//                         }
//                         else
//                         {
//                                 RenderSettings.fogDensity = nightFogDensity;
//                         }
//                 }
//                 else
//                 {
//                         // 만약 낮일 경우 현재의 fog 농도를 조금씩 감소시켜 dayFogDensity에 수렴하도록 함
//                         if (currentFogDensity > dayFogDensity)
//                         {
//                                 currentFogDensity -= 10f * fogChangeSpeed * Time.deltaTime;
//                                 RenderSettings.fogDensity = currentFogDensity;
//                         }
//                         else
//                         {
//                                 RenderSettings.fogDensity = dayFogDensity;
//                         }
//                 }
//                 // Debug.Log("isNight: " + isNight.ToString());
//         }

//         public bool GetIsNight()
//         {
//                 return isNight;
//         }
// }