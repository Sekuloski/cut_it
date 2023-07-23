using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Plank Object", fileName = "New Plank")]
public class PlankObjectSO : ScriptableObject
{
    [Header("General")]
    [SerializeField] string name = "Enter Plank name";
    [SerializeField] Texture2D spritesheetTexture;

    [Header("Left Pieces")]
    public Sprite leftEight;
    public Sprite leftHalf;
    public Sprite leftQuarter;
    public Sprite leftSevenEight;
    public Sprite leftThreeQuarter;

    [Header("Right Pieces")]
    public Sprite rightEight;
    public Sprite rightHalf;
    public Sprite rightQuarter;
    public Sprite rightSevenEight;
    public Sprite rightThreeQuarter;
    
    public Sprite[] LoadSprites()
    {       
        return Resources.LoadAll<Sprite>("Planks/" + spritesheetTexture.name);
    }
}
