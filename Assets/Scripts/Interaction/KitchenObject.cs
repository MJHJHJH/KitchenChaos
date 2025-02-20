using System;
using UnityEngine;
using UnityEngine.Rendering;

public class KitchenObject
{
    public bool isPlatesObject = false;
    private GameObject gameObject;
    private IKitchenObjectParent iKitchenObjectParent;
    //对应的资源SO
    private KitchenObjectSO currentKichenObjectSO;
    public KitchenObjectSO GetKichenObjectSO()
    {
        return currentKichenObjectSO;
    }

    public KitchenObject(KitchenObjectSO kichenObjectSO,
        IKitchenObjectParent _iKitchenObjectParent = null,
        GameObject _gameObject = null)
    {
        currentKichenObjectSO = kichenObjectSO;
        InitData(_iKitchenObjectParent, _gameObject);
    }

    public void InitData(IKitchenObjectParent _iKitchenObjectParent)
    {
        if (_iKitchenObjectParent != null && gameObject != null)
        {
            iKitchenObjectParent = _iKitchenObjectParent;
            gameObject.transform.parent = iKitchenObjectParent.GetTopAnchorPoint();
            gameObject.transform.localPosition = Vector3.zero;
        }
    }

    public void InitData(IKitchenObjectParent _iKitchenObjectParent, GameObject _gameObject)
    {
        if (_iKitchenObjectParent != null && _gameObject != null)
        {
            gameObject = _gameObject;
            ChangeiKitchenObjectParent(_iKitchenObjectParent);
        }
    }

    public void ChangeiKitchenObjectParent(IKitchenObjectParent newiKitchenObjectParent)
    {
        if (iKitchenObjectParent != null)
        {
            iKitchenObjectParent.ClearKichenObject(this);
        }
        newiKitchenObjectParent.GetNewKichenObject(this);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    //只有给垃圾桶交互才会调用真正的删除掉 - 后续有对象池类这里进行回收处理
    public void Clear()
    {
        iKitchenObjectParent.ClearKichenObject(this);
        if (gameObject)
        {
            GameObject.Destroy(gameObject);
        }
        gameObject = null;
        iKitchenObjectParent = null;
    }

    //生成KichenObject
    public static KitchenObject CreateKichenObject(KitchenObjectSO kichenObjectSO,
         IKitchenObjectParent _iKitchenObjectParent)
    {
        GameObject tomatoUnit = GameObject.Instantiate(kichenObjectSO.prefab);
        tomatoUnit.name = kichenObjectSO.PrefabName;
        KitchenObject kichenObject = new KitchenObject(kichenObjectSO, _iKitchenObjectParent, tomatoUnit);
        tomatoUnit.transform.parent = _iKitchenObjectParent.GetTopAnchorPoint();
        tomatoUnit.transform.localPosition = Vector3.zero;
        return kichenObject;
    }
}