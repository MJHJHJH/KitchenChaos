using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KichenObjectSO", menuName = "ScriptableObject/KichenObjectSO")]
public class KitchenObjectSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite sprite;
    public int spriteID;
    public string PrefabName
    {
        get
        {
            return prefab.name;
        }

    }
}
