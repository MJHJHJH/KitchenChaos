using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            PlayerController playerController = (sender as PlayerController);
            KitchenObject currentKichenObject = playerController.GetCurrentKichenObject();
            if (currentKichenObject != null && currentKichenObject.isPlatesObject)
            {
                //交付这个盘子
                DeliveryManager.Instance.DeliverRecipe((currentKichenObject as PlatesObject), this);
            }
        }
    }
}
