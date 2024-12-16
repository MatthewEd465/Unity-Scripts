using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;

    public List<string> collectedItems = new List<string>();
    public List<int> pickedUpItemIDs = new List<int>();

    public float playerFear;
    public string currentObjective;
}


