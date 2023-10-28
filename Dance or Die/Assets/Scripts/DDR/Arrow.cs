using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour
{
    public Type type;

    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowSpeedMult;
    [SerializeField] private Vector2 arrowDir;
    [SerializeField] private bool canMove;
    [SerializeField] private KeyCode input1;
    [SerializeField] private KeyCode input2;

    private Score scoreType;
    private bool acceptingInput;
    private GameManager gameManager;

    private enum Score
    {
        None,
        Perfect,
        Good,
        OK,
        Death
    }
    public enum Type
    {
        Up,
        Down,
        Left,
        Right
    }

    private void Start()
    { 
        scoreType = Score.None;
        acceptingInput = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            transform.position += (Vector3)arrowDir * (arrowSpeed + arrowSpeedMult*GameManager.numTimesCaught)* Time.deltaTime;

        //Debug.Log("Score is now " + scoreType.ToString());

        if (acceptingInput && (Input.GetKeyDown(input1) || Input.GetKeyDown(input2)))
        {
            switch (scoreType)
            {
                case Score.Perfect:
                    gameManager.curScore += 100;
                    GameManager.score += 100;
                    Destroy(gameObject);
                    break;
                case Score.Good:
                    gameManager.curScore += 50;
                    GameManager.score += 50;
                    Destroy(gameObject);
                    break;
                case Score.OK:
                    gameManager.curScore += 25;
                    GameManager.score += 25;
                    Destroy(gameObject);
                    break;
                case Score.Death:
                    Death();
                    break;
                default:
                    Debug.Log("STOOPID");
                    //Destroy(gameObject); 
                    break;
            }

            Debug.Log(GameManager.score);
        }
    }

    private void OnBecameInvisible()
    {
        if (scoreType == Score.Death)
            Death();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        acceptingInput = true;
        switch(collision.tag)
        {
            case "Perfect":
                scoreType = Score.Perfect;
                break;
            case "Good":
                scoreType = Score.Good;
                break;
            case "OK":
            case "LastOK":
                scoreType = Score.OK;
                break;
        }
        Debug.Log("Score type is " + scoreType.ToString());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "LastOK")
        {
            scoreType = Score.Death;
        }
    }

    private void Death()
    {
        SceneManager.LoadScene("DeathScene");
    }

}
