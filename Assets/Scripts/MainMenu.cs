using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Playgame1()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void Playgame2()
    {
        SceneManager.LoadScene("Game2");
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    private void OnEnable()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
