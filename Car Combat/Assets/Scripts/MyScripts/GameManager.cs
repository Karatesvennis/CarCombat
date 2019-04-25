using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int nrOfEnemiesAlive;
    bool lastWave = false;

    [SerializeField] Text winLabel;
    public Text loseLabel;
    public Button restartButton;


    // Start is called before the first frame update
    void Start()
    {
        winLabel.gameObject.SetActive(false);
        loseLabel.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
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
            Cursor.visible = true;
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("ProofOfConcept");
    }
}
