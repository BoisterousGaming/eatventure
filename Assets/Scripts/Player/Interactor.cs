using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] float radius = 1.2f;
    IPlayerInput _input;
    PlayerContext _ctx;

    void Awake()
    {
        _input = GetComponent<IPlayerInput>();
        _ctx = GetComponent<PlayerContext>();
    }

    void Update()
    {
        if (_input == null || !_input.InteractDown) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        foreach (var h in hits)
        {
            if (h.TryGetComponent<IInteractable>(out var target))
            {
                if (target.TryInteract(_ctx)) break;
            }
        }
    }
}
