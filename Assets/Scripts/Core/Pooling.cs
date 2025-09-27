using UnityEngine;

public interface IPoolable
{
    void OnSpawnFromPool();
    void OnReturnToPool();
}

public interface IObjectPool<T> where T : Component
{
    T Get();
    void Release(T instance);
}

public interface IPoolService
{
    T Get<T>(T prefab) where T : Component;
    void Release<T>(T instance) where T : Component;
}
