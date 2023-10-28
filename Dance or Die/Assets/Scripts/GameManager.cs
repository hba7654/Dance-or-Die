using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int score;

    private float timer;
    private float nextGenTime;
    private int randomNum;

    [SerializeField] private float minNextGenTime;
    [SerializeField] private float maxNextGenTime;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Transform[] arrowSpawnPos;
    [SerializeField] private Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        randomNum = 0;
        timer = 0;
        nextGenTime = UnityEngine.Random.Range(minNextGenTime, maxNextGenTime);
    }

    // Update is called once per frame
    void Update()
    {
        timeIncrementer();
        if (timer > nextGenTime) {
        nextGenTime = UnityEngine.Random.Range(minNextGenTime, maxNextGenTime);
            timer = 0;
            nextGenTime = UnityEngine.Random.Range(minNextGenTime, maxNextGenTime);
            randomNum = UnityEngine.Random.Range(0,4);
            //Debug.Log(randomNum);
            CreateArrow(randomNum);
        }
        scoreText.text = "Score: " + score;
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
