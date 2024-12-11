using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Assets.Scripts.Utility;

public class InteroceptiveAgent : Agent
{
        [Header("Configuration")]
        public string configFileName = "agentConfig.json";
        public static bool isEnvironmentReady = false; // Global readiness flag
        protected EnvironmentParameters m_ResetParams;
        protected ResourceProperty[] FoodObjects;
        protected Rigidbody m_AgentRb;
        protected bool isAgentActionEat = false;
        public bool IsAgentActionEat { get { return this.isAgentActionEat; } set { this.isAgentActionEat = value; } }
        protected bool eatenResource = false;
        public bool EatenResource { get { return this.eatenResource; } set { this.eatenResource = value; } }
        protected string eatenResourceTag;
        public string EatenResourceTag { get { return this.eatenResourceTag; } set { this.eatenResourceTag = value; } }
        protected float bodyTemp;
        protected GameObject[] agents;

        public bool isAIControlled; // Default to true for AI control

        [Header("Game Ojects for script")]
        public GameObject heatMap;
        public GameObject playRecorder;
        public CameraSwitcher camareManager;

        [Header("Environment settings")]
        public bool singleTrial;
        public bool initRandomAgentPosition;

        public ThreeDVector initAgentPosition;
        public ThreeDVector initAgentAngle;

        [Header("Actions")]
        public float moveSpeed;
        public float turnSpeed;
        public float eatingDistance;
        public bool autoEat;
        public Vector3 agentRotation;
        public Vector3 agentPosition;

        [Header("Observations")]
        public bool useTouchObs;
        public float touchObservation;
        public bool isTouched;
        public bool useOlfactoryObs;
        public float olfactorySensorLength;
        public int olfactoryFeatureSize;
        public float[] olfactoryObservation;
        public bool useThermalObs;
        public bool relativeThermalObs;
        public float[] thermoObservation;
        public GameObject thermoSensorCenter; 
        public GameObject thermoSensorForward;
        public GameObject thermoSensorBackward;
        public GameObject thermoSensorLeft;
        public GameObject thermoSensorRight;
        public GameObject thermoSensorForwardLeft;
        public GameObject thermoSensorForwardRight;
        public GameObject thermoSensorBackwardLeft;
        public GameObject thermoSensorBackwardRight;
        public ObjectRaycast objectRaycast;
        public bool isCollisionDetected;
        public bool useCollisionObs;
        public float[] collisionObservation;
        public int collisionFeatureSize;
        private Queue<float> recentRewards; // Queue for storing recent rewards
        public int rewardWindowSize; // Size of the moving average window
        public float averageReward; // Calculated average reward
        public float currentReward; // Current reward for this step

        [Header("Essential variables (EV)")]
        public int countEV;
        public float[] resourceLevels;
        private float[] oldResourceLevels;

        [Header("Food")]
        public EVRange foodLevelRange;
        public float resourceFoodValue;
        public float startFoodLevel;

        // blue
        [Header("Water")]
        public EVRange waterLevelRange;
        public float resourceWaterValue;
        public float startWaterLevel;

        // yellow
        [Header("Temperature")]
        public EVRange thermoLevelRange;
        public float startThermoLevel;

        // hp
        [Header("Health")]
        public EVRange healthLevelRange;
        public float startHealthLevel;

        // Food Function Coefficient
        // Index 0 : Constant Decay
        // Index 1 : Food Effect
        // Index 2 : Water Effect
        // Index 3 : Thermo Effect
        // Index 4 : Interaction Effect
        // Index 5 : Discrete Change (Eating)
        [Header("Coefficient for EV dynamics")]
        public Coefficient foodCoefficient;
        public Coefficient waterCoefficient;
        public Coefficient thermoCoefficient;
        public Coefficient healthCoefficient;


