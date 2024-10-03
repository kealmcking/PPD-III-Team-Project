using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(CapsuleCollider),typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    private Vector3 playerDir;
    public Vector3 PlayerDirection => playerDir;
    [SerializeField] NavMeshAgent agent;
    Vector3 startingPos;
    float stoppingDistanceOrig;
    bool isRoaming;
    bool playerInRange;
    Coroutine someCo;
    [SerializeField] int roamDist = 15;
    [SerializeField] int roamTimer = 1;
    [SerializeField] int faceTargetSpeed = 1 ;

    // Start is called before the first frame update
    void Start()
    {
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerDir = GameObject.FindWithTag("Player").transform.position - transform.position;
        if (playerInRange)
        {
            faceTarget();
        }
        if (!playerInRange && !isRoaming && agent.remainingDistance < 0.05f && someCo == null)
            someCo = StartCoroutine(roam());
    }

    IEnumerator roam()
    {
        isRoaming = true;
        Vector3 randomPos = Random.insideUnitSphere * roamDist;
        randomPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
        while(agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        yield return new WaitForSeconds(roamTimer);

        isRoaming = false;
        someCo = null;
    }

    void faceTarget()
    {
        playerDir.y = 0;
        Quaternion rot = Quaternion.LookRotation(playerDir.normalized, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (someCo != null)
            {
                StopCoroutine(someCo);
                someCo = null;
                isRoaming = false;
            }
            playerInRange = true;
            agent.SetDestination(transform.position);
            Debug.Log("Trigger Enter" + playerInRange + "   " + other.gameObject.tag.ToString());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Trigger Exit" + playerInRange + "   " + other.gameObject.tag.ToString());
        }
    }

}
   