using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Predator : MonoBehaviour
{
    public enum PredatorState { Resting, Searching, Chasing, Attacking }
    public PredatorState currentState;

    [Header("Movement Settings")]
    [SerializeField] public float walkSpeed;
    [SerializeField] public float restDuration = 5f;
    [SerializeField] public float totalChaseTime;

    private Vector3 destination;
    private float currentChaseTime;

    private bool isWalking;

    [Header("Field of View Settings")]
    [SerializeField] public float viewAngle = 120f; // Field of view angle
    [SerializeField] public float viewDistance = 10f; // Field of view distance
    [SerializeField] private LayerMask targetMask; // Layer mask for detecting the agent
    private Transform detectedAgent;

    [Header("Damage Settings")]
    [SerializeField] public float damageAmount = 1f; // Damage to apply to the agent
    [SerializeField] public float maxDamage = 5f; // Maximum damage cap
    [SerializeField] public float attackInterval = 1f; // Time between damage applications

    [Header("Attack State Settings")]
    [SerializeField] public float attackStateLockDuration = 2f; // Minimum time to stay in Attacking state

    private bool isAttackStateLocked = false; // Prevents immediate state switching

    [Header("Components")]
    [SerializeField] private Animator anim;
    private NavMeshAgent nav;

    // Add step counters for Resting and Searching states
    private int restingStepCounter = 0; // Counter for steps in Resting state
    private int searchingStepCounter = 0; // Counter for steps in Searching state
    [SerializeField] public int maxRestingSteps = 50; // Maximum steps to stay in Resting state
    [SerializeField] public int maxSearchingSteps = 100; // Maximum steps to stay in Searching state

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        ChangeState(PredatorState.Searching); // Start in the Searching state
    }

    // This method will be called explicitly to take one action
    public void TakeAction()
    {
        switch (currentState)
        {
            case PredatorState.Resting:
                // Increment the resting step counter
                restingStepCounter++;

                // Check if an agent is in view
                if (View())
                {
                    ChangeState(PredatorState.Chasing);
                }
                else if (restingStepCounter >= maxRestingSteps)
                {
                    // Transition to Searching state after max resting steps
                    restingStepCounter = 0; // Reset the counter
                    ChangeState(PredatorState.Searching);
                }
                break;

            case PredatorState.Searching:
                // Increment the searching step counter
                searchingStepCounter++;

                // Perform random actions at intervals
                if (searchingStepCounter % 20 == 0) // Perform random action every 20 steps
                {
                    RandomAction();
                }

                // Check if an agent is in view
                if (View())
                {
                    ChangeState(PredatorState.Chasing);
                }
                else if (searchingStepCounter >= maxSearchingSteps)
                {
                    // Transition to Resting state after max searching steps
                    searchingStepCounter = 0; // Reset the counter
                    ChangeState(PredatorState.Resting);
                }
                break;

            case PredatorState.Chasing:
                if (detectedAgent != null && nav.enabled)
                {
                    nav.speed = walkSpeed;
                    nav.SetDestination(detectedAgent.position);

                    // If the agent is no longer in view, transition to Searching
                    if (!View())
                    {
                        Debug.Log("Agent moved out of view! Switching to Searching state."); // Log message
                        detectedAgent = null;
                        ChangeState(PredatorState.Searching);
                    }
                }
                else
                {
                    ChangeState(PredatorState.Searching);
                }
                break;

            case PredatorState.Attacking:
                if (detectedAgent != null)
                {
                    ApplyDamage();

                    // If the agent is no longer in view, transition to Searching
                    if (!View())
                    {
                        Debug.Log("Agent moved out of view during attack! Switching to Searching state."); // Log message
                        detectedAgent = null;
                        ChangeState(PredatorState.Searching);
                    }
                }
                else
                {
                    ChangeState(PredatorState.Searching);
                }
                break;
        }
    }

    private void ApplyDamage()
    {
        InteroceptiveAgent agentScript = detectedAgent.GetComponent<InteroceptiveAgent>();
        if (agentScript != null)
        {
            float damage = Mathf.Min(damageAmount, maxDamage); // Cap the damage
            agentScript.resourceLevels[3] -= damage; // Apply damage to the agent's health
        }
    }

    private void ChangeState(PredatorState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case PredatorState.Resting:
                StopMovement();
                restingStepCounter = 0; // Reset the resting counter
                break;

            case PredatorState.Searching:
                ResumeMovement();
                searchingStepCounter = 0; // Reset the searching counter
                break;

            case PredatorState.Chasing:
                ResumeMovement();
                break;

            case PredatorState.Attacking:
                StopMovement();
                break;
        }
    }

    private void StopMovement()
    {
        isWalking = false;
        anim.SetBool("Walking", isWalking);
        nav.ResetPath();
        nav.isStopped = true;
    }

    private void ResumeMovement()
    {
        nav.isStopped = false;
    }

    private IEnumerator RestCoroutine()
    {
        yield return new WaitForSeconds(restDuration);
        ChangeState(PredatorState.Searching);
    }

    private void RandomAction()
    {
        int random = Random.Range(0, 2); // Randomly decide to stop or walk
        if (random == 0)
        {
            Debug.Log("RandomAction: Stopping movement.");
            StopMovement();
        }
        else
        {
            Debug.Log("RandomAction: Trying to walk.");
            TryWalk();
        }
    }

    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        // Set a random destination within a certain range
        destination.Set(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
        Debug.Log($"TryWalk: Setting destination to {destination}");

        if (nav.enabled)
        {
            nav.speed = walkSpeed;
            nav.SetDestination(transform.position + destination);

            if (!nav.hasPath)
            {
                Debug.LogWarning("TryWalk: NavMeshAgent has no path. Check NavMesh or destination.");
            }
        }
        else
        {
            Debug.LogError("TryWalk: NavMeshAgent is not enabled.");
        }
    }

    private bool View()
    {
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        foreach (Collider target in targetsInView)
        {
            Transform targetTransform = target.transform;

            if (targetTransform.CompareTag("player"))
            {
                Vector3 directionToTarget = (targetTransform.position - transform.position).normalized;
                float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

                if (angleToTarget < viewAngle * 0.5f)
                {
                    detectedAgent = targetTransform;
                    return true;
                }
            }
        }

        detectedAgent = null;
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Use Box Collider for the initial collision to change to Attacking state
        if (collision.collider is BoxCollider && collision.gameObject.CompareTag("player"))
        {
            Debug.Log("Box Collider triggered collision! Switching to Attacking state."); // Debug message for collision
            detectedAgent = collision.transform;
            ChangeState(PredatorState.Attacking);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Use Box Collider to detect when the agent moves out of collision
        if (collision.collider is BoxCollider && collision.gameObject.CompareTag("player"))
        {
            Debug.Log("Agent moved out of Box Collider! Switching to Searching state."); // Debug message for collision exit
            detectedAgent = null;
            ResumeMovement(); // Re-enable movement
            ChangeState(PredatorState.Searching);
        }
    }
}