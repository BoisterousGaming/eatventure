using UnityEngine;

public class BoxAreaLimiter : MonoBehaviour, IPositionLimiter
{
    [Header("XZ Bounds (world space)")]
    [SerializeField] float minX = -5f;
    [SerializeField] float maxX = 5f;
    [SerializeField] float minZ = -5f;
    [SerializeField] float maxZ = 5f;

    public Vector3 Clamp(Vector3 worldPos)
    {
        worldPos.x = Mathf.Clamp(worldPos.x, minX, maxX);
        worldPos.z = Mathf.Clamp(worldPos.z, minZ, maxZ);
        return worldPos;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 0.7f, 1f, 0.2f);
        Vector3 center = new((minX + maxX) * 0.5f, 0, (minZ + maxZ) * 0.5f);
        Vector3 size = new(Mathf.Abs(maxX - minX), 0.05f, Mathf.Abs(maxZ - minZ));
        Gizmos.DrawCube(center, size);
        Gizmos.color = new Color(0, 0.7f, 1f, 1f);
        Gizmos.DrawWireCube(center, size);
    }
#endif
}