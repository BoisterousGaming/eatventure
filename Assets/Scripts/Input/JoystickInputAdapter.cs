using UnityEngine;

public class JoystickInputAdapter : MonoBehaviour, IPlayerInput
{
    [SerializeField] MonoBehaviour joystick;
    [SerializeField] KeyCode interactKey = KeyCode.Space;

    public Vector2 Move
    {
        get
        {
            if (joystick == null) return Vector2.zero;
            var t = joystick.GetType();
            float h = (float)(t.GetProperty("Horizontal")?.GetValue(joystick) ?? 0f);
            float v = (float)(t.GetProperty("Vertical")?.GetValue(joystick) ?? 0f);
            Vector2 m = new(h, v);
            return m.sqrMagnitude > 1 ? m.normalized : m;
        }
    }

    public bool InteractDown => Input.GetKeyDown(interactKey);
}
