using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemy;
    public Transform[] spawnPoints;
    public float startingSpawnTime = 10f;
    public float spawnRate = 0.1f;
    public int minSpawn = 1;
    public int maxSpawn = 3;

    private List<GameObject> enemies;
    private float nextSpawnTime= 0f;
    

    private void OnEnable()
    {
        enemies = new List<GameObject>();
    }

    private void SpawnEnemies()
    {
        int numberOfEnemies = Random.Range(minSpawn, maxSpawn);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 newEnemyPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            GameObject newEnemy = Instantiate(enemy, newEnemyPosition, Quaternion.identity);
            enemies.Add(newEnemy);
        }
    }


    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Debug.Log("Current time is " + Time.time + " and the next Spawn Time is " + nextSpawnTime);
            SpawnEnemies();
            nextSpawnTime = Time.time + startingSpawnTime;
            if (startingSpawnTime > 0.5f)
            {
                startingSpawnTime -= spawnRate;
            }
            Debug.Log("Spawn complete. Current time is " + Time.time + " and the next Spawn Time is " + nextSpawnTime);
        }

        if (enemies.Count > 9)
        {
            foreach (GameObject enemy in enemies.ToList())
            {
                Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
                if (rb.simulated == false)
                {
                    enemy.SetActive(false);
                    enemies.Remove(enemy);
                    break;
                }
            }
        }
    }
}
