using UnityEngine;

public class Draggable : MonoBehaviour
{
    private Vector3 offset;
    private Camera cam;
    private bool isPlaced = false; // lock after placed

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (isPlaced) return; // can't drag anymore
        offset = transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (isPlaced) return; // can't drag anymore
        transform.position = GetMouseWorldPosition() + offset;
    }

    private void OnMouseUp()
    {
        if (isPlaced) return;

        // Raycast to find grid square under mouse
        Vector2 mousePos = GetMouseWorldPosition();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponent<DropSlot>() != null)
        {
            DropSlot slot = hit.collider.GetComponent<DropSlot>();

            if (!slot.isOccupied)
            {
                // Snap to grid center
                transform.position = slot.transform.position;
                slot.isOccupied = true;

                // Lock the object (can't drag anymore)
                isPlaced = true;
            }
            else
            {
                // If occupied, just stay where it was dragged (optional: snap back)
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = 10f; // distance from camera
        return cam.ScreenToWorldPoint(mouseScreenPos);
    }
}
