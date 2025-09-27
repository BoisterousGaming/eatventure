using UnityEngine;

public class PlayerMotor : MonoBehaviour, IPlayerMover
{
    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float rotationLerp = 12f;

    public float Speed { get => moveSpeed; set => moveSpeed = value; }

    public void Move(Vector3 worldDir)
    {
        if (worldDir.sqrMagnitude < 0.0001f) return;
        transform.Translate(worldDir.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Face(Vector3 worldDir)
    {
        if (worldDir.sqrMagnitude < 0.0001f) return;
        var target = Quaternion.LookRotation(new Vector3(worldDir.x, 0, worldDir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationLerp * Time.deltaTime);
    }
}
