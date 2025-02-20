//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/InputControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControls"",
    ""maps"": [
        {
            ""name"": ""GamePlayMap"",
            ""id"": ""ff52d024-6bbc-4b75-86fc-bcf42e3e259a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f20b8c0b-1ea4-471c-8d80-bb4010b93bf6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""a861a27a-fb09-4e8a-8d2e-983255645979"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cooking"",
                    ""type"": ""Button"",
                    ""id"": ""ea446c70-04ed-42fc-abd1-3f06d0fb8c69"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""ad9dd089-37a0-48c4-8240-9a77819af173"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fc405dde-0082-42aa-b758-0730b259fd8c"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""102f7e1c-db96-4e6e-bdf8-e4df117c3526"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""405592d1-cafa-4ea1-8d1a-e47f60b51544"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""34e0275e-d36e-4f59-adb4-2e0a734dc3fd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""480b0df3-2a88-4b49-ab9b-da3c939841f7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7469f352-f092-4c3d-85be-4cae91e74b09"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2e7e7457-6157-4398-99d0-c104ff9d3558"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cooking"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16f75621-04c1-40a6-a77c-872d024c905c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePlayMap
        m_GamePlayMap = asset.FindActionMap("GamePlayMap", throwIfNotFound: true);
        m_GamePlayMap_Move = m_GamePlayMap.FindAction("Move", throwIfNotFound: true);
        m_GamePlayMap_Interact = m_GamePlayMap.FindAction("Interact", throwIfNotFound: true);
        m_GamePlayMap_Cooking = m_GamePlayMap.FindAction("Cooking", throwIfNotFound: true);
        m_GamePlayMap_Pause = m_GamePlayMap.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // GamePlayMap
    private readonly InputActionMap m_GamePlayMap;
    private List<IGamePlayMapActions> m_GamePlayMapActionsCallbackInterfaces = new List<IGamePlayMapActions>();
    private readonly InputAction m_GamePlayMap_Move;
    private readonly InputAction m_GamePlayMap_Interact;
    private readonly InputAction m_GamePlayMap_Cooking;
    private readonly InputAction m_GamePlayMap_Pause;
    public struct GamePlayMapActions
    {
        private @InputControls m_Wrapper;
        public GamePlayMapActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_GamePlayMap_Move;
        public InputAction @Interact => m_Wrapper.m_GamePlayMap_Interact;
        public InputAction @Cooking => m_Wrapper.m_GamePlayMap_Cooking;
        public InputAction @Pause => m_Wrapper.m_GamePlayMap_Pause;
        public InputActionMap Get() { return m_Wrapper.m_GamePlayMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePlayMapActions set) { return set.Get(); }
        public void AddCallbacks(IGamePlayMapActions instance)
        {
            if (instance == null || m_Wrapper.m_GamePlayMapActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GamePlayMapActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Cooking.started += instance.OnCooking;
            @Cooking.performed += instance.OnCooking;
            @Cooking.canceled += instance.OnCooking;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
        }

        private void UnregisterCallbacks(IGamePlayMapActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Cooking.started -= instance.OnCooking;
            @Cooking.performed -= instance.OnCooking;
            @Cooking.canceled -= instance.OnCooking;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
        }

        public void RemoveCallbacks(IGamePlayMapActions instance)
        {
            if (m_Wrapper.m_GamePlayMapActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGamePlayMapActions instance)
        {
            foreach (var item in m_Wrapper.m_GamePlayMapActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GamePlayMapActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GamePlayMapActions @GamePlayMap => new GamePlayMapActions(this);
    public interface IGamePlayMapActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnCooking(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
