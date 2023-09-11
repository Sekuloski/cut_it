using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidLineManager : MonoBehaviour
{
    SpriteRenderer redLine;
    float midLineHeightReal = -0.4434f;
    float midLineHeight = -0.6f;
    public float highestPiece = 0;
    [SerializeField] GameObject midLine;

    ArrayList cutPieces;

    private void Awake()
    {
        redLine = midLine.GetComponent<SpriteRenderer>();
        cutPieces = new ArrayList();
    }

    private void Update()
    {
        float newHighest = -6;
        foreach (GameObject piece in cutPieces)
        {
            if (piece.transform.position.y > midLineHeight)
            {
                newHighest = midLineHeight;
            }
            else if (piece.transform.position.y > newHighest)
            {
                newHighest = piece.transform.position.y;
            }
        }
        if (newHighest > -6)
        {
            highestPiece = newHighest;
        }
       
        if (highestPiece > -5)
        {
            float newAlpha = midLineHeight / highestPiece;
            redLine.color = new Color(255, 255, 255, newAlpha);
        }
        else
        {
            redLine.color = new Color(255, 255, 255, 0);
        }
    }

    public void UpdateObjects(GameObject piece)
    {
        cutPieces.Add(piece);
    }
}
