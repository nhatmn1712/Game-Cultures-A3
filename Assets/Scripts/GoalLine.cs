using UnityEngine;

public class GoalLine : MonoBehaviour
{
    public int damageToPlayer = 1; // how much damage player takes

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if object is an enemy
        if (collision.CompareTag("Enemy"))
        {

            // Destroy the enemy (optional)
            Destroy(collision.gameObject);
        }
    }
}
