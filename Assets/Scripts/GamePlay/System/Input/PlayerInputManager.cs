using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Const;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    InputControls playerInput;

    protected override void Awake()
    {
        base.Awake();
        playerInput = new InputControls();
        InitAllEvent();
        SwitchActionMap(PlayerInputActionMap.GamePlayMap);
    }

    public void SwitchActionMap(PlayerInputActionMap playerInputActionMap)
    {
        StopAllActionMap();
        switch (playerInputActionMap)
        {
            case PlayerInputActionMap.GamePlayMap:
                {
                    playerInput.GamePlayMap.Enable();
                    // Cursor.lockState = CursorLockMode.Locked;
                }
                break;
        }
    }

    public void StopAllActionMap()
    {
        playerInput.GamePlayMap.Disable();
    }

    protected override void OnDestroy()
    {
        Event_Interact = null;
        Event_Cooking = null;
        Event_Pause = null;
        base.OnDestroy();
    }

    #region 初始化相关按键事件
    // public event UnityAction<Vector2> event_Move = null;
    public event UnityAction Event_Interact = null;
    public event UnityAction Event_Cooking = null;
    public event UnityAction Event_Pause = null;
    private void InitAllEvent()
    {
        // playerInput.GamePlayMap.Moveaction.performed += (InputAction.CallbackContext context) =>
        // {
        //     Debug.Log(context.ReadValue<Vector2>());
        //     event_Move?.Invoke(context.ReadValue<Vector2>());
        // };

        playerInput.GamePlayMap.Interact.performed += (InputAction.CallbackContext context) =>
        {
            Event_Interact?.Invoke();
        };

        playerInput.GamePlayMap.Cooking.performed += (InputAction.CallbackContext context) =>
        {
            Event_Cooking?.Invoke();
        };

        playerInput.GamePlayMap.Pause.performed += (InputAction.CallbackContext context) =>
        {
            Event_Pause?.Invoke();
        };
    }
    #endregion

    #region 按键状态-通过定义变量与动作表action状态映射
    // public bool Jump => playerInput.Gameplay.Jump.WasPressedThisFrame();
    // public bool StopJumnp => playerInput.Gameplay.Jump.WasReleasedThisFrame();
    public Vector2 moveAxes => playerInput.GamePlayMap.Move.ReadValue<Vector2>();
    // public float AxisX => axes.x;
    // public bool IsMoving => AxisX != 0f;
    #endregion
}
