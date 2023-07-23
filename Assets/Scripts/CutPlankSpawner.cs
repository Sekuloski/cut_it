using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutPlankSpawner : MonoBehaviour
{
    private const float halfDestroyTime = 1f;
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

    private void Start()
    {
        currentSprites = planks[activePlank].LoadSprites();
    }

    public bool Cut(float x, float moveSpeedMultiplier)
    {
        if (x < 0)
        {
            if (-x < halfPosition/2)
            {
                CutHalf(x, moveSpeedMultiplier);
            }
            else if (-x < quarterPosition)
            {
                CutRightQuarter(x, moveSpeedMultiplier);
            } 
            else if (-x < eightPosition + sevenEightPosition)
            {
                CutRightEight(x, moveSpeedMultiplier);
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (x < halfPosition/2)
            {
                CutHalf(x, moveSpeedMultiplier);
            }
            else if (x < quarterPosition)
            {
                CutLeftQuarter(x, moveSpeedMultiplier);
            }
            else if (x < eightPosition + sevenEightPosition)
            {
                CutLeftEight(x, moveSpeedMultiplier);
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void CutHalf(float offsetPosition, float moveSpeed)
    {
        SpawnPieces(
            halfPrefab,
            halfPrefab,
            currentSprites[half],
            currentSprites[half+5], 
            halfPosition, 
            halfPosition,
            offsetPosition,
            moveSpeed,
            true
            );
    }

    public void CutLeftEight(float offsetPosition, float moveSpeed)
    {
        SpawnPieces(
            eightPrefab,
            sevenEightPrefab,
            currentSprites[eight],
            currentSprites[sevenEight + 5],
            eightPosition,
            sevenEightPosition,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutLeftQuarter(float offsetPosition, float moveSpeed)
    {
        SpawnPieces(
            quarterPrefab,
            threeQuarterPrefab,
            currentSprites[quarter],
            currentSprites[threeQuarter + 5],
            quarterPosition,
            threeQuarterPosition,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutRightQuarter(float offsetPosition, float moveSpeed)
    {
        SpawnPieces(
            threeQuarterPrefab,
            quarterPrefab,
            currentSprites[threeQuarter],
            currentSprites[quarter + 5],
            threeQuarterPosition,
            quarterPosition,
            offsetPosition,
            moveSpeed
            );
    }

    public void CutRightEight(float offsetPosition, float moveSpeed)
    {
        SpawnPieces(
            sevenEightPrefab,
            eightPrefab,
            currentSprites[sevenEight],
            currentSprites[eight + 5],
            sevenEightPosition,
            eightPosition,
            offsetPosition,
            moveSpeed
            );
    }

    void SpawnPieces(
        GameObject leftPrefab, GameObject rightPrefab,
        Sprite leftSprite, Sprite rightSprite,
        float Position, float rightPosition,
        float offset, float moveSpeed,
        bool half = false
        )
    {
        GameObject leftPiece = Instantiate(leftPrefab, new Vector3(offset - Position, 0, 0), Quaternion.identity);
        SpriteRenderer spriteRenderer = leftPiece.GetComponent<SpriteRenderer>();
        Rigidbody2D rb2D = leftPiece.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = leftSprite;
        rb2D.AddForce(new Vector2(-rightwardForce * moveSpeed * Random.Range(0.5f, 1.5f), upwardForce), ForceMode2D.Impulse);

        if (!half )
        {
            rb2D.AddTorque(rotationTorque * Random.Range(0.5f, 1.5f), ForceMode2D.Impulse);
        }
        else
        {
            leftPiece.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(leftPiece, halfDestroyTime);
        }

        GameObject rightPiece = Instantiate(rightPrefab, new Vector3(offset + rightPosition, 0, 0), Quaternion.identity);
        spriteRenderer = rightPiece.GetComponent<SpriteRenderer>();
        rb2D = rightPiece.GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = rightSprite;
        rb2D.AddForce(new Vector2(rightwardForce * moveSpeed * Random.Range(0.5f, 1.5f), upwardForce), ForceMode2D.Impulse);

        if (!half )
        {
            rb2D.AddTorque(-rotationTorque * Random.Range(0.5f, 1.5f), ForceMode2D.Impulse);
        }
        else
        {
            rightPiece.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(rightPiece, halfDestroyTime);
        }
    }
}
