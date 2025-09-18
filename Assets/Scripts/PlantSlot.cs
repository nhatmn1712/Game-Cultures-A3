using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PlantSlot : MonoBehaviour,
    IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Card Data")]
    public Sprite plantSprite;
    public GameObject plantPrefab;        // the plant prefab to place
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
        // only start if we can afford
        if (!GameManager.Instance || !GameManager.Instance.CanAfford(price)) return;

        // spawn ghost copy to drag (do NOT spend yet)
        draggingPlant = Instantiate(plantPrefab, MouseWorld(), Quaternion.identity);

        // ghost look + disable colliders while dragging
        var sr = draggingPlant.GetComponentInChildren<SpriteRenderer>();
        if (sr) sr.color = Ghost;
        SetAllCollidersEnabled(draggingPlant, false);

        // keep passive while dragging (e.g. shooter disabled)
        var shooter = draggingPlant.GetComponentInChildren<ShooterPlant>(true);
        if (shooter) shooter.enabled = false;
    }

    public void OnBeginDrag(PointerEventData _) { /* not used */ }

    public void OnDrag(PointerEventData _)
    {
        if (draggingPlant) draggingPlant.transform.position = MouseWorld();
    }

    public void OnEndDrag(PointerEventData _)
    {
        if (!draggingPlant) return;

        // look for a free DropSlot under the mouse
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
            // place and lock into the slot
            draggingPlant.transform.position = target.transform.position;
            target.PlacePlant(draggingPlant);

            // restore look + colliders
            var sr = draggingPlant.GetComponentInChildren<SpriteRenderer>();
            if (sr) sr.color = Color.white;
            SetAllCollidersEnabled(draggingPlant, true);

            // disable any drag script on the placed plant
            var oldDrag = draggingPlant.GetComponent<Draggable>();
            if (oldDrag) oldDrag.enabled = false;

            // enable shooter/behaviour after placement
            var shooter = draggingPlant.GetComponentInChildren<ShooterPlant>(true);
            if (shooter) shooter.enabled = true;

            // NOW spend sun (only after successful placement)
            if (GameManager.Instance) GameManager.Instance.SpendSun(price);

            draggingPlant = null;
        }
        else
        {
            // invalid drop → delete the ghost and do not spend
            Destroy(draggingPlant);
            draggingPlant = null;
        }
    }

    Vector3 MouseWorld()
    {
        var mp = Input.mousePosition;
        mp.z = 0f; // orthographic cam
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
