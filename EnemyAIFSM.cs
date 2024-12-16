using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIFSM : MonoBehaviour
{
    public Transform player;
    public float normalSpeed = 3.5f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float fieldOfView = 90f;
    public float rotationSpeed = 10f; 

    private NavMeshAgent agent;
    private bool playerInSight = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        agent.speed = normalSpeed;
        agent.updateRotation = false; 
    }

    void Update()
    {
        if (player == null) return;

        playerInSight = IsPlayerInSight();
        agent.speed = playerInSight ? chaseSpeed : normalSpeed;
        agent.SetDestination(player.position);

        UpdateAnimatorSpeed();
        RotateMonster();
    }

    void RotateMonster()
    {
        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            Vector3 direction = agent.velocity.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    void UpdateAnimatorSpeed()
    {
        if (animator != null)
        {
            float speedPercent = agent.velocity.magnitude / agent.speed;
            animator.SetFloat("Speed", speedPercent);
        }
    }

    bool IsPlayerInSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= detectionRange)
        {
            float angle = Vector3.Angle(transform.forward, directionToPlayer);
            if (angle <= fieldOfView * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange))
                {
                    if (hit.transform == player)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Vector3 fovLine1 = Quaternion.AngleAxis(fieldOfView * 0.5f, transform.up) * transform.forward * detectionRange;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fieldOfView * 0.5f, transform.up) * transform.forward * detectionRange;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);
    }
}











