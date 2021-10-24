using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PerformActionState : State
{
    PlayableActor actorOwner;

    protected override void Initialize()
    {
        actorOwner = (PlayableActor)owner;
        Debug.Assert(actorOwner);
    }
    public override void Enter() 
    {
        //actorOwner.ProjectAbilityRange();
        Debug.Log("ActionState enter"); }
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
        //actorOwner.TryPerformAttack(/*actionToPerform,*/ clickPosition);

       /* if (walkableTilemap.HasTile(cellPos) && gridArray[cellPos.x, cellPos.y].IsWalkable())
        {
            List<Grid_Cell> path = Pathfinding.instance.FindPath(occupiedTile, gridArray[cellPos.x, cellPos.y]);
            if (path != null && path.Count <= currentMovementPoints)
            {
                grid.ResetTileTints();
                if (moveCharacter != null)
                {
                    StopCoroutine(moveCharacter);
                }
                //customGrid.gridArray[cellPos.x, cellPos.y].SetTileSelected();
                moveCharacter = StartCoroutine(ProcessPath(Pathfinding.instance.FindPath(transform.position, clickPosition)));
            }
        }
        else
            Debug.Log("No tile at: " + cellPos);*/
    }
}
