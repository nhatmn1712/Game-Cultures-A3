using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1;
    public float speed = 8f;      // units/second
    public float lifetime = 6f;   // auto–destroy if it never hits

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move right each frame
        transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        // (The video uses Time.fixedDeltaTime in Update; using Time.deltaTime in Update is the standard.)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If we hit a zombie, damage it and destroy the pea
        Zombie z = other.GetComponent<Zombie>();
        if (z != null)
        {
            z.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
