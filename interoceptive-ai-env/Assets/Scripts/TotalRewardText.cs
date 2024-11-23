using UnityEngine;
using TMPro;

public class TotalRewardDisplay : MonoBehaviour
{
        [Header("Agent Reference")]
        [Tooltip("Reference to the agent's InteroceptiveAgent script.")]
        public InteroceptiveAgent agentState;

        [Header("UI Components")]
        [Tooltip("Reference to the Total Reward Text (TextMeshProUGUI).")]
        public TextMeshProUGUI totalRewardText;

        void Update()
        {
                if (agentState == null || totalRewardText == null)
                {
                        Debug.LogWarning("AgentState or TotalRewardText is not assigned.");
                        return;
                }

                // Fetch rewards from the agent
                float mlAgentReward = agentState.currentReward;
                float averagedReward = agentState.averageReward;
                float rewardWindowSize = agentState.rewardWindowSize;
                // Display the reward
                totalRewardText.text = $"\n\nReward: {mlAgentReward:F2}\nAvg Last {rewardWindowSize:F0} Steps: \n{averagedReward:F2}";

                // Optional: Change text color based on reward value
                if (mlAgentReward >= -0.5f) // Example threshold
                        totalRewardText.color = Color.green;
                else if (mlAgentReward >= -1f)
                        totalRewardText.color = Color.yellow;
                else
                        totalRewardText.color = Color.red;
        }
}
