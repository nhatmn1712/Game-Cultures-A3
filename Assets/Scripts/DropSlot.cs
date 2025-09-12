using UnityEngine;

public class DropSlot : MonoBehaviour
{
    public bool isOccupied = false;
    private GameObject currentPlant;

    public void PlacePlant(GameObject plant)
    {
        currentPlant = plant;
        isOccupied = true;

        // parent the plant to this slot so it's easy to track
        plant.transform.SetParent(transform);

        // when the plant gets destroyed → free the slot
        PlantDestroyedWatcher watcher = plant.AddComponent<PlantDestroyedWatcher>();
        watcher.onDestroyed = () =>
        {
            isOccupied = false;
            currentPlant = null;
            Debug.Log($"[DropSlot] Plant destroyed → Slot {name} is free again.");
        };
    }
}
