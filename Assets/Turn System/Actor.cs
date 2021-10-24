using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ActionType
{
    Attack,
    Heal,
    EndTurn,
    Error
}

public abstract class Actor : MonoBehaviour
{
    [Header("Actor attributes")]
    [SerializeField] protected int attackDamage;
    [SerializeField] protected int attackRange;
    [SerializeField] protected int maxMovementPoints;
    [SerializeField] protected int maxActionPoints;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int currentMovementPoints;
    [SerializeField] protected int currentActionPoints;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;

    private HealthBar healthBar;


    //Grid references
    [Header("Grid References")]
    [SerializeField] protected Grid_Custom grid;
    [SerializeField] protected Grid_Cell occupiedTile;

    //MoveCharacter
    protected Vector3 targetPosition;
    protected Coroutine moveCharacter;
    private bool reachedNextCell = true;

    //Debug
    public int startX, startY;

    protected void Start()
    {   //Debug
        transform.position = grid.GetCellCenter(startX, startY);
        targetPosition = transform.position;

        healthBar = GetComponentInChildren<HealthBar>();
        occupiedTile = grid.WorldToCell(transform.position);
        occupiedTile.Occupy(this);
    }
    protected IEnumerator ProcessPath(List<Grid_Cell> path)
    {
        StartCoroutine(ProcessPath(path, null, null));
        yield return null;
    }
    protected IEnumerator ProcessPath(List<Grid_Cell> path, Action<Actor> actionOnComplete, Actor target)
    {
        //no path found
        if (path == null)
        {
            Debug.Log("Path is null");
            yield break;
        }
        
        /*
         If we try to stop only this coroutine (allowing the MoveToNextCell to finish), and thereby allowing clicks in the middle of travelling a path,
        2 instances of MoveToNextCell will both move the transform and neither will ever reach its' target position, effectively freezing unity
         */ 

        for (int i = 0; i < path.Count;)
        {
            if (currentMovementPoints <= 0)
                break;

            if (reachedNextCell)
            {
                StartCoroutine(MoveToNextCell(path[i]));
                yield return new WaitForSeconds(0.5f);
                i++;
                currentMovementPoints--;
            }
        }
        actionOnComplete?.Invoke(target);
        yield return null;
    }
    private IEnumerator MoveToNextCell(Grid_Cell nextCell)
    {
        OccupyTile(nextCell);
        reachedNextCell = false;
        targetPosition = grid.GetCellCenter(nextCell);

        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        reachedNextCell = true;       
    }

    #region Actions
    //When more actors are introduced we need to store them all somewhere, and from this set check which is closest, 
    // or which is more enticing to attack, this of course to set the target of the chosen Action (if it has a target)

    //Overloading so that the enemy actor doesnt need its own abilities for now
    // the plan is that all of these calls should use the TryAttack(actor, ability)
    protected virtual void TryAttack(Actor target, Ability ability)
    {   
        if (IsInRange(target.occupiedTile))
        {
            Debug.Log("Attacking");
            target.TakeDamage(ability.Damage);
        }
        else
            Debug.Log("Target not in range!");
    }
    protected virtual void TryAttack(Actor target)
    {    //is target in range && am satisfied with position => attack
        //Move to target or move closer to target
        //Am i in range now? => attack
        if (IsInRange(target.occupiedTile))
        {
            Debug.Log("Attacking");
            target.TakeDamage(attackDamage);
        }
        else
            Debug.Log("Target not in range!");
    }


    //Obsolete
    protected virtual void PerformAttack(Actor target)
    {
        Debug.Log("Attacking");
        target.TakeDamage(attackDamage);
    }

    protected virtual void Heal() 
    {
        int healAmount = 20;
        healthBar.ChangeHealthValue(healAmount);
        health += healAmount; 
    }
    protected bool IsInRange(Grid_Cell targetCell)
    {
        return (grid.GetDistanceBetweenCells(occupiedTile, targetCell) <= attackRange);
    }

    #endregion


    public void TakeDamage(float damage)
    {
        healthBar.ChangeHealthValue(-damage);
        health -= damage;
        if (health <= 0)
            Die();
    }
    private void Die()
    {
        //play death animation perhaps
        Destroy(this.gameObject, 1f);
    }
    public virtual void Tick() { }
    public abstract void InitiateTurn();
    public void EndTurn()
    {
        TurnHandler.instance.TurnEnded();
    }
    public void UnoccupyTile()
    {
        occupiedTile.Unoccupy();
        //OccupyTile() sets occupiedTile, so no assignment needs to be done here
    }
    public void OccupyTile(Grid_Cell tileToOccupy)
    {
        UnoccupyTile();
        occupiedTile = tileToOccupy;
        tileToOccupy.Occupy(this);
    }


    //Gets
    public float GetMaxHealth() { return maxHealth; }
    public float GetHealth() { return health; }
    public int GetCurrentMovenmentPoints() { return currentMovementPoints; }
    public int GetCurrentActionPoints() { return currentActionPoints; }


}
