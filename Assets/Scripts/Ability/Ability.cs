using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ability : ScriptableObject
{
    
    [SerializeField] private float damage;
    [SerializeField] private int attackRange;
    [SerializeField] private int cost;
    public float Damage { get => damage; }
    public int AttackRange { get => attackRange; }
    public int Cost { get => cost; }

}
