using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Used to gather and mark spawn points for differentiating what can use the spawn points
/// </summary>
public class SpawnPoint : MonoBehaviour
{
  //  [Tooltip("Used to determine the max distance to the navmesh each spawner should be in order for the spawn points to be moved to the navmesh")
  //  ,SerializeField]
//    float maxDistanceToNavMesh = 7f;
    [SerializeField] float sphereRadius = .5f;
    [SerializeField] SpawnPointType type;
    public SpawnPointType Type => type;
    public void Awake()
    {
       // MoveSpawnPointToNavMesh();
    }
    /// <summary>
    /// Sets the spawn points of each spawner to the navmesh useful for setting the AI directly on the navmesh
    /// </summary>
    /// <param name="spawnPoints"></param>
 /*   public void MoveSpawnPointToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, maxDistanceToNavMesh, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }        
    }*/


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(type == SpawnPointType.Ghost)
        Gizmos.color = Color.white;
        if(type == SpawnPointType.Suspect)
            Gizmos.color = Color.green;
        if (type == SpawnPointType.Killer)
            Gizmos.color = Color.red;
        if (type == SpawnPointType.Clue)
            Gizmos.color = Color.yellow;
        if (type == SpawnPointType.Starting)
            Gizmos.color = Color.blue;
        if (type == SpawnPointType.Puzzle)
            Gizmos.color = Color.magenta;
        if (type == SpawnPointType.Interactable)
            Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, sphereRadius);
    }
#endif

}
public enum SpawnPointType
{
    None,
    Starting,
    Ghost,
    Suspect,
    Killer,
    Interactable,
    Puzzle,
    Clue,
}