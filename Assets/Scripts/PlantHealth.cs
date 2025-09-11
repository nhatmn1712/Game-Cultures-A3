using UnityEngine;
using System.Collections;

public class PlantHealth : MonoBehaviour
{
    public int maxHealth = 6;
    public float hitFlashTime = 0.06f;

    int current;
    SpriteRenderer sr;
    Color baseColor;

    void Awake()
    {
        current = maxHealth;
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr) baseColor = sr.color;
    }

    public void TakeDamage(int dmg)
    {
        current -= dmg;
        if (sr) StartCoroutine(Flash());
        if (current <= 0) Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(hitFlashTime);
        sr.color = baseColor;
    }
}
