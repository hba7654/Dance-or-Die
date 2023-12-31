using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int numTimesCaught = 0;
    public static int score = 0;

    [HideInInspector] public float timer;
    [HideInInspector] public int curScore;

    private float genTimer;
    private float nextGenTime;
    private int randomNum;

    [SerializeField] private float minNextGenTime;
    [SerializeField] private float maxNextGenTime;
    [SerializeField] private float timeToWin;
    [SerializeField] private int roundScore;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Transform[] arrowSpawnPos;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text livesText;
    [SerializeField] private AudioSource musicPlayer;
    [SerializeField] private AudioClip[] musicTracks;

    // Start is called before the first frame update
    void Start()
    {
        numTimesCaught++;

        randomNum = 0;
        timer = timeToWin;
        genTimer = 0;
        curScore = 0;
        nextGenTime = UnityEngine.Random.Range(minNextGenTime - 0.05f*numTimesCaught, maxNextGenTime - 0.2f*numTimesCaught);
        musicPlayer.clip = musicTracks[UnityEngine.Random.Range(0, musicTracks.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        genTimer += Time.deltaTime;
        timer -= Time.deltaTime;
        if (genTimer > nextGenTime) {
            genTimer = 0;
            nextGenTime = UnityEngine.Random.Range(minNextGenTime - 0.05f * numTimesCaught, maxNextGenTime - 0.2f * numTimesCaught);
            randomNum = UnityEngine.Random.Range(0,arrows.Length);
            CreateArrow(randomNum);
        }
        timeText.text = String.Format("DANCE! {0:F2}",timer);
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + Arrow.lives;

        if (timer <= 0)
        {
            score += 50;
            SceneManager.LoadScene("HouseScene");
        }

        musicPlayer.volume = MainMenu.volume;
    }

    private void CreateArrow(int num)
    {
        Instantiate(arrows[num], arrowSpawnPos[num]).GetComponent<Arrow>();
    }
}
