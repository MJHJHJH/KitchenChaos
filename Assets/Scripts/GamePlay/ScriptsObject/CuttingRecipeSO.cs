using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeSO", menuName = "ScriptableObject/CuttingRecipeSO")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    //需要处理的进度值
    public int needPower = 3;
}