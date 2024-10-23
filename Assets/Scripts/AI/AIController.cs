using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
[RequireComponent(typeof(CapsuleCollider),typeof(NavMeshAgent),typeof(AudioSource))]
public class AIController : MonoBehaviour
{
    private Vector3 playerDir;
    private Vector3 playerPos;
    public Vector3 PlayerDirection => playerDir;
    [SerializeField] NavMeshAgent agent;
    Vector3 startingPos;
    [SerializeField] Animator anim;
    [SerializeField] int animSpeedTrans;
    float stoppingDistanceOrig = .2f;
    bool isRoaming;
    bool playerInRange;
    bool isEnemyChasing = false;
    bool isScared = false;
    bool isRandSFX = false;
    [SerializeField] float attackDist = .5f;
    [SerializeField] float normSpeed = 1f;
    [SerializeField] float chaseSpeed = 2f;
    Coroutine someCo;
    Coroutine scaredCo;
    [SerializeField] int roamDist = 15;
    [SerializeField] int roamTimer = 1;
    [SerializeField] int faceTargetSpeed = 1 ;
    [SerializeField] Vector3 roomCenter = Vector3.zero;
    [SerializeField] Vector3 roomSize = new Vector3 (10, 0, 10);

    [SerializeField] Suspect suspect;
    [SerializeField] Item item;
    private AudioSource audioSource;

    // Start is called before the first frame update

    private void Awake()
    {
        suspect ??= GetComponent<Suspect>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Ambiant NPC Error");
        }
        else
        {
            audioSource.playOnAwake = false;
            audioSource.outputAudioMixerGroup = audioManager.instance.GetSFXAudioMixer();
            audioSource.spatialBlend = 1f;
        }
    }
    void Start()
    {
        stoppingDistanceOrig = agent.stoppingDistance;
        startingPos = transform.position;
        agent.speed = normSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Blend", agent.velocity.normalized.magnitude);
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Blend");
        

        playerPos = GameObject.FindWithTag("Player").transform.position;
        playerDir = playerPos - transform.position;
        chasePlayer();
        if (playerInRange)
        {
            faceTarget();
        }
        if (!playerInRange && !isRoaming && agent.remainingDistance < 0.05f && someCo == null && !isEnemyChasing)
            someCo = StartCoroutine(roam());

        if (!isRandSFX)
        {
            StartCoroutine(RandomSound());
        }
    }

    private void chasePlayer()
    {
         if(suspect.IsKiller && GameManager.instance.Day == 7)
         {
            if (!item.gameObject.activeSelf)
                item.gameObject.SetActive(true);
             isEnemyChasing = true;
             setSpeed(chaseSpeed);
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
        setSpeed(normSpeed);
        Vector3 randomPos = Random.insideUnitSphere * roamDist + startingPos;
        //Vector3 randomPos = getRandomRoomPos();
        //randomPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
        while(agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        //if(isScared)
        //    anim.SetBool("Scared", true);
        //yield return new WaitForSeconds(roamTimer);
        //if (isScared)
        //    anim.SetBool("Scared", false);
        anim.SetFloat("Blend", 0);
        yield return new WaitForSeconds(Random.Range(2f, 3f));
        isRoaming = false;
        someCo = null;
    }

    private Vector3 getRandomRoomPos()
    {
        float xPos = Random.Range(roomCenter.x - roomSize.x / 2, roomCenter.x + roomSize.x / 2);
        float zPos = Random.Range(roomCenter.z - roomSize.z / 2, roomCenter.z + roomSize.z / 2);
        return new Vector3(xPos, transform.position.y, zPos);
    }

    void scaredState()
    {
        isScared = true;
    }

    //void OnEnable()
    //{
    //    GameManager.onNPCDeath += scaredState;
    //    GameManager.instance.NPCDied();
    //}

    //void OnDisable()
    //{
    //    GameManager.onNPCDeath -= scaredState;
    //}

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

    private void setSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    public void BeginningSlash()
    {
        item.BodyCol.enabled = true;
    }

    public void EndOfSlash()
    {
        item.BodyCol.enabled = false;

    }

    private IEnumerator RandomSound()   //Plays a random sound in random intervals
    {
        isRandSFX = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(20, 50));
        audioSource.PlayOneShot(audioManager.instance.ambiantNPCSFX[UnityEngine.Random.Range(0, audioManager.instance.ambiantNPCSFX.Length)], audioManager.instance.ambiantNPCVol);

        isRandSFX = false;
    }
}
   