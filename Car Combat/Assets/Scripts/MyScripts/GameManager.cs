using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public int nrOfEnemiesAlive;
    public bool lastWave = false;

    [SerializeField] Text winLabel;
    public Text loseLabel;
    public Button restartButton;
    public Button mainMenuButton;
    public GameObject crosshair;
    private EnemyShoot enemyShoot;
    private EnemySpawner enemySpawner;


    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemies();
        }
        
        winLabel.gameObject.SetActive(false);
        loseLabel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false);
        crosshair = FindObjectOfType<Crosshair>().gameObject;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //TestToKillEnemies();

        if (enemySpawner.firstWaveSpawned && nrOfEnemiesAlive == 0 && !lastWave)
        {
            lastWave = true;
            enemySpawner.SpawnEnemies();
        }
        else if (enemySpawner.firstWaveSpawned && nrOfEnemiesAlive == 0 && lastWave && enemySpawner.spawning == false)
        {
            winLabel.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            Cursor.visible = true;
            crosshair.SetActive(false);
        }
    }

    void TestToKillEnemies()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
            }
        }
    }
}
