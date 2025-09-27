using UnityEngine;
using System;

public class CustomerInteractable : MonoBehaviour, IInteractable, ICustomerLifecycle, IPoolable
{
    [SerializeField] int reward = 5;

    public Action OnServed { get; set; }
    IPoolService _pool;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();

    public bool TryInteract(PlayerContext ctx)
    {
        if (ctx == null || ctx.Inventory == null) return false;
        if (!ctx.Inventory.HasCup) return false;

        if (ctx.Inventory.PopCup(out var cup))
        {
            _pool?.Release(cup);

            ctx.CurrencyFX?.FlyFromWorld(transform.position + Vector3.up * 1.2f, reward, ctx.ScoreTarget);
            ctx.Score?.Add(reward);

            Served();
            return true;
        }
        return false;
    }

    public void Served()
    {
        OnServed?.Invoke();
        _pool?.Release(this);
    }

    public void OnSpawnFromPool()
    {
        OnServed = null;
        gameObject.SetActive(true);
    }

    public void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
