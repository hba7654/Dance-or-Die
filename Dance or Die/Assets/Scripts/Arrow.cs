using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Type type;

    [SerializeField] private float arrowSpeed;
    [SerializeField] private Vector2 arrowDir;
    [SerializeField] private bool canMove;

    private GameManager gameManager;
    private Score scoreType;

    private enum Score
    {
        None,
        Perfect,
        Good,
        OK
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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); 
        Debug.Log("Arrow type is " + type.ToString());
        scoreType = Score.None;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            transform.position += (Vector3)arrowDir * arrowSpeed * Time.deltaTime;

        //Debug.Log("Score is now " + scoreType.ToString());

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            switch (scoreType)
            {
                case Score.Perfect:
                    gameManager.score += 100;
                    Destroy(gameObject);
                    break;
                case Score.Good:
                    gameManager.score += 50;
                    Destroy(gameObject);
                    break;
                case Score.OK:
                    gameManager.score += 25;
                    Destroy(gameObject);
                    break;
                default:
                    Debug.Log("STOOPID");
                    Destroy(gameObject); 
                    break;
            }

            Debug.Log(gameManager.score);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Perfect":
                scoreType = Score.Perfect;
                break;
            case "Good":
                scoreType = Score.Good;
                break;
            case "OK":
                scoreType = Score.OK;
                break;
            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "OK")
        {
            scoreType = Score.None;
        }
    }

}
