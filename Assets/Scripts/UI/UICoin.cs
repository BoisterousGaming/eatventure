using UnityEngine;

public class UICoin : MonoBehaviour, IPoolable
{
    RectTransform _rt;
    Transform _defaultParent;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _defaultParent = transform.parent;
    }

    public void OnSpawnFromPool()
    {
        transform.SetParent(_defaultParent);
        _rt.anchoredPosition3D = Vector3.zero;
        _rt.localScale = Vector3.one;
        _rt.rotation = Quaternion.identity;
    }

    public void OnReturnToPool()
    {
        transform.SetParent(_defaultParent);
        _rt.anchoredPosition3D = Vector3.zero;
        _rt.localScale = Vector3.one;
        _rt.rotation = Quaternion.identity;
    }
}
