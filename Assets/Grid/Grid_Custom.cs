using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grid_Custom : MonoBehaviour
{
    public Grid_Cell[,] cellArray { get; private set; }

    [SerializeField] private Tilemap tilemapWalkable;
    [SerializeField] private Grid_Cell gridCellPrefab;
    [SerializeField] private Cell_Data cellData;

    private int gridSizeX;
    private int gridSizeY;
    private float cellSize;

    private void Awake()
    {
        CreateGrid(10, 10, 1);
    }

    //Grid Creation
    public void CreateGrid(int width, int height, float cellSize)
    {
        this.gridSizeX = width;
        this.gridSizeY = height;
        this.cellSize = cellSize;

        cellArray = new Grid_Cell[width, height];

        for (int x = 0; x < cellArray.GetLength(0); x++)
        {
            for (int y = 0; y < cellArray.GetLength(1); y++)
            {
                //Create/instantiate the grid cell and name it accordingly
                Grid_Cell gridCell = Instantiate(gridCellPrefab, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, Quaternion.identity, transform);
                gridCell.NameCell(x, y);
                cellArray[x, y] = gridCell;

                if (tilemapWalkable.HasTile(new Vector3Int(x, y, 0)))
                {
                    gridCell.SetWalkable(true);
                }
                else
                {
                    gridCell.SetWalkable(false);
                }

            }
        }
    }
    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize;
    }
    //
   

    //Grid Operations
    public List<Grid_Cell> GetNeighbours(Grid_Cell gridCell)
    {
        //Only using vertical and horizontal neighbours here
        List<Grid_Cell> neighbours = new List<Grid_Cell>();
        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                //This check removes diagonal moves, but maybe theres a better way to do it without sullying the code completely
                if (x == 0 || y == 0)
                {
                    int checkX = gridCell.gridX + x;
                    int checkY = gridCell.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(cellArray[checkX, checkY]);
                    }
                }
            }
        }

        return neighbours;
    }
    public Grid_Cell WorldToCell(Vector3 position)
    {
        float x = position.x / cellSize;
        float y = position.y / cellSize;
        return cellArray[(int)x, (int)y];
    }
    public Vector3Int WorldToTilePosition(Vector3 position)
    {
        float x = position.x / cellSize;
        float y = position.y / cellSize;
        return new Vector3Int((int)x, (int)y, 0);
    }
    public Vector3 GetCellCenter(Grid_Cell cell)
    {
        return GetCellCenter(cell.gridX, cell.gridY);
    }
    public Vector3 GetCellCenter(int x, int y)
    {
        return new Vector3((x * cellSize) + cellSize * 0.5f, (y * cellSize) + cellSize * 0.5f, 0);
    }
    public int GetHeight() { return gridSizeY; }
    //Counts distance WITHOUT diagonal movement, GetDistance in Pathfinding includes diagonal movement
    public int GetDistanceBetweenCells(Grid_Cell a, Grid_Cell b)
    {
        return GetDistanceBetweenCells(new Vector3Int(a.gridX, a.gridY, 0), new Vector3Int(b.gridX, b.gridY, 0));
    }
    public int GetDistanceBetweenCells(Vector3Int a, Vector3Int b) 
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);
        return distX + distY; 
    } 

    //
    private bool TileInLineOfSight(Grid_Cell callerCell, Grid_Cell targetCell, LayerMask blockedBy, float distance)
   {
       Vector3 targetPos = GetCellCenter(targetCell.gridX, targetCell.gridY);
       RaycastHit2D cast = Physics2D.Raycast(transform.position, (targetPos - transform.position).normalized, distance,  blockedBy);

       //Debug
       //gizmoTarPos = transform.position + (targetPos - transform.position).normalized * distance;
       return cast.collider == null;
   }
    //Gets
    public Tilemap GetTilemapWalkable() { return tilemapWalkable; }
    public Cell_Data GetCellData()
    {
        return cellData;
    }

    #region ColourGrid
    public void ResetTileTints()
    {
        for (int x = 0; x < cellArray.GetLength(0); x++)
        {
            for (int y = 0; y < cellArray.GetLength(1); y++)
            {
                if (tilemapWalkable.HasTile(new Vector3Int(x, y, 0)))
                    cellArray[x, y].SetTileNoTint();
            }
        }
    }

    // a lot of optimising can be done in this method
    public void ProjectReachableTiles(Grid_Cell callerCell, int movementPoints)
    {
        for (int x = callerCell.gridX - movementPoints; x <= callerCell.gridX + movementPoints; x++)
        {
            for (int y = callerCell.gridY - movementPoints; y <= callerCell.gridY + movementPoints; y++)
            {
                if (!tilemapWalkable.HasTile(new Vector3Int(x, y, 0)))
                    continue;

                if (Pathfinding.instance.FindPath(callerCell, cellArray[x, y]) != null && Pathfinding.instance.FindPath(callerCell, cellArray[x, y]).Count <= movementPoints)
                {
                    if (cellArray[x, y].IsWalkable())
                        cellArray[x, y].SetTileReachable();
                    else
                        cellArray[x, y].SetTileNotWalkable();
                }
            }
        }
    }
    public void ProjectAttackRange(Grid_Cell callerCell, int attackRange, LayerMask blockedBy)
    {
        //loop through possible grid cells
        for (int x = callerCell.gridX - attackRange; x <= callerCell.gridX + attackRange; x++)
        {
            for (int y = callerCell.gridY - attackRange; y <= callerCell.gridY + attackRange; y++)
            {
               
                if (!tilemapWalkable.HasTile(new Vector3Int(x, y, 0)))
                    continue;

                //We can check the distance like this, the question then is if the nested loop can be made smaller
                float distance = GetDistanceBetweenCells(callerCell, cellArray[x, y]);
                if ( distance <= attackRange)
                {
                    if (TileInLineOfSight(callerCell, cellArray[x,y], blockedBy, distance))
                        cellArray[x, y].SetTileInRange();
                }
            }
        }

    }
    #endregion

    #region CodeMonkey_Utils
    public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        //textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        //textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh; 
        
    }
    #endregion
}
