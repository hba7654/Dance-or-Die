using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public int score;

    private float timer;
    private int randomNum;
    [SerializeField] private float nextGenTime;
    [SerializeField] private GameObject[] arrows;
    [SerializeField] private Transform[] arrowSpawnPos;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        randomNum = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timeIncrementer();
        if (timer > nextGenTime) {
            timer = 0;
            randomNum = UnityEngine.Random.Range(0,4);
            //Debug.Log(randomNum);
            CreateArrow(randomNum);
        }
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
