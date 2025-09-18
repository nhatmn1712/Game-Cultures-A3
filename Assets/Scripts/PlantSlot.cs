using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlantSlot : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Data")]
    public Sprite plantSprite;
    public GameObject placedPrefab;       // ✅ this is the placed plant (no drag logic)
    public int price = 0;

    [Header("Card UI")]
    public Image icon;
    public TextMeshProUGUI priceText;

    private Camera cam;
    private GameObject draggingGhost;     // ghost copy to drag
    private static readonly Color Ghost = new Color(1f, 1f, 1f, 0.6f);

    void Awake()
    {
        cam = Camera.main;

        if (icon) { icon.enabled = plantSprite != null; icon.sprite = plantSprite; }
        if (priceText) priceText.text = price.ToString();

        var btn = GetComponent<Button>();
        if (btn) btn.onClick.RemoveAllListeners();
    }

    public void OnPointerDown(PointerEventData _)
    {
        if (!GameManager.Instance || !GameManager.Instance.CanAfford(price)) return;

        // make a ghost (copy of placedPrefab) to drag
        draggingGhost = Instantiate(placedPrefab, MouseWorld(), Quaternion.identity);

        // ghost look
        var sr = draggingGhost.GetComponentInChildren<SpriteRenderer>();
        if (sr) sr.color = Ghost;

        // disable gameplay while dragging
        SetAllCollidersEnabled(draggingGhost, false);
        var shooter = draggingGhost.GetComponentInChildren<ShooterPlant>(true);
        if (shooter) shooter.enabled = false;
    }

    public void OnBeginDrag(PointerEventData _) { }

    public void OnDrag(PointerEventData _)
    {
        if (draggingGhost) draggingGhost.transform.position = MouseWorld();
    }

    public void OnEndDrag(PointerEventData _)
    {
        if (!draggingGhost) return;

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
            // place real prefab (not ghost) in the slot
            GameObject placedPlant = Instantiate(placedPrefab, target.transform.position, Quaternion.identity);
            target.PlacePlant(placedPlant);

            // enable gameplay
            var shooter = placedPlant.GetComponentInChildren<ShooterPlant>(true);
            if (shooter) shooter.enabled = true;

            SetAllCollidersEnabled(placedPlant, true);

            if (GameManager.Instance) GameManager.Instance.SpendSun(price);
        }

        // always destroy the ghost
        Destroy(draggingGhost);
        draggingGhost = null;
    }

    Vector3 MouseWorld()
    {
        var mp = Input.mousePosition;
        mp.z = 0f;
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
