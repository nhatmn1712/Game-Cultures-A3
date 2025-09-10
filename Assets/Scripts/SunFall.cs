using UnityEngine;

public class SunFall : MonoBehaviour
{
    private bool collected = false;
    private Vector3 targetPosition;   // grid position
    private float fallSpeed = 2f;     // falling speed
    private float lifetime = 3f;      // time before disappearing
    private Vector3 barPosition = new Vector3(-7f, 4f, 0f); // adjust to your bar position
    private float collectSpeed = 10f; // speed when flying to bar

    private void Start()
    {
        // Sun starts a bit higher
        targetPosition = transform.position;
        transform.position += new Vector3(0, 2f, 0);

        // Destroy automatically after lifetime if not clicked
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!collected)
        {
            // Falling down smoothly
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                fallSpeed * Time.deltaTime
            );
        }
        else
        {
            // Fly to bar
            transform.position = Vector3.MoveTowards(
                transform.position,
                barPosition,
                collectSpeed * Time.deltaTime
            );

            // If reached bar → give points and destroy
            if (Vector3.Distance(transform.position, barPosition) < 0.1f)
            {
                GameManager1.instance.AddSun(25);
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (collected) return;

        collected = true;
    }
}
