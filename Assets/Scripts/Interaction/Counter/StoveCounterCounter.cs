using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoveCounterCounter : BaseCounter, IKitchenObjectParent
{

    [SerializeField] private FryingRecipeSO[] allFryingRecipeSO;
    [SerializeField] private Transform topAnchorPoint;
    [SerializeField] private AudioSource audioSource;
    private KitchenObject currentKichenObject = null;


    protected override void Interaction(object sender, OnSelectedCounterChangedEventArgs e)
    {
        if (e.baseCounter == this)
        {
            state = State.Idle;
            if (currentKichenObject == null)
            {
                (sender as PlayerController).PushKitchenObjectOnCounter(this);
                currentFryingRecipeSO = GetFryingRecipeSO();
                if (currentFryingRecipeSO != null)
                {
                    state = State.Frying;
                    OpenFryingVisual();
                }
            }
            else
            {
                CloseFryingVisual();
                (sender as PlayerController).GetKichenObject(currentKichenObject, this);
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

    //---内置一个简单的状态机
    private enum State
    {
        Idle,//闲置状态
        Frying,//生肉
        Fried,//熟肉
        Burned,//焦肉
    }
    private State state = State.Idle;
    //当前使用的煎炸配方
    private FryingRecipeSO currentFryingRecipeSO = null;
    //计时器
    private float fryTimer = 0f;
    private void Update()
    {
        if (currentKichenObject == null) { return; }

        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryTimer += Time.deltaTime;
                bar.fillAmount = fryTimer / currentFryingRecipeSO.FryingTimeMax;
                if (fryTimer > currentFryingRecipeSO.FryingTimeMax)
                {
                    //TODO:这里注意一下Obj的回调-按理说没什么问题
                    currentKichenObject.Clear();
                    currentKichenObject = KitchenObject.CreateKichenObject(currentFryingRecipeSO.output, this);
                    fryTimer = 0;
                    GetFryingRecipeSO();
                    currentFryingRecipeSO = GetFryingRecipeSO();
                    if (currentFryingRecipeSO != null)
                    {
                        state = State.Fried;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                }
                break;
            case State.Fried:
                fryTimer += Time.deltaTime;
                bar.fillAmount = fryTimer / currentFryingRecipeSO.FryingTimeMax;
                if (fryTimer > currentFryingRecipeSO.FryingTimeMax)
                {
                    //TODO:这里注意一下Obj的回调-按理说没什么问题
                    currentKichenObject.Clear();
                    currentKichenObject = KitchenObject.CreateKichenObject(currentFryingRecipeSO.output, this);
                    fryTimer = 0;
                    state = State.Burned;
                    CloseFryingVisual();
                }
                break;
            case State.Burned:
                break;
        }
    }

    //获取对应的煎炸配方
    private FryingRecipeSO GetFryingRecipeSO()
    {
        FryingRecipeSO resultFryingRecipeSO = null;
        if (currentKichenObject != null)
        {
            string currentKichenObjectName = currentKichenObject.
                GetKichenObjectSO().PrefabName;
            foreach (FryingRecipeSO f in allFryingRecipeSO)
            {
                if (f.input.PrefabName == currentKichenObjectName)
                {
                    return f;
                }
            }
        }
        CloseFryingVisual();
        return resultFryingRecipeSO;
    }

    //视觉效果表现补充
    [SerializeField] private GameObject _particleSystem;
    [SerializeField] private GameObject effectGameobj;
    public Canvas canvas;
    public Image bar;

    private void OpenFryingVisual()
    {
        _particleSystem.SetActive(true);
        effectGameobj.SetActive(true);
        bar.fillAmount = 0;
        canvas.enabled = true;
        audioSource.Play();
    }

    private void CloseFryingVisual()
    {
        _particleSystem.SetActive(false);
        effectGameobj.SetActive(false);
        canvas.enabled = false;
        audioSource.Stop();
    }
}
