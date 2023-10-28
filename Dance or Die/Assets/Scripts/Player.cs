using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Direction direction;
    private Vector2 dirVector;

    [SerializeField] private float speed;

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
    }

    private void Update()
    {
        transform.position += (Vector3)dirVector * speed * Time.deltaTime;
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

    private void OnBecameInvisible()
    {
        if(Mathf.Abs(transform.position.y) >= 0.1f)
            transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);

        else
            transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Finish")
        {
            direction = Direction.None;
            dirVector = Vector2.zero;
            transform.position = Vector3.zero;
        }
    }
}
