using UnityEngine;

public class PlayerContext : MonoBehaviour
{
    public IInventory Inventory { get; private set; }
    public IScoreService Score { get; private set; }
    public ICurrencyFX CurrencyFX { get; private set; }
    public RectTransform ScoreTarget;

    void Awake()
    {
        Inventory = GetComponent<IInventory>();
        Score = FindFirstObjectByType<ScoreService>();
        CurrencyFX = FindFirstObjectByType<CurrencyFlyFX>();
    }
}
