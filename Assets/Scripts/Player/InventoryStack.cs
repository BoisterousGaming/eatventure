using System.Collections.Generic;
using UnityEngine;

public class InventoryStack : MonoBehaviour, IInventory
{
    [SerializeField] int maxBeans = 3;
    [SerializeField] Transform backAnchor;
    [SerializeField] Vector3 perItemOffset = new(0, 0.22f, 0);

    readonly List<Transform> _beans = new();
    CoffeeCup _cup;

    public bool CanAddBean => _beans.Count < maxBeans;

    public bool PushBean(BeanBag b)
    {
        if (!CanAddBean || b == null) return false;
        b.SetCarried(true, backAnchor);
        b.transform.localPosition = perItemOffset * _beans.Count;
        _beans.Add(b.transform);
        return true;
    }

    public bool PopBean(out BeanBag b)
    {
        b = null;
        if (_beans.Count == 0) return false;
        Transform top = _beans[^1]; _beans.RemoveAt(_beans.Count - 1);
        b = top.GetComponent<BeanBag>();
        b.SetCarried(false, null);
        return true;
    }

    public bool HasCup => _cup != null;
    public bool PushCup(CoffeeCup c)
    {
        if (_cup != null || c == null) return false;
        _cup = c; c.SetCarried(true, backAnchor);
        c.transform.localPosition = new Vector3(0, 0.9f, 0.2f);
        return true;
    }
    public bool PopCup(out CoffeeCup c)
    {
        c = _cup; if (c == null) return false;
        _cup.SetCarried(false, null); _cup = null; return true;
    }
}
