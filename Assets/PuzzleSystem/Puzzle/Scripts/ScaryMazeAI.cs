using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScaryMazeAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();
    private int currentPatrolIndex = 0;

    private void Start()
    {
        if (patrolPoints.Count > 0)
        {
            MoveToNextPatrolPoint();
        }
        //agent.updatePosition = false;
        agent.updateRotation = true;
    }

    private void Update()
    {
        FaceTarget();
        //transform.LookAt(agent.steeringTarget);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            MoveToNextPatrolPoint();
        }
        
    }
    void FaceTarget()
    {
        // Create a new position for the target that ignores the Y axis
        Vector3 targetPosition = new Vector3(agent.steeringTarget.x, transform.position.y, agent.steeringTarget.z);
        Vector3 direction = targetPosition - transform.position;

        // Check if the direction vector has a non-zero length
        if (direction.sqrMagnitude > 0.001f) // Use a small threshold to avoid precision errors
        {
            // Calculate the rotation needed to look at the target position
            Quaternion rot = Quaternion.LookRotation(direction.normalized);

            // Apply rotation with smoothing
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * agent.angularSpeed);
        }
    }
    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Count == 0) return;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScaryMazePuzzleManager.Instance.SendToNewPoint(other.transform);
        }
    }
}
