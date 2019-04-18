using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Health : MonoBehaviour
{
    [SerializeField] float startingHealth;
    [SerializeField] float currentHealth;

    [SerializeField] GameObject deathVFX;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            if (tag == "Player")
            {
                FindObjectOfType<GameManager>().loseLabel.gameObject.SetActive(true);
                FindObjectOfType<GameManager>().restartButton.gameObject.SetActive(true);
                Cursor.visible = true;
            }
            GameObject myDeathVFX = Instantiate(deathVFX, this.transform.position, Quaternion.identity);
            Destroy(myDeathVFX, 2f);
            Destroy(gameObject);
        }
    }


}
