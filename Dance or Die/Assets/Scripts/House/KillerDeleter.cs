using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerDeleter : MonoBehaviour
{
    int numCols;
    private void Start()
    {
        numCols = 0;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Door")
        {
            numCols++;
            if(numCols == 2)
                Destroy(gameObject);
        }
    }
}
