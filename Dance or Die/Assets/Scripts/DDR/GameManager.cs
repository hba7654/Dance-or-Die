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
    public static int score;

    private float timer;
    private float nextGenTime;
    private int randomNum;

    [SerializeField] private float minNextGenTime;
    [SerializeField] private float maxNextGenTime;
    [SerializeField] private int winScore;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Transform[] arrowSpawnPos;
    [SerializeField] private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        randomNum = 0;
        timer = 0;
        nextGenTime = UnityEngine.Random.Range(minNextGenTime - 0.05f*numTimesCaught, maxNextGenTime - 0.2f*numTimesCaught);

        numTimesCaught++;
    }

    // Update is called once per frame
    void Update()
    {
        timeIncrementer();
        if (timer > nextGenTime) {
            timer = 0;
            nextGenTime = UnityEngine.Random.Range(minNextGenTime - 0.05f * numTimesCaught, maxNextGenTime - 0.2f * numTimesCaught);
            randomNum = UnityEngine.Random.Range(0,arrows.Length);
            //Debug.Log(randomNum);
            CreateArrow(randomNum);
        }
        scoreText.text = "Score: " + score;

        if (score >= winScore)
            SceneManager.LoadScene("HouseScene");
    }

    private void CreateArrow(int num)
    {
        Instantiate(arrows[num], arrowSpawnPos[num].position, Quaternion.identity).GetComponent<Arrow>().type = Enum.Parse<Arrow.Type>(arrows[num].name);
    }

    float timeIncrementer()
    {
        return timer += Time.deltaTime;
    }
}
