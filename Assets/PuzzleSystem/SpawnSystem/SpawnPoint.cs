using UnityEngine;
/// <summary>
/// Used to gather and mark spawn points for differentiating what can use the spawn points
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] float sphereRadius = .5f;
    [SerializeField] SpawnPointType type;
    public SpawnPointType Type => type;
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
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
}