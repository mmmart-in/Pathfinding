using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;
    //Sizes are interchangable and so are coordinates (IN THIS CASE), i dont really need a reference to the Unity grid here but i couldnt be bothered writing 
    //the method for finding coordinates from world position in Grid_Custom 
    public Grid_Custom grid;
    private void Awake()
    {
        instance = this;
    }

    //Convert world position to respective cells
    public List<Grid_Cell> FindPath(Vector3 startPos, Vector3 targetPos)
    { 
        Vector3Int startCell = grid.WorldToTilePosition(startPos);
        Grid_Cell start = grid.cellArray[startCell.x, startCell.y];
    
        Vector3Int targetCell = grid.WorldToTilePosition(targetPos);
        Grid_Cell target = grid.cellArray[targetCell.x, targetCell.y];

        return FindPath(start, target);
    }
    public List<Grid_Cell> FindPath(Grid_Cell start, Grid_Cell target)
    { 
        List<Grid_Cell> open = new List<Grid_Cell>();
        HashSet<Grid_Cell> closed = new HashSet<Grid_Cell>();
        open.Add(start);

        while(open.Count > 0)
        {
            Grid_Cell current = open[0];
            for(int i = 0; i < open.Count; i++)
            {
                if(open[i].fCost < current.fCost || open[i].fCost == current.fCost && open[i].hCost < current.hCost)
                {
                    current = open[i];
                }
            }
            //if we have evaluated a node as 'current', it should never be evaluated again
            open.Remove(current);
            closed.Add(current);

            if (current == target)
            {
                return RetracePath(start, target);                
            }
           
            foreach(Grid_Cell neighbour in grid.GetNeighbours(current))
            {
                if(!neighbour.IsWalkable() || closed.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = current.gCost + GetDistance(current, neighbour);
                if(newMovementCostToNeighbour < neighbour.gCost || !open.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, target);
                    neighbour.parentCell = current;

                    if (!open.Contains(neighbour))
                        open.Add(neighbour);
                }
            }
        }
        return null;
    }
    public bool HasPath(Vector3 startPos, Vector3 targetPos)
    {
        if (FindPath(startPos, targetPos) != null)
            return true;
        return false;
    }
    private List<Grid_Cell> RetracePath(Grid_Cell startCell, Grid_Cell endCell)
    {
        List<Grid_Cell> path = new List<Grid_Cell>();
        Grid_Cell currentCell = endCell;

        while (currentCell != startCell)
        {
            path.Add(currentCell);
            currentCell = currentCell.parentCell;
        }
        path.Reverse();

        return path;
    }
    public int GetDistance(Grid_Cell cellA, Grid_Cell cellB)
    {
        int distX = Mathf.Abs(cellA.gridX - cellB.gridX);
        int distY = Mathf.Abs(cellA.gridY - cellB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }


    #region debug
    public void ColorPath(List<Grid_Cell> path)
    {
        foreach (Grid_Cell g in path)
            g.SetTileSelected();
    }
    #endregion

}