        [Header("Collision System")]
        public float raysPerDirection;
        public float maxDistance;
        public float radialRange;
        public float damageConstant;
        private void LoadConfig()
        {
                string configFolderPath = Application.isEditor
                ? Path.Combine(Application.dataPath, "../Config")
                : Path.Combine(Directory.GetCurrentDirectory(), "Config");

                string configFilePath = Path.Combine(configFolderPath, configFileName);

                if (!File.Exists(configFilePath))
                {
                        Debug.LogError($"Config file not found: {configFilePath}");
                        return;
                }

                string json = File.ReadAllText(configFilePath);
                JsonUtility.FromJsonOverwrite(json, this);
        }       
        public override void Initialize()
        {
                LoadConfig();
                m_ResetParams = Academy.Instance.EnvironmentParameters;
                SetResetParameters();
                                
                // Update the CameraSwitcher if available
                if (camareManager != null)
                {
                        camareManager.isAIControlled = isAIControlled;
                }
                // Set initial position and rotation
                if (!initRandomAgentPosition)
                {
                        transform.position = initAgentPosition.ToVector3();
                        transform.eulerAngles = initAgentAngle.ToVector3();
                }

                m_AgentRb = GetComponent<Rigidbody>();
                m_AgentRb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
                eatenResource = false;

                this.agentPosition = this.transform.position;
                this.agentRotation = this.transform.eulerAngles;

                this.resourceLevels = new float[this.countEV];
                this.oldResourceLevels = new float[this.countEV];

                if (this.useOlfactoryObs)
                {
                        this.olfactoryObservation = new float[this.olfactoryFeatureSize];
                }
                if (this.useThermalObs)
                {
                        this.thermoObservation = new float[8];
                }
                if (this.useCollisionObs)
                {
                        // isCollided = true;
                        this.collisionObservation = new float[this.collisionFeatureSize];
                }
                if (this.useTouchObs)
                {
                        this.touchObservation = 0.0f;
                }
                
                recentRewards = new Queue<float>(rewardWindowSize);
        }

        public override void OnEpisodeBegin()
        {

                if (!isEnvironmentReady)
                {
                        Debug.LogWarning("Environment is not ready. Skipping OnEpisodeBegin.");
                        return;
                }

                // Find the SpawnerManager and reset all spawners
                var spawnerManager = FindObjectOfType<SpawnerManager>();
                if (spawnerManager != null)
                {
                        spawnerManager.ResetAllSpawners();
                }
                else
                {
                        Debug.LogError("SpawnerManager not found in the scene.");
                }
                // Reset agent
                m_AgentRb.velocity = Vector3.zero;

                eatenResource = false;

                SetResetParameters();

                // Reset energy
                for (int i = 0; i < this.countEV; i++)
                {
                        if (i == 0)
                        {
                                this.resourceLevels[i] = startFoodLevel;
                                this.oldResourceLevels[i] = this.resourceLevels[i];
                        }
                        else if (i == 1)
                        {
                                this.resourceLevels[i] = startWaterLevel;
                                this.oldResourceLevels[i] = this.resourceLevels[i];
                        }
                        else if (i == 2)
                        {
                                this.resourceLevels[i] = startThermoLevel;
                                this.oldResourceLevels[i] = this.resourceLevels[i];
                        }
                        else if (i == 3)
                        {
                                this.resourceLevels[i] = startHealthLevel;
                                this.oldResourceLevels[i] = this.resourceLevels[i];
                        }
                }

                // Reset olfactory
                if (this.useOlfactoryObs)
                {
                        for (int i = 0; i < olfactoryFeatureSize; i++)
                        {
                                this.olfactoryObservation[i] = 0;
                        }
                }
                

                if (this.useThermalObs)
                {
                        for (int i = 0; i < 8; i++)
                        {
                                this.thermoObservation[i] = 0;
                        }

                        bodyTemp = 0;

                        // thermoSensorCenter.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorForward.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorBackward.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorLeft.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorRight.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorForwardLeft.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorForwardRight.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorBackwardLeft.GetComponent<ThermalSensing>().SetThermalSense(0);
                        thermoSensorBackwardRight.GetComponent<ThermalSensing>().SetThermalSense(0);
                        // // Find all spotlight hotzones and set their hotzones
                        // SpotlightHotzone[] spotlightHotzones = FindObjectsOfType<SpotlightHotzone>(); // Get all spotlight hotzones
                        // foreach (var spotlightHotzone in spotlightHotzones)
                        // {
                        //         spotlightHotzone.ApplySpotlightHotzone(); // Set hotzone for each spotlight
                        // }
                        // Reset heatmap
                        heatMap.GetComponent<HeatMap>().EpisodeHeatMap();
                }

                if (useCollisionObs)
                {
                        // this.collisionObservation = 0.0f;
                        for (int i = 0; i < 10; i++)
                        {
                                this.collisionObservation[i] = 0;
                        }                                            

                }

                if (useTouchObs)
                {
                        this.touchObservation = 0.0f;
                }

                Debug.Log("InteroceptiveAgent: OnEpisodeBegin");
        }

