using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;

    [Header("Win Condition")]
    public int targetKills = 40;
    public int currentKills = 0;

    [Tooltip("Optional: drag the ZombieSpawn here to auto-read totalToSpawn")]
    public ZombieSpawn spawner;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe to sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (spawner) targetKills = spawner.totalToSpawn;
    }

    public void RegisterKill()
    {
        currentKills++;
        Debug.Log($"[WinManager] Kills: {currentKills}/{targetKills}");

        if (currentKills >= targetKills)
        {
            SceneManager.LoadScene("Victoryyyy"); // safer by name
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Chuong test")
        {
            // Reset values every time Chuong test reloads
            currentKills = 0;
            if (spawner) targetKills = spawner.totalToSpawn;
        }
    }
}
