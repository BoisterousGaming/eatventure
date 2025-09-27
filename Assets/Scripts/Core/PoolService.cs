using System.Collections.Generic;
using UnityEngine;

public class PoolService : MonoBehaviour, IPoolService
{
    [System.Serializable]
    class PoolConfig
    {
        public Component prefab;
        public int prewarm = 0;
    }

    [SerializeField] List<PoolConfig> prewarmConfigs = new();

    readonly Dictionary<Component, Queue<Component>> _pools = new();
    readonly Dictionary<Component, Component> _instanceToPrefab = new();

    Transform _root;

    void Awake()
    {
        _root = new GameObject("PoolRoot").transform;
        _root.SetParent(transform, false);

        foreach (var cfg in prewarmConfigs)
        {
            if (cfg.prefab == null || cfg.prewarm <= 0) continue;
            if (!_pools.TryGetValue(cfg.prefab, out var q))
                _pools[cfg.prefab] = q = new Queue<Component>();

            for (int i = 0; i < cfg.prewarm; i++)
            {
                var inst = Instantiate(cfg.prefab, _root);
                RegisterInstance(inst, cfg.prefab);
                SetActive(inst, false);
                q.Enqueue(inst);
            }
        }
    }

    public T Get<T>(T prefab) where T : Component
    {
        if (prefab == null)
        {
            Debug.LogError("PoolService: Null prefab requested");
            return null;
        }

        if (!_pools.TryGetValue(prefab, out var q))
            _pools[prefab] = q = new Queue<Component>();

        T inst;
        if (q.Count > 0)
        {
            inst = (T)q.Dequeue();
        }
        else
        {
            inst = Instantiate(prefab, _root) as T;
            RegisterInstance(inst, prefab);
        }

        SetActive(inst, true);
        (inst as IPoolable)?.OnSpawnFromPool();
        return inst;
    }

    public void Release<T>(T instance) where T : Component
    {
        if (instance == null) return;

        if (!_instanceToPrefab.TryGetValue(instance, out var prefab))
        {
            Debug.LogWarning($"PoolService: Instance {instance.name} not registered; destroying to avoid leak.");
            Destroy(instance.gameObject);
            return;
        }

        (instance as IPoolable)?.OnReturnToPool();
        instance.transform.SetParent(_root, false);
        SetActive(instance, false);
        _pools[prefab].Enqueue(instance);
    }

    void RegisterInstance(Component inst, Component prefab)
    {
        _instanceToPrefab[inst] = prefab;
    }

    static void SetActive(Component c, bool state) => c.gameObject.SetActive(state);
}
