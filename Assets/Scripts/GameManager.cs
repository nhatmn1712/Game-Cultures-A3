using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Sun")]
    [Min(0)]
    public int sunPoints = 5;                       // starting sun
    [SerializeField] private TextMeshProUGUI sunText; // drag your TMP text here

    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        UpdateSunUI();
    }

    // --- used by PlantSlot / others ---
    public bool CanAfford(int cost) => sunPoints >= cost;

    public void SpendSun(int cost)
    {
        if (cost <= 0) return;
        sunPoints = Mathf.Max(0, sunPoints - cost);
        UpdateSunUI();
    }

    public void AddSun(int amount)
    {
        if (amount <= 0) return;
        sunPoints += amount;
        UpdateSunUI();
    }

    // optional helper if you want to assign the label from code
    public void SetSunText(TextMeshProUGUI t)
    {
        sunText = t;
        UpdateSunUI();
    }

    private void UpdateSunUI()
    {
        if (sunText) sunText.text = sunPoints.ToString();
    }
}
