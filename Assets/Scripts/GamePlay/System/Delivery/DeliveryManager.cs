using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private List<RecipeSO> allRecipeSOList;
    private List<RecipeSO> waitingRecipeSOList = new List<RecipeSO>();

    private static DeliveryManager _Instance = null;
    public static DeliveryManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    private int MaxNeedRecipe = 5;
    private float spawnRecipeTimer = 0f;
    private float spawnRecipeTimerMax = 3f;
    private int alreadyDeliveryNum = 0;
    public int GetAlreadyDeliveryNum() { return alreadyDeliveryNum; }

    public event Action<RecipeSO> AddRecipeSO_Event = null;
    public event Action<RecipeSO> RemoveRecipeSO_Event = null;

    //交付成功与失败的广播
    public event Action<bool> RecipeSOResult_Event = null;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }

        GameManager.Instance.OnStateChanged += OnStetaChangedAction;
    }

    private void OnStetaChangedAction(GameManager.GameState state)
    {
        if (state == GameManager.GameState.WaitingToStart)
        {
            alreadyDeliveryNum = 0;
        }
    }

    private void Update()
    {
        if (waitingRecipeSOList.Count < MaxNeedRecipe)
        {
            spawnRecipeTimer += Time.deltaTime;
            if (spawnRecipeTimer >= spawnRecipeTimerMax)
            {
                spawnRecipeTimer = 0f;
                AddRecipeSO();
            }
        }
    }

    private void AddRecipeSO()
    {
        RecipeSO recipeSO = allRecipeSOList[UnityEngine.Random.Range(0, allRecipeSOList.Count)];
        waitingRecipeSOList.Add(recipeSO);
        AddRecipeSO_Event?.Invoke(recipeSO);
    }

    public bool DeliverRecipe(PlatesObject platesObject, DeliveryCounter deliveryCounter)
    {
        List<KitchenObjectSO> platesObjectsHasKitchenObjectSOs = platesObject.GetHasKitchenObjectSOs();
        platesObject.Clear();
        for (int index = 0; index < waitingRecipeSOList.Count; index++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[index];
            if (waitingRecipeSO.kitchenObjectSOs.Count == platesObjectsHasKitchenObjectSOs.Count)
            {
                bool result = true;
                for (int j = 0; j < platesObjectsHasKitchenObjectSOs.Count; j++)
                {
                    if (!waitingRecipeSO.kitchenObjectSOs.Contains(platesObjectsHasKitchenObjectSOs[j]))
                    {
                        result = false;
                        break;
                    }
                }
                if (result)
                {
                    RemoveRecipeSO_Event(waitingRecipeSOList[index]);
                    waitingRecipeSOList.RemoveAt(index);
                    RecipeSOResult_Event?.Invoke(true);
                    SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.deliverySuccess,
                        deliveryCounter.gameObject.transform.position);
                    return true;
                }
            }
        }
        RecipeSOResult_Event?.Invoke(false);
        SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.deliveryFail,
                       deliveryCounter.gameObject.transform.position);
        return false;
    }
}
