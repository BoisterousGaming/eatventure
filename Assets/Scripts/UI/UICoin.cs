using UnityEngine;

public class UICoin : MonoBehaviour, IPoolable
{
    RectTransform _rt;
    Transform _defaultParent;
    Vector3 _initialLocalScale;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _defaultParent = transform.parent;
        _initialLocalScale = transform.localScale;
    }

    public void OnSpawnFromPool()
    {
        transform.SetParent(_defaultParent, false);
        transform.localScale = _initialLocalScale;
        _rt.anchoredPosition3D = Vector3.zero;
        _rt.localRotation = Quaternion.identity;
    }

    public void OnReturnToPool()
    {
        transform.SetParent(_defaultParent, false);
        transform.localScale = _initialLocalScale;
        _rt.anchoredPosition3D = Vector3.zero;
        _rt.localRotation = Quaternion.identity;
    }
}
