using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static float volume;

    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject howToPlayMenu;
    [SerializeField] private Slider volSlider;
    [SerializeField] private AudioSource musicPlayer;
    private void Start()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + GameManager.score;
            GameManager.score = 0;
            GameManager.numTimesCaught = 0;
        }
    }

    private void Update()
    {
        if(volSlider != null)
        {
            volume = volSlider.value;
            musicPlayer.volume = volume;
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

    public void HowToPlay()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        howToPlayMenu.SetActive(!mainMenu.activeInHierarchy);
    }
}
