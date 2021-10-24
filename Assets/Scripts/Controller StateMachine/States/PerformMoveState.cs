using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PerformMoveState : State
{
    PlayableActor actorOwner;

    protected override void Initialize()
    {
        Debug.Log("MoveState Initialized");
        actorOwner = (PlayableActor)owner;
        Debug.Assert(actorOwner);
    }
    public override void Enter()
    {
        Debug.Log("MoveState enter");
        //ProjectReachableTiles i Enter?
        actorOwner.ProjectReachableTiles();
    }
    public override void RunUpdate()
    {
        //This will probably not run in update, but rather as an event or so
        /*if (Input.GetMouseButtonUp(0))
        {
            //Down the road i want to separate some of this to allow a sort of preview of the path that will be executed when you release the button
            OnMouseClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }*/
    }
    public override void Exit() { }
    private void OnMouseClick(Vector3 clickPosition)
    {
        actorOwner.TryMoveTo(clickPosition);
    }
}
