// ShooterPlant.cs
using UnityEngine;

public class ShooterPlant : MonoBehaviour
{
    [Header("Firing")]
    public GameObject bulletPrefab;
    public Transform muzzle;
    public float fireRate = 1f;
    public float scanInterval = 0.15f;
    public float rowTolerance = 0.2f;
    public float lookAheadDistance = 40f;

    private float nextFireTime;
    private float nextScanTime;

    // call this from your placer if you prefer
    public void Activate() { enabled = true; }
    public void Deactivate() { enabled = false; }

    void Update()
    {
        if (Time.time < nextScanTime) return;
        nextScanTime = Time.time + scanInterval;

        if (EnemyAheadInSameRow())
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + (1f / fireRate);
            }
        }
    }

    void Fire()
    {
        Vector3 spawnPos = muzzle ? muzzle.position : (transform.position + Vector3.right * 0.3f);
        Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
    }

    bool EnemyAheadInSameRow()
    {
        float myY = transform.position.y;
        float myX = transform.position.x;

        // simple scan of all zombies in scene
        var zombies = FindObjectsOfType<Zombie>();
        foreach (var z in zombies)
        {
            if (!z) continue;

            // roughly same row (Y), and ahead (X greater)
            if (Mathf.Abs(z.transform.position.y - myY) <= rowTolerance &&
                z.transform.position.x > myX &&
                (z.transform.position.x - myX) <= lookAheadDistance)
            {
                return true;
            }
        }
        return false;
    }
}
