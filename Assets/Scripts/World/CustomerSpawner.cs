using UnityEngine;

public class CustomerSpawner : MonoBehaviour, ICustomerFactory
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] CustomerInteractable customerPrefab;

    IPoolService _pool;
    CustomerInteractable _current;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();
    void Start() => Spawn();

    public CustomerInteractable Spawn()
    {
        var c = _pool.Get(customerPrefab);
        c.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        c.OnServed = () =>
        {
            _current = null;
            Spawn();
        };

        _current = c;
        return c;
    }

    void OnDisable()
    {
        if (_current != null && _pool != null)
        {
            _current.OnServed = null;
            _pool.Release(_current);
            _current = null;
        }
    }
}
