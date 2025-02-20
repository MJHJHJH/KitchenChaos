using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kichenObjectSO;
    [SerializeField] private Transform topAnchorPoint;
    [SerializeField] private Animator animator;
    private const string OPEN_CLOSE = "OpenClose";

    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this && kichenObjectSO != null)
        {
            //TODO:优化点在合适的帧生成物品
            //学习动画的帧事件-最好看下麦扣的勇士里的攻击实现
            if (!(sender as PlayerController).IsHandKichenObject())
            {
                animator.SetTrigger(OPEN_CLOSE);
                KitchenObject.CreateKichenObject(kichenObjectSO, (sender as PlayerController));
            }

        }
    }

    public string GetkichenObjectSOName()
    {
        return kichenObjectSO.PrefabName;
    }
}
