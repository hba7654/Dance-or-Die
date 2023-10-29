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

    [SerializeField] private float speed;
    [SerializeField] private float killerSpeed;
    [SerializeField] private GameObject killer;
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform[] killerSpawnPoints;
    [SerializeField] private Transform[] itemSpawnPoints;
    [SerializeField] private int roundsToSpawnItems;
    [SerializeField] private GameObject[] doors;

    [SerializeField] private Text scoreText;

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
    }

    private void Update()
    {
        scoreText.text = "Score: " + GameManager.score;

        transform.position += (Vector3)dirVector * speed * Time.deltaTime;
        if (killerInstance != null)
            killerInstance.transform.position += (Vector3)killerDirVector * (killerSpeed + 0.5f * numRounds) * Time.deltaTime;

        //Entering new rooms
        if (Mathf.Abs(transform.position.x) >= 5.5)
        {
            if (isSafe)
            {
                transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
                NewRoom();
            }
            else
                Caught();
        }
        else if (Mathf.Abs(transform.position.y) >= 5.5)
        {
            if (isSafe)
            {
                transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
                NewRoom();
            }
            else
                Caught();
        }
    }

    public void Input(InputAction.CallbackContext context)
    {
        if (direction == Direction.None)
        {
            dirVector = context.ReadValue<Vector2>();
            if (dirVector.x > 0)
                direction = Direction.Right;
            else if (dirVector.x < 0)
                direction = Direction.Left;
            else if (dirVector.y > 0)
                direction = Direction.Up;
            else if (dirVector.y < 0)
                direction = Direction.Down;
        }
    }

    private void NewRoom()
    {
        GameManager.score += 25;

        //Reactivate all doors
        for(int i = 0; i < doors.Length; i++)
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

        yield return null;
    }

    private void Caught()
    {
        SceneManager.LoadScene("GameScene");
    }
}
