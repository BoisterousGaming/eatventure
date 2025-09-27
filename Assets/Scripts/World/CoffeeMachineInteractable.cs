using System.Collections;
using UnityEngine;

public class CoffeeMachineInteractable : MonoBehaviour, IWeightedInteractable, ICoffeeProcessor
{
    [SerializeField] Transform counterPoint;
    [SerializeField] float processSeconds = 2f;
    [SerializeField] CoffeeCup cupPrefab;

    public bool IsProcessing { get; private set; }
    public float Progress01 { get; private set; }
    public bool CanInsertBean => !_processing;

    bool _processing;
    IPoolService _pool;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();

    public int GetPriority(PlayerContext ctx)
    {
        if (TryTakeCup(out _) && !ctx.Inventory.HasCup) return 100;

        var invRead = ctx.Inventory as IBeanReadable;
        if (!_processing && invRead != null && invRead.BeanCount > 0) return 80;

        return 0;
    }

    public bool TryInteract(PlayerContext ctx)
    {
        if (ctx == null || _pool == null) return false;

        if (TryTakeCup(out var cup) && !ctx.Inventory.HasCup)
            return ctx.Inventory.PushCup(cup);

        var invRead = ctx.Inventory as IBeanReadable;
        if (!_processing && invRead != null && invRead.BeanCount > 0 && ctx.Inventory.PopBean(out var bean))
        {
            _pool.Release(bean);
            InsertBean();
            return true;
        }

        return false;
    }

    public void InsertBean()
    {
        if (_processing) return;
        StartCoroutine(ProcessOneCup());
    }

    IEnumerator ProcessOneCup()
    {
        _processing = true;
        IsProcessing = true;
        Progress01 = 0f;

        float t = 0f;
        while (t < processSeconds)
        {
            t += Time.deltaTime;
            Progress01 = Mathf.Clamp01(t / processSeconds);
            yield return null;
        }

        var cup = _pool.Get(cupPrefab);
        cup.transform.SetPositionAndRotation(counterPoint.position, counterPoint.rotation);

        _processing = false;
        IsProcessing = false;
        Progress01 = 0f;
    }

    public bool TryTakeCup(out CoffeeCup cup)
    {
        cup = null;
        Collider[] hits = Physics.OverlapSphere(counterPoint.position, 0.3f);
        foreach (var h in hits)
            if (h.TryGetComponent(out CoffeeCup c) && c.gameObject.activeInHierarchy) { cup = c; return true; }
        return false;
    }
}
