using UnityEngine;

public class CardDraggable : MonoBehaviour
{
    public GameObject unitPrefab; // your dreamcatcher prefab
    private GameObject draggingUnit;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        // Spawn a copy of the dreamcatcher
        draggingUnit = Instantiate(unitPrefab);
    }

    private void OnMouseDrag()
    {
        if (draggingUnit == null) return;

        Vector3 mousePos = GetMouseWorldPosition();
        draggingUnit.transform.position = mousePos;
    }

    private void OnMouseUp()
    {
        if (draggingUnit == null) return;

        Vector2 mousePos = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponent<DropSlot>() != null)
        {
            DropSlot slot = hit.collider.GetComponent<DropSlot>();

            if (!slot.isOccupied)
            {
                // Snap and mark occupied
                draggingUnit.transform.position = slot.transform.position;
                slot.isOccupied = true;

                // Lock the unit (it shouldn’t move anymore)
                var dragScript = draggingUnit.GetComponent<Draggable>();
                if (dragScript != null) dragScript.enabled = false;

                draggingUnit = null; // reset
                return;
            }
        }

        // If invalid → destroy the ghost
        Destroy(draggingUnit);
        draggingUnit = null;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f;
        return cam.ScreenToWorldPoint(mouseScreenPos);
    }
}
