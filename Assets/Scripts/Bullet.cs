// Bullet.cs
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 1;
    public float maxLife = 5f;

    void Start()
    {
        Destroy(gameObject, maxLife);
    }

    void Update()
    {
        // move to the right
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;  // ignore sensors
        var z = other.GetComponent<Zombie>();
        if (z != null)
        {
            z.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
