using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Axe Object", fileName = "New Axe")]
public class AxeSO : ScriptableObject
{
    [Header("General")]
    public string name = "Enter Axe name";
    public Sprite axeTexture;
}
