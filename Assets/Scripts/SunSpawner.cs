using UnityEngine;

public class SunSpawner : MonoBehaviour
{
    public GameObject sunPrefab;
    public float spawnInterval = 5f;
    private float timer;

    private GameObject[] squares;

    private void Start()
    {
        squares = GameObject.FindGameObjectsWithTag("Square");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnSun();
            timer = 0f;
        }
    }

    private void SpawnSun()
    {
        if (squares.Length == 0) return;

        GameObject randomSquare = squares[Random.Range(0, squares.Length)];
        Vector3 spawnPos = randomSquare.transform.position;

        Instantiate(sunPrefab, spawnPos, Quaternion.identity);
    }
}
