using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Sun")]
    public int sunPoints = 50;               // starting sun
    public TextMeshProUGUI sunText;          // drag your UI text here

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        UpdateSunUI();
    }

    // --- Methods PlantSlot expects ---
    public bool CanAfford(int cost) => sunPoints >= cost;

    public void SpendSun(int cost)
    {
        sunPoints -= cost;
        UpdateSunUI();
    }

    public void AddSun(int amount)
    {
        sunPoints += amount;
        UpdateSunUI();
    }
    // ---------------------------------

    private void UpdateSunUI()
    {
        if (sunText) sunText.text = sunPoints.ToString();
    }
}
