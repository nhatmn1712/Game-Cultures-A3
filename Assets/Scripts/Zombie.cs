// Zombie.cs
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed = 1.5f;
    public int health = 5;

    private void FixedUpdate()
    {
        // move left each physics tick
        transform.position += Vector3.left * speed * Time.fixedDeltaTime;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Destroy(gameObject);
    }
}
