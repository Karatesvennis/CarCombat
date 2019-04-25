using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] enemySpawnPoints;

    
    void Start()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            Instantiate(enemy, enemySpawnPoints[i].position, Quaternion.identity);
        }
    }
}
