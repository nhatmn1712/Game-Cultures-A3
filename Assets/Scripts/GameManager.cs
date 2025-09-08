using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    // selection
    public GameObject currentPlant;
    public Sprite currentPlantSprite;
    public int sun = 50;

    [Header("Placement")]
    public LayerMask tileMask;   // set this in Inspector to your Tile layer

    // Called by PlantSlot when a card is clicked
    public void BuyPlant(GameObject plantPrefab, Sprite icon, int cost)
    {
        if (plantPrefab == null || icon == null) { Debug.LogWarning("[Gamemanager] Missing prefab/icon."); return; }
        if (sun < cost) { Debug.Log("[Gamemanager] Not enough sun."); return; }

        sun -= cost;
        currentPlant = plantPrefab;
        currentPlantSprite = icon;

        Debug.Log($"[Gamemanager] Selected {plantPrefab.name} (cost {cost}). Sun left: {sun}");
    }

    private void Update()
    {
        // nothing selected? do nothing
        if (currentPlant == null) return;

        // mouse -> world
        Vector3 m3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        m3.z = 0f;

        // which tile is under the mouse?
        Collider2D hit = Physics2D.OverlapPoint(m3, tileMask);

        // left click to place
        if (hit != null && Input.GetMouseButtonDown(0))
        {
            // prevent stacking plants
            Tile tile = hit.GetComponent<Tile>();
            if (tile != null && tile.hasPlant)
            {
                Debug.Log("[Gamemanager] Tile already has a plant.");
                return;
            }

            Instantiate(currentPlant, hit.transform.position, Quaternion.identity);
            if (tile != null) tile.hasPlant = true;

            Debug.Log($"[Gamemanager] Planted {currentPlant.name} on {hit.name}");

            // clear selection after placing (optional)
            currentPlant = null;
            currentPlantSprite = null;
        }
    }
}
