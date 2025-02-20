using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerController : MonoBehaviour, IKitchenObjectParent
{
    private bool CanController = false;
    [SerializeField] private Transform topAnchorGameObject;
    [SerializeField] private float speed = 7f;
    [SerializeField] LayerMask countersLayerMask;
    private Vector2 inputDir = Vector2.zero;
    private Vector3 outputDir = Vector3.zero;
    private PlayerAnimatior playerAnimatior;

    private static PlayerController _Instance = null;
    public static PlayerController Instance
    {
        get
        {
            return _Instance;
        }
        private set { }
    }

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += OnStateChangedAction;
        playerAnimatior = new PlayerAnimatior(GetComponentInChildren<Animator>());
        if (_Instance == null)
        {
            _Instance = this;
        }
    }

    private void OnStateChangedAction(GameManager.GameState state)
    {
        CanController = (state == GameManager.GameState.GamePlaying);
    }

    private void Start()
    {
        PlayerInputManager.Instance.Event_Interact += Interact;
        PlayerInputManager.Instance.Event_Cooking += CookingInteract;
    }

    private void OnDestroy()
    {
        if (PlayerInputManager.Instance)
        {
            PlayerInputManager.Instance.Event_Interact -= Interact;
            PlayerInputManager.Instance.Event_Cooking -= CookingInteract;
        }
    }

    // Update is called once per frame
    public AudioSource audioSource;
    private float footStepTimer = 0f;
    private float footStepTimerMax = 0.15f;
    void Update()
    {
        if (!CanController) return;
        inputDir.x = PlayerInputManager.Instance.moveAxes.x;
        inputDir.y = PlayerInputManager.Instance.moveAxes.y;
        if (inputDir.x != 0 || inputDir.y != 0)
        {
            footStepTimer += Time.deltaTime;
            if (footStepTimer >= footStepTimerMax)
            {
                footStepTimer = 0f;
                audioSource.PlayOneShot(SoundManager.Instance.audioClipRefsSo.footstep[
                               UnityEngine.Random.Range(0, SoundManager.Instance.audioClipRefsSo.footstep.Length)]);
            }
        }
        Move(inputDir);
    }

    private void FixedUpdate()
    {
        if (!CanController) return;
        inputDir.x = PlayerInputManager.Instance.moveAxes.x;
        inputDir.y = PlayerInputManager.Instance.moveAxes.y;
        HandleMovement(inputDir);
    }

    private void Move(Vector2 inputDir)
    {
        if (inputDir.x != 0 || inputDir.y != 0)
        {
            playerAnimatior.SetWalking(true);
            outputDir.x = inputDir.x;
            outputDir.z = inputDir.y;

            if (!CanMove(outputDir))
            {
                return;
            }

            transform.position += outputDir.normalized * speed * Time.deltaTime;
            transform.forward = Vector3.Slerp(transform.forward, outputDir.normalized, Time.deltaTime * 10f);
        }
        else
        {
            playerAnimatior.SetWalking(false);
        }
    }

    private Vector3 tempVec3 = Vector3.zero;
    private bool CanMove(Vector3 inputDir)
    {
        float playerSize = 0.65f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position,
                   transform.position + Vector3.up * playerHeight, playerSize,
                    outputDir, speed * Time.deltaTime);

        //尝试分解向量-应对侧向移动的情况
        if (!canMove)
        {
            if (inputDir.x != 0)
            {
                tempVec3.x = inputDir.x;
                tempVec3.z = 0;
                canMove = !Physics.CapsuleCast(transform.position,
                   transform.position + Vector3.up * 2f, 0.65f,
                    tempVec3, speed * Time.deltaTime);
                if (canMove)
                {
                    outputDir.x = tempVec3.x;
                    outputDir.z = tempVec3.z;
                    return canMove;
                }
            }

            if (inputDir.z != 0)
            {
                tempVec3.x = 0;
                tempVec3.z = inputDir.z;
                canMove = !Physics.CapsuleCast(transform.position,
                   transform.position + Vector3.up * 2f, 0.65f,
                    tempVec3, speed * Time.deltaTime);
                if (canMove)
                {
                    outputDir.x = tempVec3.x;
                    outputDir.z = tempVec3.z;
                    return canMove;
                }
            }

        }

        return canMove;

    }

    private GameObject currentInteraObj = null;
    private void HandleMovement(Vector2 inputDir)
    {
        if (inputDir.x != 0 || inputDir.y != 0)
        {
            tempVec3.x = inputDir.x;
            tempVec3.y = 0;
            tempVec3.z = inputDir.y;

            float interactDistance = 2f;
            Physics.Raycast(transform.position, tempVec3,
             out RaycastHit hitInfo, interactDistance, countersLayerMask);
            if (hitInfo.transform != null &&
                currentInteraObj != hitInfo.transform.gameObject)
            {
                if (currentInteraObj != null)
                {
                    IsShowInteract(currentInteraObj, false);
                }
                currentInteraObj = hitInfo.transform.gameObject;
                IsShowInteract(currentInteraObj, true);
            }
            else if (hitInfo.transform == null)
            {
                IsShowInteract(currentInteraObj, false);
                currentInteraObj = null;
            }
        }
    }

    //交互按钮回调
    private void Interact()
    {
        // Debug.Log("按下了交互按钮 - Interact");

        if (currentInteraObj != null)
        {
            if (currentInteraObj.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                //TODO:补充一个判断条件，如果该容器为存储容器并且手持对应物品不应该再交互
                //避免重复生成
                if (CheckIsPlayerHasSameKitchenObject(baseCounter))
                {
                    AllEventHander.CallOnSelectedCounterChangedEvent(this,
                                   new OnSelectedCounterChangedEventArgs(baseCounter));
                }
                else
                {
                    Debug.Log("已经持有了相同的物体 不在重复生成");
                }

            }
        }
    }

    private void CookingInteract()
    {
        if (currentInteraObj != null)
        {
            if (currentInteraObj.TryGetComponent<BaseCounter>(out BaseCounter baseCounter))
            {
                //TODO:补充一个判断条件，如果该容器为存储容器并且手持对应物品不应该再交互
                //避免重复生成
                if (CheckIsPlayerHasSameKitchenObject(baseCounter))
                {
                    AllEventHander.CallOnCuttingCounterChangedEvent(this,
                                   new OnCuttingCounterChangeEventArgs(baseCounter));
                }
                else
                {
                    Debug.Log("已经持有了相同的物体 不在重复生成");
                }

            }
        }
    }

    private void IsShowInteract(GameObject currentInteraObj, bool isShow = true)
    {
        if (currentInteraObj)
        {
            Transform transform = currentInteraObj.transform.GetChild(1);
            if (transform)
            {
                GameObject child = transform.gameObject;
                child.SetActive(isShow);
            }

        }
    }

    //---------玩家手持厨房物品功能相关
    private KitchenObject currentKichenObject = null;
    public Transform GetTopAnchorPoint()
    {
        return topAnchorGameObject;
    }

    public void GetNewKichenObject(KitchenObject newKichenObject)
    {
        currentKichenObject = newKichenObject;
        currentKichenObject.InitData(this);
        SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.objectPickup,
            transform.position);
    }

    public void ClearKichenObject(KitchenObject clearKichenObject)
    {
        if (clearKichenObject == currentKichenObject)
        {
            //移除对其的引用，物品已经给到了其他持有者，这里不应该操作currentKichenObject
            currentKichenObject = null;
        }
    }

    //是否手持厨房物品
    public bool IsHandKichenObject()
    {
        return currentKichenObject != null;
    }

    //获取玩家当前手持物品
    public KitchenObject GetCurrentKichenObject()
    {
        return currentKichenObject;
    }

    //放置物品函数
    public void PushKitchenObjectOnCounter(IKitchenObjectParent iKitchenObjectParent)
    {
        if (currentKichenObject != null)
        {
            currentKichenObject.ChangeiKitchenObjectParent(iKitchenObjectParent);
            SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.objectDrop,
                transform.position);
        }
    }

    //获取物品
    public void GetKichenObject(KitchenObject newKichenObject, IKitchenObjectParent kitchenObjectParent)
    {
        //如果玩家手持的是盘子
        if (currentKichenObject != null && currentKichenObject.isPlatesObject)
        {
            (currentKichenObject as PlatesObject).AddKitchenObject(newKichenObject);
        }
        else
        {
            if (currentKichenObject != null)
            {
                currentKichenObject.ChangeiKitchenObjectParent(kitchenObjectParent);
            }
            newKichenObject.ChangeiKitchenObjectParent(this);
        }
    }

    //获取物品 - 该函数需要手中没有物品才能调用
    public void GetKichenObject(KitchenObject newKichenObject)
    {
        newKichenObject.ChangeiKitchenObjectParent(this);
    }

    //检查玩家手上是否持有容器柜台能给予的物品
    public bool CheckIsPlayerHasSameKitchenObject(BaseCounter baseCounter)
    {
        if (currentKichenObject != null)
        {
            if (baseCounter is ContainerCounter)
            {
                string PrefabNameame = (baseCounter as ContainerCounter).GetkichenObjectSOName();
                if (PrefabNameame == currentKichenObject.GetGameObject().name)
                {
                    return false;
                }
            }
        }
        return true;
    }

    //玩家丢弃手上持有的厨房对象
    public void DropKichenObject()
    {
        if (currentKichenObject != null)
        {
            currentKichenObject.Clear();
            currentKichenObject = null;
            SoundManager.Instance.PlaySound(SoundManager.Instance.audioClipRefsSo.trash,
                 transform.position);
        }
    }



}
