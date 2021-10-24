// GENERATED AUTOMATICALLY FROM 'Assets/DelegateInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @DelegateInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @DelegateInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""DelegateInput"",
    ""maps"": [
        {
            ""name"": ""ClickOnScreen"",
            ""id"": ""cbc12faa-7fe4-43c4-bd34-2d940f3cef73"",
            ""actions"": [
                {
                    ""name"": ""Click"",
                    ""type"": ""Button"",
                    ""id"": ""0a63e8cb-eeba-4fed-ac25-d4203a45344f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""35612099-e8cb-4375-930c-666513ee99da"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Click"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerControls"",
            ""id"": ""38d1ca6f-fbd4-4d07-b795-7344a27f1f80"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""81f42178-dfd9-48ef-a328-e86572688574"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a610a351-e140-4949-9b0e-394f939981ff"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ClickOnScreen
        m_ClickOnScreen = asset.FindActionMap("ClickOnScreen", throwIfNotFound: true);
        m_ClickOnScreen_Click = m_ClickOnScreen.FindAction("Click", throwIfNotFound: true);
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
        m_PlayerControls_Newaction = m_PlayerControls.FindAction("New action", throwIfNotFound: true);
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

    // ClickOnScreen
    private readonly InputActionMap m_ClickOnScreen;
    private IClickOnScreenActions m_ClickOnScreenActionsCallbackInterface;
    private readonly InputAction m_ClickOnScreen_Click;
    public struct ClickOnScreenActions
    {
        private @DelegateInput m_Wrapper;
        public ClickOnScreenActions(@DelegateInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Click => m_Wrapper.m_ClickOnScreen_Click;
        public InputActionMap Get() { return m_Wrapper.m_ClickOnScreen; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClickOnScreenActions set) { return set.Get(); }
        public void SetCallbacks(IClickOnScreenActions instance)
        {
            if (m_Wrapper.m_ClickOnScreenActionsCallbackInterface != null)
            {
                @Click.started -= m_Wrapper.m_ClickOnScreenActionsCallbackInterface.OnClick;
                @Click.performed -= m_Wrapper.m_ClickOnScreenActionsCallbackInterface.OnClick;
                @Click.canceled -= m_Wrapper.m_ClickOnScreenActionsCallbackInterface.OnClick;
            }
            m_Wrapper.m_ClickOnScreenActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Click.started += instance.OnClick;
                @Click.performed += instance.OnClick;
                @Click.canceled += instance.OnClick;
            }
        }
    }
    public ClickOnScreenActions @ClickOnScreen => new ClickOnScreenActions(this);

    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;
    private readonly InputAction m_PlayerControls_Newaction;
    public struct PlayerControlsActions
    {
        private @DelegateInput m_Wrapper;
        public PlayerControlsActions(@DelegateInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_PlayerControls_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
    public interface IClickOnScreenActions
    {
        void OnClick(InputAction.CallbackContext context);
    }
    public interface IPlayerControlsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
