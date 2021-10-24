using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class EnemyActor : Actor
{
    [SerializeField] private Actor target;
    private AIActor reasoner;


    private void Awake()
    {
        //this is just the upper right corner 
        targetPosition = new Vector3(9.5f, 9.5f, 0f);
        reasoner = GetComponent<AIActor>();
    }
    #region Actor

    public override void InitiateTurn()
    {
        currentActionPoints = maxActionPoints;
        currentMovementPoints = maxMovementPoints;

        ActionType action = reasoner.ChooseAction();
        ExecuteAction(action);

        Debug.Log("Enemy Actor's Turn!");
        EndTurn();
    }
    public new void EndTurn()
    {
        Debug.Log("ending EA turn");
        base.EndTurn();
    }
    
    #region Actions 

    #endregion
    private void ExecuteAction(ActionType action)
    {
        switch (action)
        {
            case ActionType.Attack:
                MoveToAttack();
                Debug.Log("Attack!");
                break;
            case ActionType.Heal:
                Heal();
                Debug.Log("Heal!");
                break;
            case ActionType.Error:
                Debug.Log("Error!");
                break;
        }

    }
    /*public override void Tick()
    {
        if (!logged)
        {
            Debug.Log("Enemy Actor tick!");
            MoveToTargetNeighbour();
            logged = true;
        }
        if (currentMovementPoints <= 0 && notEndedTurn)
        {
            notEndedTurn = false;
            CallEndTurn(2f);
        }
    }*/
    #endregion
    private void MoveToAttack()
    {
        List<Grid_Cell> sortedNeighbourList = GetNeighbourTiles(grid.WorldToCell(target.transform.position));

        for (int i = 0; i < sortedNeighbourList.Count; i ++)
        {
            List<Grid_Cell> pathToNeighbouringTile = Pathfinding.instance.FindPath(occupiedTile, sortedNeighbourList[i]);
            if (pathToNeighbouringTile != null)
            {
                StartCoroutine(ProcessPath(pathToNeighbouringTile, TryAttack, target));
                Pathfinding.instance.ColorPath(pathToNeighbouringTile);
                break;
            }
        }
       
    }
 
    /*
     * ordering the possible neighbour cell from distance (including diagonal), so as to not perform FindPath to more cells than necessary
     * - this way it will try to find a path in ascending distance order effectively finding the closest reachable neighbouring (of the target) tile
     * HOWEVER this can lead to slightly weird behaviour if the target is positioned across unwalkable tiles, because then the closest neighbour
     * might entail moving "around" the target to reach. 
     */
    private List<Grid_Cell> GetNeighbourTiles(Grid_Cell targetTile)
    {
        List<KeyValuePair<float, Grid_Cell>> orderedNeighbours = new List<KeyValuePair<float, Grid_Cell>>();
        //Dictionary<float, Grid_Cell> orderedNeighbours = new Dictionary<float, Grid_Cell>();
        List<Grid_Cell> neighbours = grid.GetNeighbours(targetTile);

        float distance =-1f;
        Grid_Cell closestNeighbour = null;

        for(int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].IsWalkable())
            {
                float distanceToTile = Pathfinding.instance.GetDistance(occupiedTile, neighbours[i]);
                if (distance < 0 || distanceToTile < distance)
                {
                    closestNeighbour = neighbours[i];
                    distance = distanceToTile;                  

                }
                orderedNeighbours.Add(new KeyValuePair<float, Grid_Cell>(distanceToTile, neighbours[i]));
            }

        }
        orderedNeighbours.Sort((x, y) => (y.Key.CompareTo(x.Key)));
        neighbours.Clear();
        //is there a better way to convert this back to a Grid_Cell list? Probably.
        foreach (KeyValuePair<float, Grid_Cell> kv in orderedNeighbours)
            neighbours.Insert(0, kv.Value);

        //log 'neighbours'
       /* foreach (Grid_Cell n in neighbours)
            Debug.Log("neighbours: "  +n.name);*/
        return neighbours;
    }

    private void CallEndTurn(float seconds)
    {
        Invoke("EndTurn", seconds);
    }
}
