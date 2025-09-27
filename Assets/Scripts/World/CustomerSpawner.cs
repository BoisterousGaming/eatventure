using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour, ICustomerFactory
{
    [Header("Spawn Settings")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] CustomerInteractable customerPrefab;
    [SerializeField] float minDelay = 1.5f;
    [SerializeField] float maxDelay = 2f;

    IPoolService _pool;
    CustomerInteractable _current;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();
    void Start() => Spawn();

    public CustomerInteractable Spawn()
    {
        var c = _pool.Get(customerPrefab);
        c.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        c.OnServed = () => StartCoroutine(SpawnNextWithDelay());
        _current = c;
        return c;
    }

    IEnumerator SpawnNextWithDelay()
    {
        float wait = Random.Range(minDelay, maxDelay);
        yield return new WaitForSeconds(wait);
        Spawn();
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
