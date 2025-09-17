using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;

    [Header("Win Condition")]
    public int targetKills = 40;      // set this to 40, or auto from spawner
    public int currentKills = 0;

    [Tooltip("Optional: drag the ZombieSpawn here to auto-read totalToSpawn")]
    public ZombieSpawn spawner;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        if (spawner) targetKills = spawner.totalToSpawn;   // auto = number you plan to spawn
    }

    public void RegisterKill()
    {
        currentKills++;
        Debug.Log($"[WinManager] Kills: {currentKills}/{targetKills}");

        if (currentKills >= targetKills)
        {
            // Safer by name if your build order ever changes:
            // SceneManager.LoadScene("Victoryyyy");
            SceneManager.LoadScene(5);
        }
    }
}
