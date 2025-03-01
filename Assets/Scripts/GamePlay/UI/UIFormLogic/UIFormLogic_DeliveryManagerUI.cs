using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class UIFormLogic_DeliveryManagerUI : UIFormLogicExtension
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject recipeTemplate;
    struct RecipeSOAndRecipeSO
    {
        public RecipeSO SO;
        public GameObject recipeGameObject;
        public RecipeSOAndRecipeSO(RecipeSO _SO, GameObject _recipeGameObject)
        {
            SO = _SO;
            recipeGameObject = _recipeGameObject;
        }
    }
    private List<RecipeSOAndRecipeSO> useRecipeSOAndRecipeSO = new List<RecipeSOAndRecipeSO>();

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
    }

    protected override void OnOpen(object userData)
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
        recipeTemplate.SetActive(false);
        InitAllRecipeSO();
        DeliveryManager.Instance.AddRecipeSO_Event += AddRecipeSOAction;
        DeliveryManager.Instance.RemoveRecipeSO_Event += RemoveRecipeSOAction;
        base.OnOpen(userData);
    }

    protected override void OnClose(bool isShutdown, object userData)
    {
        GameManager.Instance.OnStateChanged -= OnStateChangedAction;
        DeliveryManager.Instance.AddRecipeSO_Event -= AddRecipeSOAction;
        DeliveryManager.Instance.RemoveRecipeSO_Event -= RemoveRecipeSOAction;
        ClearAllRecipeSO();
        base.OnClose(isShutdown, userData);
    }

    private void InitAllRecipeSO()
    {
        List<RecipeSO> recipeSOs = DeliveryManager.Instance.GetWaitingRecipeSOList();
        foreach (var r in recipeSOs)
        {
            AddRecipeSOAction(r);
        }
    }

    private void AddRecipeSOAction(RecipeSO sO)
    {
        GameObject recipeGameObject = Instantiate(recipeTemplate, container);
        recipeGameObject.GetComponent<RecipeTemplate>().SetRecipeSO(sO);
        recipeGameObject.SetActive(true);
        useRecipeSOAndRecipeSO.Add(new RecipeSOAndRecipeSO(sO, recipeGameObject));
    }

    private void RemoveRecipeSOAction(RecipeSO sO)
    {
        for (int index = 0; index < useRecipeSOAndRecipeSO.Count; index++)
        {
            if (useRecipeSOAndRecipeSO[index].SO == sO)
            {
                Destroy(useRecipeSOAndRecipeSO[index].recipeGameObject);
                useRecipeSOAndRecipeSO.RemoveAt(index);
            }
        }

    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        if (state != GameManager.GameState.GamePlaying)
        {
            this.Close();
        }
    }

    private void ClearAllRecipeSO()
    {
        for (int index = 0; index < useRecipeSOAndRecipeSO.Count; index++)
        {
            Destroy(useRecipeSOAndRecipeSO[index].recipeGameObject);
        }
        useRecipeSOAndRecipeSO.Clear();
    }
}
