using System.Collections;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [Header("Setup")]
    public Transform[] spawnpoints;
    public GameObject zombiePrefab;   // Enemy type 1
    public GameObject zombiePrefab2;  // Enemy type 2

    [Header("Spawning")]
    public float initialDelay = 0.5f;
    public Vector2 spawnIntervalRange = new Vector2(2f, 4f);

    [Header("Limits")]
    public int totalToSpawn = 20;

    private Coroutine spawnLoopCo;

    private void Start()
    {
        Debug.Log($"[ZombieSpawn] Start | lanes={spawnpoints?.Length ?? 0} | totalToSpawn={totalToSpawn}");
        spawnLoopCo = StartCoroutine(SpawnLoop());
    }

    private void OnDisable()
    {
        if (spawnLoopCo != null)
        {
            StopCoroutine(spawnLoopCo);
            spawnLoopCo = null;
        }
    }

    private IEnumerator SpawnLoop()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < totalToSpawn; i++)
        {
            SpawnRandomZombie();

            if (i < totalToSpawn - 1)
            {
                float delay = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
                Debug.Log($"[ZombieSpawn] Next spawn in {delay:0.00}s ({i + 1}/{totalToSpawn})");
                yield return new WaitForSeconds(delay);
            }
        }

        Debug.Log($"[ZombieSpawn] Reached limit ({totalToSpawn}). Spawning stopped.");
        spawnLoopCo = null;
    }

    private void SpawnRandomZombie()
    {
        if (spawnpoints == null || spawnpoints.Length == 0)
        {
            Debug.LogWarning("[ZombieSpawn] Spawner not set up (missing lanes).");
            return;
        }

        int r = Random.Range(0, spawnpoints.Length);
        Transform point = spawnpoints[r];

        // Pick randomly between zombiePrefab (type 1) and zombiePrefab2 (type 2)
        GameObject prefabToSpawn = (Random.value < 0.5f) ? zombiePrefab : zombiePrefab2;

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("[ZombieSpawn] Missing prefab reference.");
            return;
        }

        GameObject z = Instantiate(prefabToSpawn, point.position, point.rotation);
        Debug.Log($"[ZombieSpawn] Spawned '{z.name}' at lane {r} @ {point.position}");
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnpoints == null) return;

        Gizmos.color = Color.yellow;
        foreach (var p in spawnpoints)
        {
            if (!p) continue;
            Gizmos.DrawWireCube(p.position, new Vector3(0.5f, 0.5f, 0f));
        }
    }
}
