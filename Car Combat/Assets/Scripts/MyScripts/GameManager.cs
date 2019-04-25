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


    // Start is called before the first frame update
    void Start()
    {
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
        if (nrOfEnemiesAlive <= 0 && !lastWave)
        {
            FindObjectOfType<EnemySpawner>().SpawnEnemies();
            lastWave = true;
        }
        else if (nrOfEnemiesAlive <= 0 && lastWave)
        {
            winLabel.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
            mainMenuButton.gameObject.SetActive(true);
            Cursor.visible = true;
            crosshair.SetActive(false);
        }
    }
}
