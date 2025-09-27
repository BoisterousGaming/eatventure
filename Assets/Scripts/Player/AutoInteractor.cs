using System.Collections.Generic;
using UnityEngine;

public class AutoInteractor : MonoBehaviour
{
    [Header("Scan")]
    [SerializeField] float radius = 1.2f;
    [SerializeField] LayerMask interactableMask = ~0;
    [SerializeField] float attemptInterval = 0.25f;
    [SerializeField] int maxTargetsPerSweep = 3;

    [Header("Debounce")]
    [SerializeField] float successCooldown = 0.15f;
    [SerializeField] float failCooldown = 0.25f;

    PlayerContext _ctx;
    float _nextSweepTime;

    readonly Dictionary<MonoBehaviour, float> _targetNextTime = new();

    void Awake()
    {
        _ctx = GetComponent<PlayerContext>();
    }

    void Update()
    {
        if (Time.time < _nextSweepTime || _ctx == null) return;
        _nextSweepTime = Time.time + attemptInterval;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius, interactableMask);
        if (hits.Length == 0) return;
        
        var candidates = new List<(MonoBehaviour mb, IInteractable inter, int prio, float distSq)>(hits.Length);

        foreach (var h in hits)
        {
            if (!h.TryGetComponent<IInteractable>(out var inter)) continue;
            var mb = inter as MonoBehaviour; if (mb == null) continue;

            int prio = 0;
            if (inter is IWeightedInteractable w)
                prio = w.GetPriority(_ctx);

            float d2 = (h.transform.position - transform.position).sqrMagnitude;
            if (_targetNextTime.TryGetValue(mb, out float readyAt) && Time.time < readyAt) continue;

            candidates.Add((mb, inter, prio, d2));
        }

        candidates.Sort((a, b) =>
        {
            int p = b.prio.CompareTo(a.prio);
            return p != 0 ? p : a.distSq.CompareTo(b.distSq);
        });

        int tried = 0;
        foreach (var c in candidates)
        {
            if (tried >= maxTargetsPerSweep) break;

            bool ok = c.inter.TryInteract(_ctx);
            tried++;

            _targetNextTime[c.mb] = Time.time + (ok ? successCooldown : failCooldown);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 1f, 0.4f, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
}
