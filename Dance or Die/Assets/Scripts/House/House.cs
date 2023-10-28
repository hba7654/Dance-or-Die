using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class House : MonoBehaviour
{
    private Direction direction;
    private Vector2 dirVector;
    private GameObject killerInstance;
    private Vector2 killerDirVector;
    private int numRounds;

    [SerializeField] private float speed;
    [SerializeField] private float killerSpeed;
    [SerializeField] private GameObject killer;
    [SerializeField] private Transform[] killerSpawnPoints;

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
    }

    private void Update()
    {
        transform.position += (Vector3)dirVector * speed * Time.deltaTime;
        if (killerInstance != null)
            killerInstance.transform.position += (Vector3)killerDirVector * (killerSpeed + 0.5f * numRounds) * Time.deltaTime;

        if (Mathf.Abs(transform.position.x) >= 5.5)
        {
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);

            Destroy(killerInstance);
            numRounds++;
        }
        else if (Mathf.Abs(transform.position.y) >= 5.5)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);

            Destroy(killerInstance);
            numRounds++;
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

    //private void OnBecameInvisible()
    //{
    //    Destroy(killerInstance);
    //    numRounds++;

    //    if(Mathf.Abs(transform.position.y) >= 0.1f)
    //        transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);

    //    else
    //        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish" && numRounds > 0)
        {
            direction = Direction.None;
            dirVector = Vector2.zero;
            transform.position = Vector3.zero;

            //Spawn killer
            StartCoroutine(SpawnKiller());
            }
        else if (collision.tag == "Killer")
            SceneManager.LoadScene("GameScene");
    }

    private IEnumerator SpawnKiller()
    {
        //float timeToWait = Random.Range(0, 10f);
        //Debug.Log("WAITING FOR " + timeToWait + " SECONDS");
        //yield return new WaitForSeconds(timeToWait);

        int randPos = Random.Range(0, killerSpawnPoints.Length);
        killerInstance = Instantiate(killer, killerSpawnPoints[randPos].position, Quaternion.identity);
        killerDirVector = -(killerSpawnPoints[randPos].position - transform.position).normalized;

        yield return null;
    }
}
