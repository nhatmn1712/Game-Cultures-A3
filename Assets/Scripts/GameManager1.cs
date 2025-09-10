using UnityEngine;
using UnityEngine.UI;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;
    public int sunPoints = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddSun(int amount)
    {
        sunPoints += amount;
        Debug.Log("Sun points: " + sunPoints);
        // later update your UI here
    }
}