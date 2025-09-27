using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerDriver : MonoBehaviour
{
    IPlayerInput _input;
    IPlayerMover _mover;

    void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _mover = GetComponent<IPlayerMover>();
    }

    void Update()
    {
        if (_input == null || _mover == null) return;

        Vector2 mv = _input.Move;
        Vector3 dir = new Vector3(mv.x, 0, mv.y);

        _mover.Face(dir);
        _mover.Move(dir);
    }
}
