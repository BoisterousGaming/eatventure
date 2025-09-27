using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Refs")]
    [SerializeField] RectTransform knob;
    [SerializeField] RectTransform area;

    [Header("Settings")]
    [SerializeField] float radius = 80f;
    [SerializeField] bool snapToCenterOnRelease = true;
    [SerializeField] bool autoHideWhenIdle = false;

    public Vector2 Value { get; private set; }

    RectTransform _rt;
    Canvas _canvas;
    Camera _uiCam;
    bool _active;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        area = area ? area : _rt;
        _canvas = GetComponentInParent<Canvas>();
        _uiCam = _canvas && _canvas.renderMode != RenderMode.ScreenSpaceOverlay ? _canvas.worldCamera : null;

        if (autoHideWhenIdle) SetVisible(false);
        CenterKnob();
    }

    public void OnPointerDown(PointerEventData e)
    {
        _active = true;
        if (autoHideWhenIdle) SetVisible(true);
        OnDrag(e);
    }

    public void OnDrag(PointerEventData e)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(area, e.position, _uiCam, out localPoint);

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        knob.anchoredPosition = clamped;

        Value = clamped / radius;
        Value = Value.sqrMagnitude > 1 ? Value.normalized : Value;
    }

    public void OnPointerUp(PointerEventData e)
    {
        _active = false;
        Value = Vector2.zero;
        if (snapToCenterOnRelease) CenterKnob();
        if (autoHideWhenIdle) SetVisible(false);
    }

    void CenterKnob() { if (knob) knob.anchoredPosition = Vector2.zero; }
    void SetVisible(bool v) { if (area) area.gameObject.SetActive(v); }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        if (_active) return;
        var mouse = Mouse.current;
        if (mouse == null) return;

        if (mouse.leftButton.isPressed)
        {
            Vector2 pos = mouse.position.ReadValue();
            if (RectTransformUtility.RectangleContainsScreenPoint(area, pos, _uiCam))
            {
                var ped = new PointerEventData(EventSystem.current) { position = pos };
                OnPointerDown(ped);
            }
        }
#endif
    }
}
