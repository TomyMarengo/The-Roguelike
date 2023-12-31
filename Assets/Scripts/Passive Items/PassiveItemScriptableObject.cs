using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemScriptableObject", menuName = "ScriptableObjects/Passive Item")]
public class PassiveItemScriptableObject : ScriptableObject
{
    [SerializeField]
    float multiplier;
    public float Multiplier { get => multiplier; private set => multiplier = value; }

    [SerializeField]
    int level; // Not meant to be modified in the game (Only in Editor)
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab; // The prefab of the next level i.e. what the object becomes when levels up
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    string passiveItemName;
    public string PassiveItemName { get => passiveItemName; private set => passiveItemName = value; }

    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }

    [SerializeField]
    string type;
    public string Type { get => type; private set => type = value; }

    [SerializeField]
    Sprite icon; // Not meant to be modified in the game (Only in Editor)
    public Sprite Icon { get => icon; private set => icon = value; }

}
