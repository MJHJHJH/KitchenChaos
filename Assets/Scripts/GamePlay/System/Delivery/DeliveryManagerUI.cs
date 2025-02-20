using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerUI : MonoBehaviour
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

    private void Start()
    {
        recipeTemplate.SetActive(false);
        DeliveryManager.Instance.AddRecipeSO_Event += AddRecipeSOAction;
        DeliveryManager.Instance.RemoveRecipeSO_Event += RemoveRecipeSOAction;
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
}
