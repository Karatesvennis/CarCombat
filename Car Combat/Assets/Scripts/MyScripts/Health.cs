using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float startingHealth = 200f;
    [SerializeField] float currentHealth = 0f;

    [SerializeField] AudioClip deathSFX;

    //[SerializeField] GameObject deathVFX = null;

    [SerializeField] Slider healthBar = null;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        CalculateHealthBar();
    }

    public void DealDamage(float damage)
    {
        currentHealth -= damage;
        CalculateHealthBar();
        if (currentHealth <= 0f)
        {
            if (tag == "Player")
            {
                FindObjectOfType<GameManager>().loseLabel.gameObject.SetActive(true);
                FindObjectOfType<GameManager>().restartButton.gameObject.SetActive(true);
                FindObjectOfType<GameManager>().mainMenuButton.gameObject.SetActive(true);
                FindObjectOfType<GameManager>().crosshair.SetActive(false);
                Cursor.visible = true;
            }

            AudioSource.PlayClipAtPoint(deathSFX, gameObject.transform.position, 1f);
            Destroy(gameObject, 0.5f);
        }
    }

    private void CalculateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / startingHealth;
        }
    }


}
