using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class EnemyCount : MonoBehaviour
{
    public int enemyCount;
    public TextMeshProUGUI text;
    public int totalenemies;
    public GameObject winnerScreen;
    // Start is called before the first frame update
    void Start()
    {
        text.SetText("Enemy Number: " + totalenemies + " / " + totalenemies);
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyCount <= 0)
        {
            
            
            Time.timeScale = 0;
            winnerScreen.SetActive(true);
            
        }
    }
    public void AddEnemy()
    {
        enemyCount += 1;
        text.SetText("Enemy Number: "+ enemyCount + " / " + totalenemies);
    }
    public void RemoveEnemy()
    {
        enemyCount -= 1;
        text.SetText("Enemy Number: " + enemyCount + " / " + totalenemies);
    }
}
