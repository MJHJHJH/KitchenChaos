using System.Collections.Generic;
using UnityEngine;

//盘子厨房物体,多一个添加其他KitchenObject的功能
public class PlatesObject : KitchenObject
{
    public PlatesVisual platesVisual;
    private List<KitchenObjectSO> hasKitchenObjectSOs = new List<KitchenObjectSO>();
    public List<KitchenObjectSO> GetHasKitchenObjectSOs()
    {
        return hasKitchenObjectSOs;
    }

    public PlatesObject(KitchenObjectSO kichenObjectSO, IKitchenObjectParent _iKitchenObjectParent = null, GameObject _gameObject = null) : base(kichenObjectSO, _iKitchenObjectParent, _gameObject)
    {
    }

    public bool AddKitchenObject(KitchenObject KichenObject)
    {
        bool result = platesVisual.TryAddGameObject(KichenObject.GetKichenObjectSO());
        if (result)
        {
            hasKitchenObjectSOs.Add(KichenObject.GetKichenObjectSO());
            KichenObject.Clear();
        }
        return result;
    }

    //生成KichenObject
    public static new PlatesObject CreateKichenObject(KitchenObjectSO kichenObjectSO,
         IKitchenObjectParent _iKitchenObjectParent)
    {
        GameObject tomatoUnit = GameObject.Instantiate(kichenObjectSO.prefab);
        tomatoUnit.name = kichenObjectSO.PrefabName;
        PlatesObject platesObject = new PlatesObject(kichenObjectSO, _iKitchenObjectParent, tomatoUnit);
        tomatoUnit.transform.parent = _iKitchenObjectParent.GetTopAnchorPoint();
        tomatoUnit.transform.localPosition = Vector3.zero;
        platesObject.platesVisual = tomatoUnit.GetComponent<PlatesVisual>();
        platesObject.isPlatesObject = true;
        return platesObject;
    }
}