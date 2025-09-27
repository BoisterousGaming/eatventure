using System.Collections;
using UnityEngine;

public class CurrencyFlyFX : MonoBehaviour, ICurrencyFX
{
    [SerializeField] RectTransform canvasRect;
    [SerializeField] UICoin coinPrefab;
    [SerializeField] float travel = 0.6f;

    IPoolService _pool;

    void Awake() => _pool = FindFirstObjectByType<PoolService>();

    public void FlyFromWorld(Vector3 worldPos, int amount, RectTransform target)
    {
        if (!target || _pool == null || coinPrefab == null) return;
        int n = Mathf.Clamp(amount, 1, 12);
        for (int i = 0; i < n; i++) StartCoroutine(Fly(worldPos, target));
    }

    IEnumerator Fly(Vector3 worldPos, RectTransform target)
    {
        var cam = Camera.main;
        if (!cam) yield break;

        UICoin coin = _pool.Get(coinPrefab);
        var rt = coin.GetComponent<RectTransform>();
        rt.SetParent(canvasRect, false);
        rt.position = cam.WorldToScreenPoint(worldPos);

        float t = 0f;
        Vector3 from = rt.position, to = target.position;

        while (t < travel)
        {
            t += Time.deltaTime;
            float k = t / travel;
            rt.position = Vector3.Lerp(from, to, k * k * k);
            yield return null;
        }

        _pool.Release(coin);
    }
}
