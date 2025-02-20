using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CuttingCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private CuttingRecipeSO[] AllcuttingRecipeSO;
    [SerializeField] private Transform topAnchorPoint;
    private KitchenObject currentKichenObject = null;
    //切割进度
    private int cuttingProgress = 0;
    public Canvas canvas;
    public Image bar;
    public Animator animator;

    protected override void Init()
    {
        base.Init();
        AllEventHander.onCuttingCounterChangeEvent += CuttingInteraction;
    }

    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            cuttingProgress = 0;
            bar.fillAmount = 0;
            if (currentKichenObject == null)
            {
                (sender as PlayerController).PushKitchenObjectOnCounter(this);
                canvas.enabled = true;
            }
            else
            {
                canvas.enabled = false;
                (sender as PlayerController).GetKichenObject(currentKichenObject, this);
            }

        }
    }

    private void CuttingInteraction(object sender, OnCuttingCounterChangeEventArgs e)
    {
        if (e.baseCounter == this)
        {
            HanderKichenObjectCutting();
        }
    }

    public void ClearKichenObject(KitchenObject newKichenObject)
    {
        if (newKichenObject == currentKichenObject)
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

    //处理食材切割的函数
    public void HanderKichenObjectCutting()
    {
        if (currentKichenObject != null)
        {
            CuttingRecipeSO result = GetCuttingRecipeSO();
            if (result != null)
            {
                cuttingProgress++;
                animator.SetTrigger("Cut");
                SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.chop,
                    transform.position);
                bar.fillAmount = (float)cuttingProgress / (float)result.needPower;
                if (cuttingProgress >= result.needPower)
                {
                    currentKichenObject.Clear();
                    KitchenObject.CreateKichenObject(result.output, this);
                    canvas.enabled = false;
                }

            }
        }
    }

    //获取配方
    public CuttingRecipeSO GetCuttingRecipeSO()
    {
        CuttingRecipeSO result = null;
        if (currentKichenObject != null)
        {
            KitchenObjectSO target = currentKichenObject.GetKichenObjectSO();
            foreach (CuttingRecipeSO cuttingRecipeSO in AllcuttingRecipeSO)
            {
                if (cuttingRecipeSO.input == target)
                {
                    return cuttingRecipeSO;
                }
            }
        }
        return result;
    }

}

