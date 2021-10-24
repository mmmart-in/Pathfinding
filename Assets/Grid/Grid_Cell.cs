using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Cell : MonoBehaviour
{
    public Actor occupyingActor; 
    public SpriteRenderer spriteRenderer { get; private set; }
    public Grid_Cell parentCell { get; set; }
    public int gCost, hCost;

    [SerializeField] private bool walkable = true;
    private Sprite storedSprite;
    private Cell_Data cellData;
  

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cellData = GetComponentInParent<Grid_Custom>().GetCellData();
    }

    //OnMouse relies on box colliders right now. They could also be considered GUI elements to have these functions work(?) but this will do for now
    private void OnMouseEnter()
    {
        if (IsWalkable())
        {
            storedSprite = spriteRenderer.sprite;
            SetTileHover();
        }
        else
            SetTileNotWalkable();
    }
    private void OnMouseExit()
    {
        if (storedSprite != null)
            spriteRenderer.sprite = storedSprite;
        else
            SetTileNoTint();
    }
    public void Occupy(Actor actor)
    {
        walkable = false;
        occupyingActor = actor;
    }
    public void Unoccupy()
    {
        walkable = true;
        occupyingActor = null;
    }
    public void NameCell(int x, int y)
    {
        gameObject.name = "Grid_Cell" + " " + x + ", " + y;
        gridX = x;
        gridY = y;

    }
    public void SetWalkable(bool param) { walkable = param; }
    public Actor GetActor() { return occupyingActor; }

    #region SetTileTints
    public bool IsWalkable()
    {
        return walkable;
    }
    public void SetTileHover() { spriteRenderer.sprite = cellData.GetTileHover(); }
    public void SetTileSelected()
    {
        spriteRenderer.sprite = cellData.GetTileSelected();
    }
    public void SetTileReachable()
    {
        spriteRenderer.sprite = cellData.GetTileReachable();
    }
    public void SetTileNoTint()
    {
        spriteRenderer.sprite = null;
    }
    public void SetTileInRange()
    {
        spriteRenderer.sprite = cellData.GetTileInRange(); 
    }
    public void SetTileNotWalkable()
    {
        spriteRenderer.sprite = cellData.GetTileNotWalkable();
    }
    #endregion

    public int fCost { get { return gCost + hCost; } }
    public int gridX { get; private set; }
    public int gridY { get; private set; }


}
