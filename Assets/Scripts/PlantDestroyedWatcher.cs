using UnityEngine;
using System;

public class PlantDestroyedWatcher : MonoBehaviour
{
    public Action onDestroyed;

    private void OnDestroy()
    {
        if (onDestroyed != null)
            onDestroyed.Invoke();
    }
}
