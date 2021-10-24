using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayableActor : Actor
{
    public List<Ability> abilityList = new List<Ability>();

    public LayerMask blockedBy;
    //We dont truly need these references but it allows for some short hand in the code
    private Grid_Cell[,] gridArray;
    private Tilemap walkableTilemap;

    //Debug
    Vector3 gizmoTarPos;


    private void Awake()
    {
        //reference setups
        base.Start();
        gridArray = grid.cellArray;
        walkableTilemap = grid.GetTilemapWalkable();
        //stateMachine = new StateMachine(this, states);

    }
    public void TryMoveTo(Vector3 clickPosition)  
    {
        Vector3Int cellPos = grid.WorldToTilePosition(clickPosition);
      
        if (walkableTilemap.HasTile(cellPos) && gridArray[cellPos.x, cellPos.y].IsWalkable())
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
        /*else
            Debug.Log("No tile at: " + cellPos);*/

    }   
    public void TryPerformAttack(Vector3 clickPosition, Ability ability)
    {
        Vector3Int cellPos = grid.WorldToTilePosition(clickPosition);

        if (walkableTilemap.HasTile(cellPos))
        {
            Actor target = grid.WorldToCell(clickPosition).occupyingActor;

            //Cannot use enviromental abilities at the moment, TryAttack takes a target ACTOR; this should be changed if that method is how we want to execute abilities
            if(target)
                TryAttack(target, ability);
        }
    }

  
    public void ProjectReachableTiles()
    {
        grid.ProjectReachableTiles(occupiedTile, currentMovementPoints);
    }
    public void ProjectAbilityRange(Ability a)
    {
        grid.ProjectAttackRange(occupiedTile, a.AttackRange, blockedBy);
    }
    #region Actor  
    public override void InitiateTurn() 
    {
        Debug.Log("player Init turn");
        currentActionPoints = maxActionPoints;
        currentMovementPoints = maxMovementPoints;
    }
    public override void Tick()
    {

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        /* if (Input.GetMouseButtonUp(0))
         {
             //Down the road i want to separate some of this to allow a sort of preview of the path that will be executed when you release the button
             MouseClick(Camera.main.ScreenToWorldPoint(Input.mousePosition));
         }*/ 

       /* if (Input.GetKeyUp(KeyCode.E))
            EndTurn();

        if (Input.GetKeyUp(KeyCode.Escape))
            StopCoroutine(moveCharacter);

        //statemachine tests
        if (Input.GetKeyUp(KeyCode.R))
        {
            grid.ProjectAttackRange(occupiedTile, attackRange, blockedBy);
            stateMachine.ChangeState<PerformActionState>(); 
        }

        if (Input.GetKeyUp(KeyCode.M))
            stateMachine.ChangeState<PerformMoveState>();*/
    }
    public new void EndTurn()
    {
        base.EndTurn();
    }
    #endregion
    #region Debug
    //Debug----------------------------
    private void ResetMovementPoints()
    {
        currentMovementPoints = maxMovementPoints;
        grid.ProjectReachableTiles(occupiedTile, currentMovementPoints);
    }
    public void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, gizmoTarPos);
    }
    #endregion
    #region GridColouring

    //At most one pass through the board should be enough, but im lacking the brain capacity to think up a solution i like right now
    //We should be storing these methods in Grid_Custom?? maybe also TileInLineOfSight & ProjectAttackRange, fix that pls
    /*  private void ResetTileTints()
      {
          for (int x = 0; x < gridArray.GetLength(0); x++)
          {
              for (int y =0; y < gridArray.GetLength(1); y++)
              {
                  if(walkableTilemap.HasTile(new Vector3Int(x,y,0)))
                      gridArray[x, y].SetTileNoTint();
              }
          }
      }
      private void ProjectReachableTiles()
      {
          Vector3Int cellPos = customGrid.WorldToCell(transform.position);

          for(int x = cellPos.x - currentMovementPoints; x <= cellPos.x + currentMovementPoints; x++)
          {
              for (int y = cellPos.y - currentMovementPoints; y <= cellPos.y + currentMovementPoints; y++)
              {
                  if (!walkableTilemap.HasTile(new Vector3Int(x, y, 0)))
                      continue;

                  if (Pathfinding.instance.FindPath(cellPos, new Vector3Int(x, y, 0)) != null && Pathfinding.instance.FindPath(cellPos, new Vector3Int(x, y, 0)).Count <= currentMovementPoints)
                  {
                      if (gridArray[x, y].IsWalkable())
                          customGrid.gridArray[x, y].SetTileReachable();
                      else
                          gridArray[x, y].SetTileNotWalkable();
                  }
              }
          }
      }*/

    //Maybe this will take a parameter of type 'Attack' or similar which holds the range for that certain attack, but that'll have to wait
    /* private void ProjectAttackRange(Grid_Cell occupiedCell, int attackRange)
     {
         Vector3Int cellPos = customGrid.WorldToTilePosition(transform.position);

         //loop through possible grid cells
         for (int x = cellPos.x - attackRange; x <= cellPos.x + attackRange; x++)
         {
             for (int y = cellPos.y - attackRange; y <= cellPos.y + attackRange; y++)
             {
                 Vector3Int tilePos = new Vector3Int(x, y, 0);

                 if (!walkableTilemap.HasTile(tilePos))
                     continue;

                 //We can check the distance like this, the question then is if the nested loop can be made smaller
                 if (customGrid.GetDistanceBetweenCells(cellPos, tilePos) <= attackRange && gridArray[x, y].IsWalkable())
                 {
                     if (TileInLineOfSight(tilePos))
                         customGrid.gridArray[x, y].SetTileInRange();
                 }
             }
         }

     }*/
    /* private bool TileInLineOfSight(Vector3Int targetTile)
    {
        Vector3 targetPos = customGrid.GetCellCenter(targetTile.x, targetTile.y);
        Vector3Int startCell = customGrid.WorldToTilePosition(transform.position);
        float distance = Pathfinding.instance.GetDistance(gridArray[startCell.x, startCell.y], gridArray[targetTile.x, targetTile.y]) / 10;

        RaycastHit2D cast = Physics2D.Raycast(transform.position, (targetPos - transform.position).normalized, distance,  blockingLayers);
       
        //Debug
        gizmoTarPos = transform.position + (targetPos - transform.position).normalized * distance;

        return cast.collider == null;
    }*/

    #endregion
}
