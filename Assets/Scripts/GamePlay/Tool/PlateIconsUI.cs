using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private Transform iconTemplate;

    public void UpdateVisual(KitchenObjectSO so)
    {
        Transform tr = Instantiate(iconTemplate, transform);
        tr.gameObject.SetActive(true);
        tr.GetComponent<IconTemplate>().SetIcon(so.spriteID);
    }
}
