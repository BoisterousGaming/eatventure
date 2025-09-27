using System.Collections;
using UnityEngine;

public class CoffeeMachineInteractable : MonoBehaviour, IInteractable, ICoffeeProcessor
{
    [SerializeField] Transform counterPoint;
    [SerializeField] float processSeconds = 2f;
    [SerializeField] CoffeeCup cupPrefab;

    bool _processing;
    IPoolService _pool;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();

    public bool CanInsertBean => !_processing;

    public bool TryInteract(PlayerContext ctx)
    {
        if (ctx == null || _pool == null) return false;

        if (CanInsertBean && ctx.Inventory.PopBean(out var bean))
        {
            _pool.Release(bean);
            InsertBean();
            return true;
        }

        if (!_processing && TryTakeCup(out var cup))
        {
            return ctx.Inventory.PushCup(cup);
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
        yield return new WaitForSeconds(processSeconds);

        var cup = _pool.Get(cupPrefab);
        cup.transform.SetPositionAndRotation(counterPoint.position, counterPoint.rotation);

        _processing = false;
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
