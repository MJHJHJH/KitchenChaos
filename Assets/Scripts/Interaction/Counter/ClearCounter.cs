using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private Transform topAnchorPoint;
    private KitchenObject currentKichenObject = null;


    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            if (currentKichenObject == null)
            {
                (sender as PlayerController).PushKitchenObjectOnCounter(this);
            }
            else
            {
                //如果玩家非空手并且柜台上放的是盘子,那么尝试将玩家手里的东西放入盘子中
                if (currentKichenObject.isPlatesObject && (sender as PlayerController).IsHandKichenObject())
                {
                    (currentKichenObject as PlatesObject).
                        AddKitchenObject((sender as PlayerController).GetCurrentKichenObject());
                }
                else
                {
                    (sender as PlayerController).GetKichenObject(currentKichenObject, this);
                }
            }
        }
    }

    public void ClearKichenObject(KitchenObject clearKichenObject)
    {
        if (clearKichenObject == currentKichenObject)
        {
            //移除对其的引用，物品已经给到了其他持有者，这里不应该操作currentKichenObject
            currentKichenObject = null;
        }

    }

    public void GetNewKichenObject(KitchenObject newKichenObject)
    {
        currentKichenObject = newKichenObject;
        currentKichenObject.InitData(this);
    }

    public Transform GetTopAnchorPoint() { return topAnchorPoint; }
}
