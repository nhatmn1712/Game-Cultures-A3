using UnityEngine;

public class Zombie : MonoBehaviour
{

    public float speed;

    public int health;

    private void FixedUpdate()
    {
        transform.position -= new Vector3(speed * Time.fixedDeltaTime, 0f, 0f);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}