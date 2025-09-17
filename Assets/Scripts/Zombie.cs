using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;
    public int health = 5;

    [Header("Attack")]
    public int biteDamage = 1;     // damage per bite
    public float biteInterval = 1f;// seconds between bites

    PlantHealth targetPlant;       // current plant we’re biting
    float nextBiteTime = 0f;

    void Update()
    {
        // If not biting a plant, keep walking left
        if (targetPlant == null)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            return;
        }

        // Bite on a timer
        if (Time.time >= nextBiteTime)
        {
            if (targetPlant != null)   // (will become null when destroyed)
                targetPlant.TakeDamage(biteDamage);

            nextBiteTime = Time.time + biteInterval;
        }
    }


    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            if (WinManager.Instance != null) WinManager.Instance.RegisterKill();
            Destroy(gameObject);
        }
    }


    // ---- Trigger area in front of the zombie ----
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (targetPlant != null) return;
        if (other.TryGetComponent<PlantHealth>(out var plant))
            targetPlant = plant; // stop walking, start biting
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (targetPlant == null) return;
        if (other.GetComponent<PlantHealth>() == targetPlant)
            targetPlant = null; // resume walking
    }
}
