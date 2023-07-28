using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCollider : MonoBehaviour
{
    int deathTimer = 4;
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            deathTimer = 4;
            StartCoroutine(PlankCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            StopAllCoroutines();
            deathTimer = 4;
        }
    }

    IEnumerator PlankCountdown()
    {
        while (deathTimer > 0)
        {
            deathTimer -= 1;

            Debug.Log(deathTimer);
            yield return new WaitForSeconds(1);
        }

        gameManager.EndGame();
        yield return null;
    }
}
