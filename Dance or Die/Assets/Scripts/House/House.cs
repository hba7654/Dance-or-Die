using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class House : MonoBehaviour
{
    private Direction direction;
    private Vector2 dirVector;
    private GameObject killerInstance;
    private Vector2 killerDirVector;
    private int numRounds;
    private bool isSafe;
    private GameObject itemSpawned;
    private bool inMiddle;
    private SpriteRenderer sr;
    private int animCounter;

    [SerializeField] private float speed;
    [SerializeField] private float killerSpeed;
    [SerializeField] private GameObject killer;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform[] killerSpawnPoints;
    [SerializeField] private Transform[] itemSpawnPoints;
    [SerializeField] private int roundsToSpawnItems;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Sprite[] playerSprites;
    [SerializeField] private Sprite[] killerSprites;
    [SerializeField] private SpriteRenderer floor;
    [SerializeField] private AudioSource music;

    private enum Direction
    {
        Up, 
        Down, 
        Left, 
        Right,
        None
    }

    private void Start()
    {
        direction = Direction.None;
        dirVector = Vector2.zero;
        numRounds = 0;
        isSafe = false;
        inMiddle = true;
        sr = GetComponent<SpriteRenderer>();
        animCounter = 0;
    }

    private void Update()
    {
        scoreText.text = "Score: " + GameManager.score;
        livesText.text = "Lives: " + Arrow.lives;

        transform.position += (Vector3)dirVector * speed * Time.deltaTime;
        if (killerInstance != null)
            killerInstance.transform.position += (Vector3)killerDirVector * (killerSpeed + 0.5f * numRounds) * Time.deltaTime;

        //Entering new rooms
        if (Mathf.Abs(transform.position.x) >= 5.5)
        {
            if (isSafe)
            {
                transform.position = new Vector3(-5.4f * Mathf.Sign(transform.position.x), transform.position.y, transform.position.z);
                NewRoom();
            }
            else
                Caught();
        }
        else if (Mathf.Abs(transform.position.y) >= 5.5)
        {
            if (isSafe)
            {
                transform.position = new Vector3(transform.position.x, -5.4f * Mathf.Sign(transform.position.y), transform.position.z);
                NewRoom();
            }
            else
                Caught();
        }
        music.volume = MainMenu.volume;
    }

    public void Input(InputAction.CallbackContext context)
    {
        if (context.started && direction == Direction.None)
        {
            StartCoroutine(Animate());
            dirVector = context.ReadValue<Vector2>();
            transform.rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * Mathf.Atan2(dirVector.x, -dirVector.y), new Vector3(0, 0, 1));
            if (dirVector.x > 0)
            {
                direction = Direction.Right;
            }
            else if (dirVector.x < 0)
            {
                direction = Direction.Left;
            }
            else if (dirVector.y > 0)
            {
                direction = Direction.Up;
            }
            else if (dirVector.y < 0)
            {
                direction = Direction.Down;
            }
        }
    }

    private void NewRoom()
    {
        GameManager.score += 25;
        floor.color = Color.HSVToRGB(Random.Range(0, 1f), 1, 1);
        if(Random.Range(0, 1f) < 0.5f)
            floor.flipX = true;
        else floor.flipX = false;
        if (Random.Range(0, 1f) < 0.5f)
            floor.flipY = true;
        else floor.flipY = false;


        //Reactivate all doors
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].SetActive(true);
        }

        //Clear all items
        Destroy(itemSpawned);

        Destroy(killerInstance);
        numRounds++;

        //Randomize Doors
        if (Random.Range(0, 1f) < 0.5)
        {
            doors[Random.Range(0, doors.Length)].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Finish":
                if(numRounds > 0)
                {
                    inMiddle = true;

                    direction = Direction.None;
                    dirVector = Vector2.zero;
                    transform.position = Vector3.zero;
                    isSafe = false;

                    //Spawn killer
                    StartCoroutine(SpawnKiller());

                    //Generate Items
                    if (numRounds >= roundsToSpawnItems)
                    {
                        Transform itemSpawnPos = itemSpawnPoints[Random.Range(0, itemSpawnPoints.Length)];
                        itemSpawned = Instantiate(items[Random.Range(0, items.Length)], itemSpawnPos.position, Quaternion.identity);
                    }
                }
                break;
            case "Killer":
                Caught();
                break;
            case "Door":
                isSafe = true;
                break;
            case "DoublePoints":
                GameManager.score += 25;
                Destroy(collision.gameObject);
                break;
            case "Health":
                Arrow.lives++;
                Destroy(collision.gameObject);
                break;
        }
    }

    private IEnumerator SpawnKiller()
    {
        //float timeToWait = Random.Range(0, 10f);
        //Debug.Log("WAITING FOR " + timeToWait + " SECONDS");
        //yield return new WaitForSeconds(timeToWait);

        int randPos = Random.Range(0, killerSpawnPoints.Length);
        killerInstance = Instantiate(killer, killerSpawnPoints[randPos]);
        killerDirVector = -(killerSpawnPoints[randPos].position - transform.position).normalized;

        while (true)
        {
            killerInstance.GetComponent<SpriteRenderer>().sprite = killerSprites[animCounter];
            animCounter = (animCounter + 1) % 2;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void Caught()
    {
        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator Animate()
    {
        inMiddle = false;
        while (!inMiddle)
        {
            sr.flipX = !sr.flipX;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
