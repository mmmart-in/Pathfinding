using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Cell_Data : ScriptableObject
{
    [SerializeField] private Sprite tileReachable;
    [SerializeField] private Sprite tileHover;
    [SerializeField] private Sprite tileSelected;
    [SerializeField] private Sprite tileNotWalkable;
    [SerializeField] private Sprite tileInRange;

    public Sprite GetTileReachable() { return tileReachable; }
    public Sprite GetTileHover() { return tileHover; }
    public Sprite GetTileSelected() { return tileSelected; }
    public Sprite GetTileNotWalkable(){return tileNotWalkable; }
    public Sprite GetTileInRange() { return tileInRange; }
}
