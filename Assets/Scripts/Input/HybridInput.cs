using UnityEngine;
using UnityEngine.InputSystem;

public class HybridInput : MonoBehaviour, IPlayerInput
{
    [SerializeField] VirtualJoystick joystick;
    [SerializeField] float keyboardDeadzone = 0.1f;

    public Vector2 Move
    {
        get
        {
            if (joystick != null)
            {
                var j = joystick.Value;
                if (j.sqrMagnitude > 0.001f) return j;
            }

            var kb = Keyboard.current;
            if (kb == null) return Vector2.zero;

            float x = 0f, y = 0f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) x -= 1f;
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) x += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed) y -= 1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed) y += 1f;

            Vector2 k = new Vector2(x, y);
            return k.magnitude < keyboardDeadzone ? Vector2.zero : Vector2.ClampMagnitude(k, 1f);
        }
    }

    public bool InteractDown => false;
}
