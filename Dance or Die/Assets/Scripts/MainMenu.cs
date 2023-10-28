using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.score;
            GameManager.score = 0;
            GameManager.numTimesCaught = 0;
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("HouseScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
