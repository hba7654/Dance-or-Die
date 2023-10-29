using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowSpeedMult;
    [SerializeField] private Vector2 arrowDir;
    [SerializeField] private KeyCode input1;
    [SerializeField] private KeyCode input2;
    [SerializeField] private Sprite dodgeSprite;
    [SerializeField] private Sprite idleSprite;

    private Text livesText;
    private SpriteRenderer player;
    private SpriteRenderer sr;

    private static int lives = 5;

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
    private void Start()
    { 
        scoreType = Score.None;
        acceptingInput = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        livesText = GameObject.Find("Lives").GetComponent<Text>();
        livesText.text = "Lives: " + lives;

        player = GameObject.Find("Player").GetComponent<SpriteRenderer>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)arrowDir * (arrowSpeed + arrowSpeedMult * GameManager.numTimesCaught) * Time.deltaTime;
        //Debug.Log("Score is now " + scoreType.ToString());

        if (acceptingInput && (Input.GetKeyDown(input1) || Input.GetKeyDown(input2)))
        {
            StartCoroutine(Dodge());

            switch (scoreType)
            {
                case Score.Perfect:
                    gameManager.timer -= 10;
                    //gameManager.curScore += 100;
                    //GameManager.score += 100;
                    // Destroy(gameObject);
                    break;
                case Score.Good:
                    //gameManager.curScore += 50;
                    //GameManager.score += 50;
                    // Destroy(gameObject);
                case Score.OK:
                    //gameManager.curScore += 25;
                    //GameManager.score += 25;
                    // Destroy(gameObject);
                    sr.enabled = false;
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
        // Debug.Log("Score type is " + scoreType.ToString());
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
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives == 0)
        {
            SceneManager.LoadScene("DeathScene");
        }
    }

    private IEnumerator Dodge()
    {
     
        player.sprite = dodgeSprite;
        Debug.Log("Dodging");

        yield return new WaitForSeconds(0.5f);

        player.sprite = idleSprite;
        Debug.Log("idle");

        Destroy(gameObject);
        yield return null;
    }

}
