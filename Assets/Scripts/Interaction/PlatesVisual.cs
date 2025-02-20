using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesVisual : MonoBehaviour
{
    [Serializable]
    public struct KichenObjectSOAndGO
    {
        public KitchenObjectSO so;
        public GameObject go;
    }

    public PlateIconsUI plateIconsUI;

    public KichenObjectSOAndGO[] allKichenObjectSOAndGO;

    public bool TryAddGameObject(KitchenObjectSO so)
    {
        for (int i = 0; i < allKichenObjectSOAndGO.Length; i++)
        {
            if (allKichenObjectSOAndGO[i].so == so)
            {
                if (allKichenObjectSOAndGO[i].go.activeSelf)
                {
                    return false;
                }
                allKichenObjectSOAndGO[i].go.SetActive(true);
                plateIconsUI.UpdateVisual(allKichenObjectSOAndGO[i].so);
                return true;
            }
        }
        return false;
    }
}