        public override void CollectObservations(VectorSensor sensor)
        {
                sensor.AddObservation(resourceLevels);
                if (useOlfactoryObs)
                {
                        OlfactorySensingRangeVisualize();
                        sensor.AddObservation(olfactoryObservation);
                }
                if (useThermalObs)
                {
                        sensor.AddObservation(thermoObservation);
                }
                if (useCollisionObs)
                {
                        sensor.AddObservation(collisionObservation);
                }
                if (useTouchObs)
                {
                        sensor.AddObservation(touchObservation);
                }
                sensor.AddObservation(agentPosition);
                sensor.AddObservation(agentRotation);
        }

        //브레인(정책)으로 부터 전달 받은 행동을 실행하는 메소드
        public override void OnActionReceived(ActionBuffers actions)
        {
                if (playRecorder.GetComponent<CaptureScreenShot>().recordEnable)
                {
                        playRecorder.GetComponent<CaptureScreenShot>().CaptureImage();
                }

                this.agentPosition = this.transform.position;
                this.agentRotation = this.transform.eulerAngles;

                if (eatenResource)
                {
                        if (eatenResourceTag.ToLower() == "food")
                        {
                                foodCoefficient.change_5 = 1.0f;
                        }
                        if (eatenResourceTag.ToLower() == "water" || eatenResourceTag.ToLower() == "pond")
                        {
                                waterCoefficient.change_5 = 1.0f;
                        }

                        if (singleTrial)
                        {
                                EndEpisode();
                        }
                }

                // EV (Food, Water, Thermo) Update
                FoodUpdate(foodCoefficient.change_0, foodCoefficient.change_1, foodCoefficient.change_2, foodCoefficient.change_3, foodCoefficient.change_4, foodCoefficient.change_5);
                WaterUpdate(waterCoefficient.change_0, waterCoefficient.change_1, waterCoefficient.change_2, waterCoefficient.change_3, waterCoefficient.change_4, waterCoefficient.change_5);
                // HealthUpdate(changeHealth_0, changeHealth_1, changeHealth_2, changeHealth_3, changeHealth_4, changeHealth_5);

                // Olfactory Observation
                if (this.useOlfactoryObs)
                {
                        OlfactoryObserving();
                }

                if (this.useThermalObs)
                {
                        // ThermalChanging();
                        ThermalObserving();
                        ThermoUpdate(thermoCoefficient.change_0, thermoCoefficient.change_1, thermoCoefficient.change_2, thermoCoefficient.change_3, thermoCoefficient.change_4);
                }

                if (this.useCollisionObs)
                {
                        CollisionObserving(); 
                }

                if (this.useTouchObs)
                {
                        TouchObserving();
                }

                bool checkFoodLevel = (this.foodLevelRange.max < this.resourceLevels[0] || this.resourceLevels[0] < this.foodLevelRange.min);
                bool checkWaterLevel = (this.waterLevelRange.max < this.resourceLevels[1] || this.resourceLevels[1] < this.waterLevelRange.min);
                bool checkThermoLevel = false;
                if (this.useThermalObs)
                {
                        checkThermoLevel = (this.thermoLevelRange.max < this.bodyTemp || this.bodyTemp < this.thermoLevelRange.min);
                }

                bool checkHealth = (this.resourceLevels[3] < this.healthLevelRange.min);

                if (checkFoodLevel || checkWaterLevel || checkThermoLevel || checkHealth)
                        EndEpisode();

                if (this.resourceLevels[3] > healthLevelRange.max)
                {
                        this.resourceLevels[3] = healthLevelRange.max;
                }

                int action = actions.DiscreteActions[0];
                MoveAgent(action);

                // Calculate reward
                currentReward = CalculateReward();
                // Track recent rewards for moving average
                if (recentRewards.Count >= rewardWindowSize)
                {
                        recentRewards.Dequeue(); // Remove oldest reward
                }
                recentRewards.Enqueue(currentReward);
                // Update average reward
                averageReward = recentRewards.Average();
                // Apply the reward to ML-Agent
                AddReward(currentReward);

                // Reset eating state as default
                eatenResource = false;
                eatenResourceTag = "none";
                foodCoefficient.change_5 = 0.0f;
                waterCoefficient.change_5 = 0.0f;

                for (int i = 0; i < this.countEV; i++)
                {
                        oldResourceLevels[i] = resourceLevels[i];
                }

        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
                var discreteActionsOut = actionsOut.DiscreteActions;
                discreteActionsOut[0] = 0;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                        discreteActionsOut[0] = 1;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                        discreteActionsOut[0] = 2;
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                        discreteActionsOut[0] = 3;
                }
                if (Input.GetKey(KeyCode.Space))
                {
                        discreteActionsOut[0] = 4;
                }
        }

        public void MoveAgent(int action)
        {
                var dirToGo = Vector3.zero;
                var rotateDir = Vector3.zero;

                // Get the action index for movement
                // int action = Mathf.FloorToInt(act[0]);
                /*** Action Category
                 * 0 : None 
                 * 1 : Forward  
                 * 2 : Left
                 * 3 : Right
                 * 4 : Eat
                 * ***/

                this.isAgentActionEat = false;
                switch (action)
                {
                        case 0:
                                m_AgentRb.velocity = Vector3.zero;
                                break;
                        case 1:
                                dirToGo = transform.forward;
                                m_AgentRb.velocity = dirToGo * moveSpeed;
                                break;
                        case 2:
                                transform.Rotate(-transform.up, Time.fixedDeltaTime * turnSpeed);
                                m_AgentRb.velocity = Vector3.zero;
                                break;
                        case 3:
                                transform.Rotate(transform.up, Time.fixedDeltaTime * turnSpeed);
                                m_AgentRb.velocity = Vector3.zero;
                                break;
                        case 4:
                                this.isAgentActionEat = true;
                                m_AgentRb.velocity = Vector3.zero;
                                break;
                }
        }

        private void OlfactoryObserving()
        {
                for (int i = 0; i < olfactoryFeatureSize; i++)
                {
                        olfactoryObservation[i] = 0;
                }

                // agent 크기 바뀌면 z값 확인하기
                Vector3 SpherePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.5f);
                // Detect layer number 8
                Collider[] olfactoryTargets = Physics.OverlapSphere(SpherePos, olfactorySensorLength, 1 << 8);

                int j = 0;
                foreach (Collider other in olfactoryTargets)
                {
                        ResourceProperty resource = other.gameObject.GetComponent<ResourceProperty>();
                        if (resource.CompareTag("pond") || resource.CompareTag("water") || resource.CompareTag("food"))
                        {
                                j += 1;
                                float resourceDistance = Vector3.Distance(SpherePos, resource.transform.position);
                                for (int i = 0; i < olfactoryFeatureSize; i++)
                                {
                                        olfactoryObservation[i] += resource.ResourceP[i] * (1 / resourceDistance);
                                }
                        }
                }
        }

        private void OlfactorySensingRangeVisualize()
        {
                GameObject olfactorySensingRange = this.transform.Find("SensingRange").gameObject;
                Vector3 newScale = new Vector3(this.olfactorySensorLength, this.olfactorySensorLength, this.olfactorySensorLength);
                olfactorySensingRange.transform.localScale = newScale;
        }

        public float[] ThermalObserving()
        {
                if (relativeThermalObs)
                {
                        thermoObservation[0] = thermoSensorForward.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[1] = thermoSensorBackward.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[2] = thermoSensorLeft.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[3] = thermoSensorRight.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[4] = thermoSensorForwardLeft.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[5] = thermoSensorForwardRight.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[6] = thermoSensorBackwardLeft.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                        thermoObservation[7] = thermoSensorBackwardRight.GetComponent<ThermalSensing>().GetThermalSense() - this.resourceLevels[2];
                }
                else
                {
                        thermoObservation[0] = thermoSensorForward.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[1] = thermoSensorBackward.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[2] = thermoSensorLeft.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[3] = thermoSensorRight.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[4] = thermoSensorForwardLeft.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[5] = thermoSensorForwardRight.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[6] = thermoSensorBackwardLeft.GetComponent<ThermalSensing>().GetThermalSense();
                        thermoObservation[7] = thermoSensorBackwardRight.GetComponent<ThermalSensing>().GetThermalSense();
                }
                return thermoObservation;
        }


        public void CollisionObserving()
        {
                objectRaycast = GetComponent<ObjectRaycast>();

                collisionObservation[0] = objectRaycast.collisionObservation[0];
                collisionObservation[1] = objectRaycast.collisionObservation[1];
                collisionObservation[2] = objectRaycast.collisionObservation[2];
                collisionObservation[3] = objectRaycast.collisionObservation[3];
                collisionObservation[4] = objectRaycast.collisionObservation[4];
                collisionObservation[5] = objectRaycast.collisionObservation[5];
                collisionObservation[6] = objectRaycast.collisionObservation[6];
                collisionObservation[7] = objectRaycast.collisionObservation[7];
                collisionObservation[8] = objectRaycast.collisionObservation[8];
                collisionObservation[9] = objectRaycast.collisionObservation[9];
                
                // this.resourceLevels[3] -= objectRaycast.damage;
        }
        public void TouchObserving()
        {
                if (isTouched)
                {
                        touchObservation = 0.0f;
                }
                else
                {
                        touchObservation = 0.0f;
                }
        }

        // EV 간 상호작용을 고려한 업데이트
        public void FoodUpdate(float changeFood_0, float changeFood_1, float changeFood_2, float changeFood_3, float changeFood_4, float changeFood_5)
        {
                this.resourceLevels[0] = this.resourceLevels[0] +
                                        changeFood_0 * foodLevelRange.max * Time.fixedDeltaTime +
                                        changeFood_1 * (this.oldResourceLevels[0] + 15) * Time.fixedDeltaTime +
                                        changeFood_2 * (this.oldResourceLevels[1] + 15) * Time.fixedDeltaTime +
                                        changeFood_3 * (this.oldResourceLevels[2] + 15) * Time.fixedDeltaTime +
                                        changeFood_4 * (CalculateInteraction(oldResourceLevels[0], oldResourceLevels[1], oldResourceLevels[2])) * Time.fixedDeltaTime +
                                        changeFood_5 * resourceFoodValue;
        }

        public void WaterUpdate(float changeWater_0, float changeWater_1, float changeWater_2, float changeWater_3, float changeWater_4, float changeWater_5)
        {
                this.resourceLevels[1] = this.resourceLevels[1] +
                                        changeWater_0 * waterLevelRange.max * Time.fixedDeltaTime +
                                        changeWater_1 * (this.oldResourceLevels[0] + 15) * Time.fixedDeltaTime +
                                        changeWater_2 * (this.oldResourceLevels[1] + 15) * Time.fixedDeltaTime +
                                        changeWater_3 * (this.oldResourceLevels[2] + 15) * Time.fixedDeltaTime +
                                        changeWater_4 * (CalculateInteraction(oldResourceLevels[0], oldResourceLevels[1], oldResourceLevels[2])) * Time.fixedDeltaTime +
                                        changeWater_5 * resourceWaterValue;
        }

        public void ThermoUpdate(float changeBody_0, float changeBody_1, float changeBody_2, float changeBody_3, float changeBody_4)
        {
                float surroundTemp = 0.0f;
                for (int i = 0; i < thermoObservation.Length; i++)
                {
                        if (relativeThermalObs)
                        { surroundTemp += thermoObservation[i]; }
                        else
                        { surroundTemp += thermoObservation[i] - this.resourceLevels[2]; }

                }

                bodyTemp = this.bodyTemp +
                            changeBody_0 * surroundTemp * Time.fixedDeltaTime +
                            changeBody_1 * (this.oldResourceLevels[0] + 15) * Time.fixedDeltaTime +
                            changeBody_2 * (this.oldResourceLevels[1] + 15) * Time.fixedDeltaTime +
                            changeBody_3 * (this.oldResourceLevels[2] + 15) * Time.fixedDeltaTime +
                            changeBody_4 * (CalculateInteraction(oldResourceLevels[0], oldResourceLevels[1], oldResourceLevels[2])) * Time.fixedDeltaTime;
                this.resourceLevels[2] = this.bodyTemp;
        }

        public void HealthUpdate(float changeHealth_0, float changeHealth_1, float changeHealth_2, float changeHealth_3, float changeHealth_4, float changeHealth_5)
        {
                this.resourceLevels[3] = this.resourceLevels[3] +
                                        changeHealth_0 * healthLevelRange.max * Time.fixedDeltaTime +
                                        changeHealth_1 * (this.oldResourceLevels[0] + 15) * Time.fixedDeltaTime +
                                        changeHealth_2 * (this.oldResourceLevels[1] + 15) * Time.fixedDeltaTime +
                                        changeHealth_3 * (this.oldResourceLevels[2] + 15) * Time.fixedDeltaTime +
                                        changeHealth_4 * (CalculateInteraction(oldResourceLevels[0], oldResourceLevels[1], oldResourceLevels[2])) * Time.fixedDeltaTime +
                                        changeHealth_5 * resourceWaterValue;
        }

        public float CalculateInteraction(float food, float water, float bodyTemp)
        {
                return 1.0f;
        }

        private float CalculateReward()
        {
                float[] setPoints = new float[] { 0f, 0f, 0f, 0f };
                setPoints[0] = startFoodLevel;
                setPoints[1] = startWaterLevel;
                setPoints[2] = startThermoLevel;
                setPoints[3] = startHealthLevel;

                float[] maxDeviations = new float[] { startFoodLevel - foodLevelRange.min, 
                                                startWaterLevel - waterLevelRange.min, 
                                                startThermoLevel - thermoLevelRange.min, 
                                                healthLevelRange.max - healthLevelRange.min };

                // Calculate normalized Euclidean distance
                float sumSquares = 0f;
                for (int i = 0; i < resourceLevels.Length; i++)
                {
                        float deviation = resourceLevels[i] - setPoints[i];
                        float normalizedDeviation = deviation / maxDeviations[i];
                        sumSquares += normalizedDeviation * normalizedDeviation;
                }

                float reward = Mathf.Sqrt(sumSquares);

                // Invert reward (higher distance = lower reward)
                return -reward;
        }

        public void SetResetParameters()
        {
                isAIControlled = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("isAIControlled", System.Convert.ToSingle(isAIControlled)));
                singleTrial = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("singleTrial", System.Convert.ToSingle(singleTrial)));
                initRandomAgentPosition = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("initRandomAgentPosition", System.Convert.ToSingle(initRandomAgentPosition)));

                moveSpeed = m_ResetParams.GetWithDefault("moveSpeed", moveSpeed);
                turnSpeed = m_ResetParams.GetWithDefault("turnSpeed", turnSpeed);
                autoEat = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("autoEat", System.Convert.ToSingle(autoEat)));
                eatingDistance = m_ResetParams.GetWithDefault("eatingDistance", eatingDistance);

                countEV = System.Convert.ToInt32(m_ResetParams.GetWithDefault("countEV", countEV));

                resourceFoodValue = m_ResetParams.GetWithDefault("resourceFoodValue", resourceFoodValue);
                startFoodLevel = m_ResetParams.GetWithDefault("startFoodLevel", startFoodLevel);

                resourceWaterValue = m_ResetParams.GetWithDefault("resourceWaterValue", resourceWaterValue);
                startWaterLevel = m_ResetParams.GetWithDefault("startWaterLevel", startWaterLevel);

                useTouchObs = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("useTouchObs", System.Convert.ToSingle(useTouchObs)));
                useCollisionObs = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("useCollisionObs", System.Convert.ToSingle(useCollisionObs)));
                useOlfactoryObs = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("useOlfactoryObs", System.Convert.ToSingle(useOlfactoryObs)));
                olfactorySensorLength = m_ResetParams.GetWithDefault("olfactorySensorLength", olfactorySensorLength);

                useThermalObs = System.Convert.ToBoolean(m_ResetParams.GetWithDefault("useThermalObs", System.Convert.ToSingle(useThermalObs)));
                startThermoLevel = m_ResetParams.GetWithDefault("startThermoLevel", startThermoLevel);

                startHealthLevel = m_ResetParams.GetWithDefault("startHealthLevel", startHealthLevel);
                raysPerDirection = m_ResetParams.GetWithDefault("raysPerDirection", raysPerDirection);
                maxDistance = m_ResetParams.GetWithDefault("maxDistance", maxDistance);
                radialRange = m_ResetParams.GetWithDefault("radialRange", radialRange);
                damageConstant = m_ResetParams.GetWithDefault("damageConstant", damageConstant);
        }
}
