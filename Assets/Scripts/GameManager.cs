using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sun")]
    [SerializeField] private int startingSun = 4;          // << set your start here
    [SerializeField] private TextMeshProUGUI sunText;

    public int SunPoints { get; private set; }

    void Awake()
    {
        // Singleton (no duplicates)
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        // Make sure the game is unfrozen whenever this scene loads
        Time.timeScale = 1f;

        // Initialize sun & UI
        SunPoints = startingSun;
        UpdateSunUI();
    }

    public bool CanAfford(int cost) => SunPoints >= cost;

    public void SpendSun(int cost)
    {
        SunPoints -= cost;
        UpdateSunUI();
    }

    public void AddSun(int amount)
    {
        SunPoints += amount;
        UpdateSunUI();
    }

    private void UpdateSunUI()
    {
        if (sunText != null) sunText.text = SunPoints.ToString();
    }
}

