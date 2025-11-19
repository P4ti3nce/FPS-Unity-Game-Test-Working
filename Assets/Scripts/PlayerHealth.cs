using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public TextMeshProUGUI Healthtext;
    
    [SerializeField] private float maxHP;

    public HealthBar healthBar;

    private float currentHealth;
    private void Start()
    {
        currentHealth = maxHP;
        Healthtext.SetText("HP: " + currentHealth + " / " + maxHP);
        healthBar.SliderSetMax(maxHP);
    }
    private void Update()
    {
        if (currentHealth > maxHP)
        {
            currentHealth = maxHP;
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthBar.SliderSet(currentHealth);
        Healthtext.SetText("HP: " + currentHealth + " / " + maxHP);
    }
    public void HealPlayer(float amount)
    {
        currentHealth += amount;
        healthBar.SliderSet(currentHealth);
        Healthtext.SetText("HP: " + currentHealth + " / " + maxHP);
    }
    private void Die()
    {
        Debug.Log("You died!");
        SceneManager.LoadScene("Menu");
    }
    
}
