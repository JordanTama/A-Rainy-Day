// GENERATED AUTOMATICALLY FROM 'Assets/Settings/Input Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""6ccd5ffb-6c7f-4cce-a4a3-214398e4751a"",
            ""actions"": [
                {
                    ""name"": ""Left Click"",
                    ""type"": ""Button"",
                    ""id"": ""324e3011-072c-4cca-aa07-13c770e45767"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse Delta"",
                    ""type"": ""Value"",
                    ""id"": ""f7f5d78f-620a-410b-a38c-221f9a87dcec"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse Position"",
                    ""type"": ""Value"",
                    ""id"": ""336ad89c-b471-40eb-8304-4eb91e0d7c03"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Right Click"",
                    ""type"": ""Button"",
                    ""id"": ""aaaf9b94-0f2a-450a-a17e-774ec53705b3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d2ae50f9-17c5-4414-9050-e0eba4c61cb9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Left Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""87dfedaf-162b-48eb-9660-5ee7787f34f9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8f4a2976-e7c0-464d-a7a6-604efd900fe0"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cc6ca938-1240-4945-a6b8-ddda7730d86c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Right Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_LeftClick = m_Player.FindAction("Left Click", throwIfNotFound: true);
        m_Player_MouseDelta = m_Player.FindAction("Mouse Delta", throwIfNotFound: true);
        m_Player_MousePosition = m_Player.FindAction("Mouse Position", throwIfNotFound: true);
        m_Player_RightClick = m_Player.FindAction("Right Click", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_LeftClick;
    private readonly InputAction m_Player_MouseDelta;
    private readonly InputAction m_Player_MousePosition;
    private readonly InputAction m_Player_RightClick;
    public struct PlayerActions
    {
        private @InputControls m_Wrapper;
        public PlayerActions(@InputControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_Player_LeftClick;
        public InputAction @MouseDelta => m_Wrapper.m_Player_MouseDelta;
        public InputAction @MousePosition => m_Wrapper.m_Player_MousePosition;
        public InputAction @RightClick => m_Wrapper.m_Player_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @MouseDelta.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @MouseDelta.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseDelta;
                @MousePosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @MousePosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMousePosition;
                @RightClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @MouseDelta.started += instance.OnMouseDelta;
                @MouseDelta.performed += instance.OnMouseDelta;
                @MouseDelta.canceled += instance.OnMouseDelta;
                @MousePosition.started += instance.OnMousePosition;
                @MousePosition.performed += instance.OnMousePosition;
                @MousePosition.canceled += instance.OnMousePosition;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnMouseDelta(InputAction.CallbackContext context);
        void OnMousePosition(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}
