using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipeSO", menuName = "ScriptableObject/FryingRecipeSO")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    //需要处理的进度值
    public float FryingTimeMax = 3;
}