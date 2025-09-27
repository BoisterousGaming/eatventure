using UnityEngine;

public class KeyboardInput : MonoBehaviour, IPlayerInput
{
    public Vector2 Move => new Vector2(
        Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical")
    ).normalized;

    public bool InteractDown => Input.GetKeyDown(KeyCode.E);
}
