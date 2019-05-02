using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] GameObject enemy;
    [SerializeField] Transform[] enemySpawnPoints;

    [SerializeField] Text waveCountdownText;

    public bool firstWaveSpawned = false;

    int countdownTime = 3;

    public bool spawning;


    private void Awake()
    {
        waveCountdownText.enabled = false;
    }

    public void SpawnEnemies()
    {
        spawning = true;
        StartCoroutine(CountDown(countdownTime));
    }

    IEnumerator CountDown(int seconds)
    {
        Debug.Log("countdown started");
        waveCountdownText.enabled = true;
        int count = seconds;
        while (count > 0)
        {
            waveCountdownText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
        }

        waveCountdownText.enabled = false;

        for (int i = 0; i < enemySpawnPoints.Length; i++)
        {
            Instantiate(enemy, enemySpawnPoints[i].position, Quaternion.identity);
        }

        firstWaveSpawned = true;
        spawning = false;
    }
}
