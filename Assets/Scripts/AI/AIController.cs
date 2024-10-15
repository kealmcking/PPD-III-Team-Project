using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(CapsuleCollider),typeof(NavMeshAgent))]
public class AIController : MonoBehaviour
{
    private Vector3 playerDir;
    private Vector3 playerPos;
    public Vector3 PlayerDirection => playerDir;
    [SerializeField] NavMeshAgent agent;
    Vector3 startingPos;
    [SerializeField] Animator anim;
    [SerializeField] int animSpeedTrans;
    float stoppingDistanceOrig;
    bool isRoaming;
    bool playerInRange;
    bool isEnemyChasing = false;
    [SerializeField] float attackDist = .5f;
    Coroutine someCo;
    [SerializeField] int roamDist = 15;
    [SerializeField] int roamTimer = 1;
    [SerializeField] int faceTargetSpeed = 1 ;
    [SerializeField] Suspect suspect;

    // Start is called before the first frame update

    private void Awake()
    {
        suspect ??= GetComponent<Suspect>();
    }
    void Start()
    {
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Blend", agent.velocity.normalized.magnitude);
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Blend");
        

        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerDir = playerPos - transform.position;
       // chasePlayer();
        if (playerInRange)
        {
            faceTarget();
        }
        if (!playerInRange && !isRoaming && agent.remainingDistance < 0.05f && someCo == null && !isEnemyChasing)
            someCo = StartCoroutine(roam());

    }

    private void chasePlayer()
    {
        if(suspect.IsKiller && GameManager.instance.Day == 7)
        {
            isEnemyChasing = true;
            agent.SetDestination(playerPos);
            if(Vector3.Distance(transform.position, playerPos) <= attackDist)
            {
                anim.SetTrigger("Attack");
            }
        }
        
        
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
   