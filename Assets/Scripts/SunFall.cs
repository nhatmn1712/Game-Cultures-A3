using UnityEngine;
using System.Collections;

public class SunFall : MonoBehaviour
{
    [Header("Fall & Lifetime")]
    [SerializeField] float dropOffsetY = 2f;      // start a bit above the tile
    [SerializeField] float fallSpeed = 2f;        // falling speed
    [SerializeField] float lifetime = 3f;         // time before auto-despawn if not clicked

    [Header("Collect")]
    [SerializeField] Transform collectTarget;     // <-- assign your red-slot target here
    [SerializeField] float collectSpeed = 10f;    // fly speed toward red slot
    [SerializeField] int reward = 1;              // sun points to add

    bool collected = false;
    Vector3 tileCenter;                           // final position on the tile

    void Awake()
    {
        if (collectTarget == null)
        {
            GameObject t = GameObject.Find("SunTarget"); // exact name in hierarchy
            if (t != null) collectTarget = t.transform;
        }
    }


    void Start()
    {
        // The spawner instantiates at the tile center; start above & fall down to it
        tileCenter = transform.position;
        transform.position = tileCenter + Vector3.up * dropOffsetY;

        // auto-despawn after a while if not clicked
        StartCoroutine(DespawnTimer());
    }

    void Update()
    {
        if (!collected)
        {
            // fall smoothly to the tile center
            transform.position = Vector3.MoveTowards(
                transform.position, tileCenter, fallSpeed * Time.deltaTime);
        }
        else if (collectTarget != null)
        {
            // fly to the red slot
            transform.position = Vector3.MoveTowards(
                transform.position, collectTarget.position, collectSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, collectTarget.position) < 0.05f)
            {
                GameManager.instance.AddSun(reward);
                Destroy(gameObject);
            }
        }
    }

    void OnMouseDown()
    {
        if (collected) return;
        collected = true;
        // stop the despawn timer so it won’t vanish mid-flight
        StopAllCoroutines();
        // optional: avoid double-clicks while flying
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;
    }

    IEnumerator DespawnTimer()
    {
        yield return new WaitForSeconds(lifetime);
        if (!collected) Destroy(gameObject);
    }
}
