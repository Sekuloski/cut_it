using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            Debug.Log("Entered");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            Debug.Log("Exited");
        }
    }
}
