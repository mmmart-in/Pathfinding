using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnHandler : MonoBehaviour
{
    //Is this suitable for a singleton? I really dont know
    public static TurnHandler instance;

    //actorArray to expose this in the inspector, a bit dirty
    //using both of these sets will probably be completely unnecessary and will just muddy the code with conversions from one format to the other
    [SerializeField] Actor[] actorArray;
    private List<Actor> actors = new List<Actor>();


    //Actor to be evaluated
    [SerializeField] private Actor currentActor;
    //index of current actor, maybe theres a better way of tracking this? 
    private int i = 0;

    private void DebugList()
    {
        foreach (Actor a in actors)
            Debug.Log("actor : " + a);
    }
    private void Awake()
    {
        instance = this;

        foreach(Actor a in actorArray)
        {
            actors.Add(a.GetComponent<Actor>());
        }    
        currentActor = actors[0];
        DebugList();
    }
    //TODO
    //Setting up order of turns.. maybe? 
    public List<Actor> CreateTurnOrder()
    {       
        return new List<Actor>();      
    }
    
    //Called from IActor's EndTurn
    public void TurnEnded()
    {
        if (i < actorArray.Length - 1)
            i++;
        else
            i = 0;

        Debug.Log("'i' is : " + i);

        currentActor = actors[i];
        currentActor.InitiateTurn();
    }

    private void Update()
    {
        currentActor.Tick();
    }
}
