using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CoffeeCup : MonoBehaviour, IPoolable
{
    Rigidbody _rb;
    Transform _defaultParent;
    Vector3 _initialLocalScale;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _defaultParent = transform.parent;
        _initialLocalScale = transform.localScale;
    }

    public void SetCarried(bool on, Transform parent)
    {
        if (on)
        {
            transform.SetParent(parent, false);
            transform.localScale = _initialLocalScale;
            transform.localRotation = Quaternion.identity;
            if (_rb) _rb.isKinematic = true;
        }
        else
        {
            transform.SetParent(_defaultParent, false);
            transform.localScale = _initialLocalScale;
            if (_rb) _rb.isKinematic = false;
        }
    }

    public void OnSpawnFromPool()
    {
        if (_rb)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }
        transform.localScale = _initialLocalScale;
        transform.localRotation = Quaternion.identity;
        transform.SetParent(_defaultParent, false);
    }

    public void OnReturnToPool()
    {
        transform.SetParent(_defaultParent, false);
        transform.localScale = _initialLocalScale;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        if (_rb)
            _rb.linearVelocity = Vector3.zero; _rb.angularVelocity = Vector3.zero;
    }
}
