using TMPro;
using UnityEngine;

public class ScoreService : MonoBehaviour, IScoreService
{
    [SerializeField] TextMeshProUGUI scoreText;
    public int Coins { get; private set; }

    public void Add(int amount)
    {
        Coins += amount;
        if (scoreText) scoreText.text = $"$ {Coins}";
    }
}
