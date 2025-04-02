using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Predator : MonoBehaviour
{
    public enum PredatorState { Resting, Searching, Chasing, Attacking }
    public PredatorState currentState;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float restDuration = 5f;
    [SerializeField] private float totalChaseTime;

    private Vector3 destination;
    private float currentChaseTime;

    private bool isWalking;

    [Header("Field of View Settings")]
    [SerializeField] private float viewAngle = 120f; // Field of view angle
    [SerializeField] private float viewDistance = 10f; // Field of view distance
    [SerializeField] private LayerMask targetMask; // Layer mask for detecting the agent
    private Transform detectedAgent;

    [Header("Damage Settings")]
    [SerializeField] private float damageAmount = 1f; // Damage to apply to the agent
    [SerializeField] private float maxDamage = 5f; // Maximum damage cap
    [SerializeField] private float attackInterval = 1f; // Time between damage applications

    [Header("Attack State Settings")]
    [SerializeField] private float attackStateLockDuration = 2f; // Minimum time to stay in Attacking state

    private bool isAttackStateLocked = false; // Prevents immediate state switching

    [Header("Components")]
    [SerializeField] private Animator anim;
    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        ChangeState(PredatorState.Searching); // Start in the Searching state
    }

    void Update()
    {
        switch (currentState)
        {
            case PredatorState.Resting:
                // Do nothing, waiting for the rest duration to end
                break;

            case PredatorState.Searching:
                if (View())
                {
                    Debug.Log("Agent detected! Switching to Chasing state."); // Debug message for detection
                    ChangeState(PredatorState.Chasing);
                }
                break;

            case PredatorState.Chasing:
                if (!View())
                {
                    Debug.Log("Agent lost! Switching to Searching state."); // Debug message for losing the agent
                    ChangeState(PredatorState.Searching);
                }
                break;

            case PredatorState.Attacking:
                // Attacking logic is handled in the coroutine
                break;
        }
    }

    private void ChangeState(PredatorState newState)
    {
        StopAllCoroutines();
        currentState = newState;

        switch (newState)
        {
            case PredatorState.Resting:
                StartCoroutine(RestCoroutine());
                break;

            case PredatorState.Searching:
                StartCoroutine(SearchCoroutine());
                break;

            case PredatorState.Chasing:
                StartCoroutine(ChaseCoroutine());
                break;

            case PredatorState.Attacking:
                StopMovement(); // Stop the predator's movement
                StartCoroutine(AttackCoroutine());
                break;
        }
    }

    private void StopMovement()
    {
        isWalking = false;
        anim.SetBool("Walking", isWalking);
        nav.ResetPath(); // Stop the NavMeshAgent from moving
        nav.isStopped = true; // Disable movement
        Debug.Log("Movement stopped!"); // Debug message for stopping movement
    }

    private void ResumeMovement()
    {
        nav.isStopped = false; // Re-enable movement
    }

    private IEnumerator RestCoroutine()
    {
        isWalking = false;
        anim.SetBool("Walking", isWalking);
        nav.ResetPath();

        yield return new WaitForSeconds(restDuration);

        ChangeState(PredatorState.Searching);
    }

    private IEnumerator SearchCoroutine()
    {
        while (currentState == PredatorState.Searching)
        {
            RandomAction();
            yield return new WaitForSeconds(Random.Range(1f, 2f)); // Wait for a random duration before the next action
        }
    }

    private IEnumerator ChaseCoroutine()
    {
        currentChaseTime = 0f;
        isWalking = true;
        anim.SetBool("Walking", isWalking);

        while (currentState == PredatorState.Chasing && currentChaseTime < totalChaseTime)
        {
            if (detectedAgent != null && nav.enabled)
            {
                nav.speed = walkSpeed;
                nav.SetDestination(detectedAgent.position);
            }
            currentChaseTime += Time.deltaTime;
            yield return null;
        }

        if (currentChaseTime >= totalChaseTime)
        {
            Debug.Log("Chase time expired! Switching to Searching state."); // Debug message for chase timeout
            ChangeState(PredatorState.Searching);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        Debug.Log("Attacking the agent!"); // Debug message for attacking

        // Lock the state for a minimum duration
        isAttackStateLocked = true;
        float lockEndTime = Time.time + attackStateLockDuration;

        while (currentState == PredatorState.Attacking)
        {
            // Check if the lock duration has expired
            if (Time.time >= lockEndTime)
            {
                isAttackStateLocked = false;
            }

            // If the agent is no longer detected, switch states
            if (detectedAgent == null && !isAttackStateLocked)
            {
                Debug.Log("Agent moved out of attack area! Switching to Searching state."); // Debug message for losing the agent
                ResumeMovement(); // Re-enable movement
                ChangeState(PredatorState.Searching);
                yield break;
            }

            // Apply damage to the agent
            InteroceptiveAgent agentScript = detectedAgent.GetComponent<InteroceptiveAgent>();
            if (agentScript != null)
            {
                float damage = Mathf.Min(damageAmount, maxDamage); // Cap the damage
                agentScript.resourceLevels[3] -= damage; // Apply damage to the agent's health
                Debug.Log($"Applied {damage} damage to the agent.");
            }

            yield return new WaitForSeconds(attackInterval);
        }
    }

    private void RandomAction()
    {
        int random = Random.Range(0, 2);
        if (random == 0)
        {
            Wait();
        }
        else
        {
            TryWalk();
        }
    }

    private void TryWalk()
    {
        isWalking = true;
        anim.SetBool("Walking", isWalking);
        destination.Set(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)); // Random destination within a range
        if (nav.enabled)
        {
            nav.speed = walkSpeed;
            nav.SetDestination(transform.position + destination);
        }
    }

    private void Wait()
    {
        isWalking = false;
        anim.SetBool("Walking", isWalking);
        if (nav.enabled)
        {
            nav.ResetPath();
        }
    }

    // Field of View Logic
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