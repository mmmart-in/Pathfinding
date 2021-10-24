using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor
{
    public void Tick();

    //Setup -New turn
    public void InitiateTurn();
    public void EndTurn();



}
