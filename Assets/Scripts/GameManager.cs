using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Sun System")]
    public int sunCount = 5;                 // default starting sun
    public TextMeshProUGUI sunText;          // UI reference

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Rebind UI after each scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Called every time a scene finishes loading
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset sun count to 5 when a new scene is loaded
        sunCount = 5;

        // Try to find the new SunText object in the new scene
        if (sunText == null)
        {
            GameObject textObj = GameObject.FindWithTag("SunText");
            if (textObj != null)
                sunText = textObj.GetComponent<TextMeshProUGUI>();
        }

        UpdateSunUI();
    }

    public void AddSun(int amount)
    {
        sunCount += amount;
        UpdateSunUI();
    }

    public void SpendSun(int amount)
    {
        sunCount -= amount;
        UpdateSunUI();
    }

    public bool CanAfford(int price)
    {
        return sunCount >= price;
    }

    void UpdateSunUI()
    {
        if (sunText != null)
            sunText.text = sunCount.ToString();
    }
}
