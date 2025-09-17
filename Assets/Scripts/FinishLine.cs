using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class FinishLine : MonoBehaviour
{
    [Tooltip("Scene to load when an Enemy reaches the line (build index).")]
    public int sceneBuildIndex = 3;   // your WhenLoseScene is index 3
    [Tooltip("Optional: load by name instead of index (safer). Leave empty to use index.")]
    public string sceneName = "";

    private bool loading;

    private void Reset()
    {
        // Make sure the collider behaves like a trigger “tripwire”
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (loading) return;
        if (!other.CompareTag("Enemy")) return;

        loading = true;
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(sceneBuildIndex);
    }

#if UNITY_EDITOR
    // Just to visualize the line in Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.25f);
        var col = GetComponent<Collider2D>();
        if (col) Gizmos.DrawCube(col.bounds.center, col.bounds.size);
    }
#endif
}
