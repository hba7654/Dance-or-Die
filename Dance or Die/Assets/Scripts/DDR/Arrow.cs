using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Arrow : MonoBehaviour
{
    [SerializeField] private bool canMove;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float arrowSpeedMult;
    [SerializeField] private Vector2 arrowDir;
    [SerializeField] private KeyCode input1;
    [SerializeField] private KeyCode input2;
    [SerializeField] private Sprite dodgeSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite stabSprite;
    [SerializeField] private Sprite standSprite;

    private Text livesText;
    private SpriteRenderer player;
    private SpriteRenderer killer;
    private SpriteRenderer sr;
    private BoxCollider2D box;

    public static int lives = 5;

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
        killer = GameObject.Find("Killer").GetComponent<SpriteRenderer>();

        sr = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            transform.position += (Vector3)arrowDir * (arrowSpeed + arrowSpeedMult * GameManager.numTimesCaught) * Time.deltaTime;

        if (/*acceptingInput && */(Input.GetKeyDown(input1) || Input.GetKeyDown(input2)))
        {
            StartCoroutine(Dodge());

            switch (scoreType)
            {
                case Score.Perfect:
                    gameManager.timer -= 10;
                    sr.enabled = false;
                    box.enabled = false;
                    StartCoroutine(Stab());
                    break;
                case Score.Good:
                case Score.OK:
                    sr.enabled = false;
                    box.enabled = false;
                    StartCoroutine(Stab());
                    break;
                case Score.Death:
                    StartCoroutine(Stab());
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
            StartCoroutine(Stab());
        }
    }

    private void Death()
    {
        lives--;
        livesText.text = "Lives: " + lives;

        if (lives == 0)
        {
            lives = 5;
            SceneManager.LoadScene("DeathScene");
        }
    }

    private IEnumerator Dodge()
    {
     
        player.sprite = dodgeSprite;
        Debug.Log("Dodging");

        yield return new WaitForSeconds(0.5f / GameManager.numTimesCaught);

        player.sprite = idleSprite;
        Debug.Log("idle");

        if(canMove)
            Destroy(gameObject);
        yield return null;
    }

    private IEnumerator Stab()
    {
        killer.sprite = stabSprite;
        Debug.Log("Dodging");

        yield return new WaitForSeconds(0.5f / GameManager.numTimesCaught);

        killer.sprite = standSprite;
        Debug.Log("idle");

        yield return null;
    }

}
