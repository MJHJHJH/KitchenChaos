using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeSO", menuName = "ScriptableObject/RecipeSO")]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOs;
    public string recipeName;
}
