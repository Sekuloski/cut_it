using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPlankSpawner : MonoBehaviour
{
    private const float halfDestroyTime = 1f;
    GameManager gameManager;
    [SerializeField] float upwardForce = 3f;
    [SerializeField] float rightwardForce = 1f;
    [SerializeField] float rotationTorque = 0.8f;
    [SerializeField] PlankObjectSO[] planks;

    [Header("Piece Prefabs")]
    [SerializeField] GameObject eightPrefab;
    [SerializeField] GameObject halfPrefab;
    [SerializeField] GameObject quarterPrefab;
    [SerializeField] GameObject sevenEightPrefab;
    [SerializeField] GameObject threeQuarterPrefab;

    int activePlank = 0;
    Sprite[] currentSprites;

    static float sevenEightPosition = 0.125f;
    static float halfPosition = 0.5f;
    static float quarterPosition = halfPosition + sevenEightPosition*2;
    static float eightPosition = quarterPosition + sevenEightPosition;
    static float threeQuarterPosition = halfPosition / 2;

    int eight = 0;
    int half = 1;
    int quarter = 2;
    int sevenEight = 3;
    int threeQuarter = 4;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        currentSprites = planks[activePlank].LoadSprites();
    }

    public bool Cut(float x, float y, float moveSpeedMultiplier)
    {
        if (x < 0)
        {
            if (-x < halfPosition/2)
            {
                CutHalf(x, y, moveSpeedMultiplier);
            }
            else if (-x < quarterPosition)
            {
                CutRightQuarter(x, y, moveSpeedMultiplier);
            } 
            else if (-x < eightPosition + sevenEightPosition)
            {
                CutRightEight(x, y, moveSpeedMultiplier);
            }
            else
            {
                gameManager.UpdateScore(-5);
                return false;
            }
        }
        else
        {
            if (x < halfPosition/2)
            {
                CutHalf(x, y, moveSpeedMultiplier);
            }
            else if (x < quarterPosition)
            {
                CutLeftQuarter(x, y, moveSpeedMultiplier);
            }
            else if (x < eightPosition + sevenEightPosition)
            {
                CutLeftEight(x, y, moveSpeedMultiplier);
            }
            else
            {
                gameManager.UpdateScore(-5);
                return false;
            }
        }

        return true;
    }

    public void CutHalf(float offsetPosition, float y, float moveSpeed)
    {
        gameManager.UpdateScore(5);
        SpawnPieces(
            halfPrefab,
            halfPrefab,
            currentSprites[half],
            currentSprites[half+5], 
            halfPosition, 
            halfPosition,
            y,
            offsetPosition,
            moveSpeed,
            true
            );
    }

    public void CutLeftEight(float offsetPosition, float y, float moveSpeed)
    {
        gameManager.UpdateScore(1);
        SpawnPieces(
            eightPrefab,
            sevenEightPrefab,
            currentSprites[eight],
            currentSprites[sevenEight + 5],
            eightPosition,
            sevenEightPosition,
            y,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutLeftQuarter(float offsetPosition, float y, float moveSpeed)
    {
        gameManager.UpdateScore(2);
        SpawnPieces(
            quarterPrefab,
            threeQuarterPrefab,
            currentSprites[quarter],
            currentSprites[threeQuarter + 5],
            quarterPosition,
            threeQuarterPosition,
            y,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutRightQuarter(float offsetPosition, float y, float moveSpeed)
    {
        gameManager.UpdateScore(2);
        SpawnPieces(
            threeQuarterPrefab,
            quarterPrefab,
            currentSprites[threeQuarter],
            currentSprites[quarter + 5],
            threeQuarterPosition,
            quarterPosition,
            y,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutRightEight(float offsetPosition, float y, float moveSpeed)
    {
        gameManager.UpdateScore(1);
        SpawnPieces(
            sevenEightPrefab,
            eightPrefab,
            currentSprites[sevenEight],
            currentSprites[eight + 5],
            sevenEightPosition,
            eightPosition,
            y,
            offsetPosition,
            moveSpeed
            );
    }

    void SpawnPieces(
        GameObject leftPrefab, GameObject rightPrefab,
        Sprite leftSprite, Sprite rightSprite,
        float leftPosition, float rightPosition, float y,
        float offset, float moveSpeed,
        bool half = false
        )
    {
        GameObject leftPiece = Instantiate(leftPrefab, new Vector3(offset - leftPosition, y, 0), Quaternion.identity);
        SpriteRenderer spriteRenderer = leftPiece.GetComponent<SpriteRenderer>();
        Rigidbody2D rb2D = leftPiece.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = leftSprite;
        spriteRenderer.sortingOrder = 11;
        rb2D.AddForce(new Vector2(-rightwardForce * moveSpeed * Random.Range(0.5f, 1.5f), upwardForce), ForceMode2D.Impulse);

        if (!half )
        {
            leftPiece.GetComponent<Animator>().enabled = false;
            rb2D.AddTorque(rotationTorque * Random.Range(0.5f, 1.5f), ForceMode2D.Impulse);
        }
        else
        {
            leftPiece.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(leftPiece, halfDestroyTime);
        }

        GameObject rightPiece = Instantiate(rightPrefab, new Vector3(offset + rightPosition, y, 0), Quaternion.identity);
        spriteRenderer = rightPiece.GetComponent<SpriteRenderer>();
        rb2D = rightPiece.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = rightSprite;
        rb2D.AddForce(new Vector2(rightwardForce * moveSpeed * Random.Range(0.5f, 1.5f), upwardForce), ForceMode2D.Impulse);

        if (!half)
        {
            rightPiece.GetComponent<Animator>().enabled = false;
            rb2D.AddTorque(-rotationTorque * Random.Range(0.5f, 1.5f), ForceMode2D.Impulse);
        }
        else
        {
            rightPiece.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(rightPiece, halfDestroyTime);
        }
    }
}
