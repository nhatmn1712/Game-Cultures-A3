using System.Collections;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [Header("Setup")]
    public Transform[] spawnpoints;
    public GameObject zombiePrefab;

    [Header("Spawning")]
    public float initialDelay = 0.5f;
    public Vector2 spawnIntervalRange = new Vector2(2f, 4f);

    [Header("Limits")]
    public int totalToSpawn = 20;   

    private Coroutine spawnLoopCo;

    private void Start()
    {
        Debug.Log($"[ZombieSpawn] Start | lanes={spawnpoints?.Length ?? 0} | prefab={(zombiePrefab ? zombiePrefab.name : "<null>")} | totalToSpawn={totalToSpawn}");
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

        // Spawn exactly totalToSpawn zombies, then stop
        for (int i = 0; i < totalToSpawn; i++)
        {
            SpawnZombie();

            // no wait after the last one
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

    public void SpawnZombie()
    {
        if (spawnpoints == null || spawnpoints.Length == 0 || zombiePrefab == null)
        {
            Debug.LogWarning("[ZombieSpawn] Spawner not set up (missing lanes or prefab).");
            return;
        }

        int r = Random.Range(0, spawnpoints.Length);
        Transform point = spawnpoints[r];

        GameObject z = Instantiate(zombiePrefab, point.position, point.rotation);
        Debug.Log($"[ZombieSpawn] Spawned '{z.name}' at lane {r} @ {point.position}");
    }

    // Gizmo helpers
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