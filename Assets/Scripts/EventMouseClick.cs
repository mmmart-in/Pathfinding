using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EventMouseClick : MonoBehaviour
{
    private DelegateInput delegateInput;
    private InputAction click;

    private delegate void actionToPerform(Vector3 atPosition);
    private actionToPerform loadedAction;
    private Ability loadedAbility;

    private void Awake()
    {
        delegateInput = new DelegateInput(); 
    }
    private void OnEnable()
    {
        click = delegateInput.ClickOnScreen.Click;
        click.Enable();

        delegateInput.ClickOnScreen.Click.performed += OnClick;
        //loadedAction = UseMovement;
    }
    private void OnDisable()
    {
        delegateInput.ClickOnScreen.Click.performed -= OnClick;
        click.Disable();
    }

    private void OnClick(InputAction.CallbackContext obj)
    {
        //We really should be able to send the click position data from the event that the input system generates..
        //i dont know why i cant find the way to do it
        Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        //Execute the currently loaded action at clickPosition
        loadedAction?.Invoke(clickPosition);
    }

    private void UseMovement(Vector3 atPosition)
    {
        //TODO; 
        //What information is actually needed here and how do we retrieve it? 
        //Debug.Log("UseMovement at:" + atPosition);
        PlayableActor owner = GetComponent<PlayableActor>();
        owner.TryMoveTo(atPosition);
    }

    public void UseAbility(Vector3 atPosition)
    {
        //TODO; 
        //How do we cleanly store the ability and its' info? 

        //Debug.Log("UseAbility at:" + atPosition);
        PlayableActor owner = GetComponent<PlayableActor>();
        owner.TryPerformAttack(atPosition, loadedAbility);
    }
    
    //Load delegate with..
    public void LoadMovement() 
    {
        Debug.Log("Load Movement");
        loadedAction = UseMovement;       
    }
    public void LoadAbility(Ability ability)
    {
        Debug.Log("Load Ability" );
        loadedAbility = ability;
        loadedAction = UseAbility;
    }

}
