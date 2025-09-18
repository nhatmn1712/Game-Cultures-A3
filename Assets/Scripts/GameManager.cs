using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Sun")]
    [SerializeField] int startingSun = 4;      // change default here
    [SerializeField] TextMeshProUGUI sunText;  // optional drag, will auto-find if empty
    public int SunPoints { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // prevent duplicates
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SunPoints = startingSun;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // make sure the game isn’t paused after a retry
        Time.timeScale = 1f;

        // re-acquire the SunText from the freshly loaded scene if needed
        if (sunText == null)
        {
            // Option A: by tag (recommended)
            // Ensure your TMP text object is tagged "SunText"
            var tagged = GameObject.FindGameObjectWithTag("SunText");
            if (tagged) sunText = tagged.GetComponent<TextMeshProUGUI>();

            // Option B: fallback by name search if tag not set
            if (sunText == null)
            {
                foreach (var tmp in FindObjectsOfType<TextMeshProUGUI>(true))
                {
                    if (tmp.name == "SunText")
                    {
                        sunText = tmp;
                        break;
                    }
                }
            }
        }

        UpdateSunUI();
    }

    // ------- API used by other scripts -------
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
    // ----------------------------------------

    void UpdateSunUI()
    {
        if (sunText) sunText.text = SunPoints.ToString();
    }

    // Optional: allow a UI script to register explicitly
    public void RegisterSunText(TextMeshProUGUI t)
    {
        sunText = t;
        UpdateSunUI();
    }
}
