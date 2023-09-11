using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceCollider : MonoBehaviour
{
    private int deathTimer = 4;
    GameManager gameManager;
    MidLineManager midLineManager;
    private Coroutine coroutine;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        midLineManager = FindObjectOfType<MidLineManager>();
        Invoke("AddToMidLine", 2);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            Debug.Log("Starting Countdown");
            deathTimer = 4;
            coroutine = StartCoroutine(PlankCountdown());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "MiddleLine")
        {
            StopCoroutine(coroutine);
            coroutine = null;
            Debug.Log("Ended Countdown");
            deathTimer = 4;
        }
    }

    private IEnumerator PlankCountdown()
    {
        while (deathTimer > 0)
        {
            deathTimer -= 1;

            Debug.Log(deathTimer);
            yield return new WaitForSeconds(1);
        }

        gameManager.StopGame();
    }

    private void AddToMidLine()
    {
        midLineManager.UpdateObjects(gameObject);
    }
}
