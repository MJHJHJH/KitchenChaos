using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RecipeTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    [SerializeField] private GameObject container;
    [SerializeField] private GameObject Template;
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        textMeshProUGUI.text = recipeSO.recipeName;
        Template.SetActive(false);
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOs)
        {
            GameObject go = Instantiate(Template, container.transform);
            go.GetComponent<RecipeIconTemplate>().SetIcon(kitchenObjectSO.sprite);
            go.SetActive(true);
        }
    }
}
