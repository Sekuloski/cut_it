using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : MonoBehaviour
{

    bool goingRight;
    CutPlankSpawner plankCutter;
    [SerializeField] float initialMoveSpeed = 3;
    [SerializeField] float moveSpeed = 3;
    [SerializeField] GameManager gameManager;


    void Awake()
    {
        goingRight = Random.Range(0, 1) > 0.5;
        plankCutter = GetComponent<CutPlankSpawner>();
    }

    void Update()
    {
        if (gameManager.isPlaying)
        {
            MovePlank();
        }
    }

    public bool Cut()
    {
        return plankCutter.Cut(transform.position.x, transform.position.y, moveSpeed / initialMoveSpeed);
    }

    private void MovePlank()
    {
        if (goingRight)
        {
            if (transform.position.x < 1.77f)
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                goingRight = false;
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
        }
        else
        {
            if (transform.position.x > -1.77f)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
            else
            {
                goingRight = true;
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
