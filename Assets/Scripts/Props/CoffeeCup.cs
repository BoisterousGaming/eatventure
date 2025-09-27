using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoffeeCup : MonoBehaviour, IPoolable
{
    Rigidbody _rb;
    Transform _defaultParent;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _defaultParent = transform.parent;
    }

    public void SetCarried(bool on, Transform parent)
    {
        transform.SetParent(on ? parent : _defaultParent);
        if (_rb) _rb.isKinematic = on;
        if (on) transform.localRotation = Quaternion.identity;
    }

    public void OnSpawnFromPool()
    {
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
        transform.SetParent(_defaultParent);
    }

    public void OnReturnToPool()
    {
        SetCarried(false, _defaultParent);
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }
}
