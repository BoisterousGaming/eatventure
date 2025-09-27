using UnityEngine;

public class ResourceAreaInteractable : MonoBehaviour, IWeightedInteractable, IResourceProvider
{
    [SerializeField] BeanBag beanPrefab;

    IPoolService _pool;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();

    public int GetPriority(PlayerContext ctx)
    {
        return (ctx != null && ctx.Inventory.CanAddBean) ? 50 : 0;
    }

    public bool TryInteract(PlayerContext ctx)
    {
        if (ctx?.Inventory == null || _pool == null) return false;
        if (!ctx.Inventory.CanAddBean) return false;

        var bean = CreateBean(transform.position + Vector3.up * 0.8f);
        return ctx.Inventory.PushBean(bean);
    }

    public BeanBag CreateBean(Vector3 hintPos)
    {
        var bean = _pool.Get(beanPrefab);
        bean.transform.position = hintPos;
        bean.transform.rotation = Quaternion.identity;
        return bean;
    }
}
