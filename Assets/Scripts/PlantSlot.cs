using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlantSlot : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Data")]
    public Sprite plantSprite;
    public GameObject plantPrefab;        // your plant prefab
    public int price = 0;

    [Header("Card UI")]
    public Image icon;
    public TextMeshProUGUI priceText;

    private Camera cam;
    private GameObject draggingPlant;
    private static readonly Color Ghost = new Color(1f, 1f, 1f, 0.6f);

    void Awake()
    {
        cam = Camera.main;

        if (icon) { icon.enabled = plantSprite != null; icon.sprite = plantSprite; }
        if (priceText) priceText.text = price.ToString();

        // If this object also has a Button, clear onClick to avoid conflicts with drag
        var btn = GetComponent<Button>();
        if (btn) btn.onClick.RemoveAllListeners();
    }

    public void OnPointerDown(PointerEventData _)
    {
        // Only start if we can afford
        if (!GameManager.instance || !GameManager.instance.CanAfford(price))
            return;

        // Spawn ghost copy to drag
        draggingPlant = Instantiate(plantPrefab, MouseWorld(), Quaternion.identity);

        // ghost look + disable colliders
        var sr = draggingPlant.GetComponentInChildren<SpriteRenderer>();
        if (sr) sr.color = Ghost;
        SetAllCollidersEnabled(draggingPlant, false);

        // keep passive while dragging
        var shooter = draggingPlant.GetComponentInChildren<ShooterPlant>(true);
        if (shooter) shooter.enabled = false;
    }

    public void OnBeginDrag(PointerEventData _) { /* nothing needed */ }

    public void OnDrag(PointerEventData _)
    {
        if (draggingPlant) draggingPlant.transform.position = MouseWorld();
    }

    public void OnEndDrag(PointerEventData _)
    {
        if (!draggingPlant) return;

        // Look for a free DropSlot under the mouse
        Vector2 p = MouseWorld();
        var hits = Physics2D.OverlapPointAll(p);

        DropSlot target = null;
        foreach (var h in hits)
        {
            var slot = h.GetComponent<DropSlot>();
            if (slot != null && !slot.isOccupied) { target = slot; break; }
        }

        if (target != null)
        {
            // Place and lock into the slot
            draggingPlant.transform.position = target.transform.position;
            target.PlacePlant(draggingPlant);   // <<<< this handles freeing when destroyed

            // restore look + colliders
            var sr = draggingPlant.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.color = Color.white;
            SetAllCollidersEnabled(draggingPlant, true);

            // disable dragging
            var oldDrag = draggingPlant.GetComponent<Draggable>();
            if (oldDrag) oldDrag.enabled = false;

            // enable shooter
            var shooter = draggingPlant.GetComponentInChildren<ShooterPlant>(true);
            if (shooter) shooter.enabled = true;

            // spend sun
            GameManager.instance.SpendSun(price);

            draggingPlant = null;
        }
        else
        {
            // Invalid drop → delete ghost
            Destroy(draggingPlant);
            draggingPlant = null;
        }
    }

    Vector3 MouseWorld()
    {
        var mp = Input.mousePosition;
        mp.z = 0f; // orthographic
        var w = cam.ScreenToWorldPoint(mp);
        w.z = 0f;
        return w;
    }

    void SetAllCollidersEnabled(GameObject go, bool enabled)
    {
        foreach (var c in go.GetComponentsInChildren<Collider2D>(true))
            c.enabled = enabled;
    }
}
