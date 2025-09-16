using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadSceneOnTimelineEnd : MonoBehaviour
{
    [SerializeField] PlayableDirector director;   // drag your Timeline object
    [SerializeField] string sceneName = "MainLevel";
    [SerializeField] float delay = 0f;            // optional pause before loading

    void Reset() { director = GetComponent<PlayableDirector>(); }

    void OnEnable()
    {
        if (!director) director = GetComponent<PlayableDirector>();
        if (director) director.stopped += OnTimelineStopped;
    }

    void OnDisable()
    {
        if (director) director.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector d)
    {
        if (delay > 0f) StartCoroutine(LoadAfterDelay());
        else LoadNow();
    }

    IEnumerator LoadAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        LoadNow();
    }

    void LoadNow()
    {
        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
