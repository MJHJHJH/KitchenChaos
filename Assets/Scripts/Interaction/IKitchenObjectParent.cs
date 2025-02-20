//实现该接口的类拥有持有IKitchenObject的能力
using UnityEngine;

public interface IKitchenObjectParent
{
    //获取物体的挂点
    public Transform GetTopAnchorPoint();

    //获取一个新物体流程
    public void GetNewKichenObject(KitchenObject newKichenObject);

    //移除持有物体流程
    public void ClearKichenObject(KitchenObject newKichenObject);
}

//具体流程
//A 将物品转交给 B时
//1.调用A的ClearKichenObject
//=====该流程确保A类中的currentKichenObject引用置空
//2.调用B的GetNewKichenObject
//=====该流程确保B类中的currentKichenObject不为空情况下先Clear 然后再去持有newKichenObject