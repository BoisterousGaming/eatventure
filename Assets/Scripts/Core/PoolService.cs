using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolService : MonoBehaviour, IPoolService
{
    [Serializable]
    public class Prewarm
    {
        public GameObject prefab;
        public int count = 0;
    }

    [SerializeField] List<Prewarm> prewarm = new();

    readonly Dictionary<GameObject, Queue<GameObject>> _pools = new();

    Transform _root;

    void Awake()
    {
        _root = new GameObject("PoolRoot").transform;
        _root.SetParent(transform, false);

        foreach (var p in prewarm)
        {
            if (!p.prefab || p.count <= 0) continue;
            if (!_pools.TryGetValue(p.prefab, out var q))
                _pools[p.prefab] = q = new Queue<GameObject>();

            for (int i = 0; i < p.count; i++)
            {
                var inst = CreateInstance(p.prefab);
                SetActive(inst, false);
                q.Enqueue(inst);
            }
        }
    }

    public T Get<T>(T prefabComponent) where T : Component
    {
        if (prefabComponent == null)
        {
            Debug.LogError("PoolService: Null prefab requested");
            return null;
        }

        GameObject key = prefabComponent.gameObject;
        GameObject instanceGO = DequeueOrCreate(key);
        SetActive(instanceGO, true);

        foreach (var p in instanceGO.GetComponentsInChildren<IPoolable>(true))
            p.OnSpawnFromPool();

        var comp = instanceGO.GetComponentInChildren<T>(true);
        if (comp == null)
            Debug.LogError($"PoolService: Instance missing component {typeof(T).Name} on {key.name}");

        return comp;
    }

    public void Release<T>(T instanceComponent) where T : Component
    {
        if (instanceComponent == null) return;

        var id = instanceComponent.GetComponentInParent<PoolIdentity>();
        if (id == null || id.prefabKey == null)
        {
            Debug.LogWarning($"PoolService: {instanceComponent.name} has no PoolIdentity; destroying to avoid leak.");
            Destroy(instanceComponent.gameObject);
            return;
        }

        GameObject go = id.gameObject;
        foreach (var p in go.GetComponentsInChildren<IPoolable>(true))
            p.OnReturnToPool();

        go.transform.SetParent(_root, false);
        SetActive(go, false);

        if (!_pools.TryGetValue(id.prefabKey, out var q))
            _pools[id.prefabKey] = q = new Queue<GameObject>();

        q.Enqueue(go);
    }

    GameObject DequeueOrCreate(GameObject key)
    {
        if (!_pools.TryGetValue(key, out var q))
            _pools[key] = q = new Queue<GameObject>();

        if (q.Count > 0)
            return q.Dequeue();

        return CreateInstance(key);
    }

    GameObject CreateInstance(GameObject key)
    {
        var go = Instantiate(key, _root);
        var id = go.GetComponent<PoolIdentity>();
        if (id == null) id = go.AddComponent<PoolIdentity>();
        id.prefabKey = key;
        return go;
    }

    static void SetActive(GameObject go, bool state) => go.SetActive(state);
}
