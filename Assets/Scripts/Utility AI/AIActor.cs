using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/*
*If EnemyActor has one of these as decision-maker, that would make it something like a brain, or a personality? 
*All of this code probably shouldnt be duplicated but changing the constant scores and also the action that are 
*evaluated could be useful? 
*/

public class AIActor : MonoBehaviour
{
    //const base scores
    [SerializeField] private const float BASE_ATTACK_WEIGHT = 0.5f;
    [SerializeField] private const float BASE_HEAL_CURVE = 0.5f;

    List<KeyValuePair<float, ActionType>> actionScores = new List<KeyValuePair<float, ActionType>>();
    private Actor agent;
    private void Start()
    {
        agent = GetComponent<Actor>();
    }
    public ActionType ChooseAction()
    {
        actionScores.Clear();
        float total = 0;

        float score = ScoreAttack();
        total += score;
        actionScores.Add(new KeyValuePair<float, ActionType>(score, ActionType.Attack));

        score = ScoreHeal();
        total += score;
        actionScores.Add(new KeyValuePair<float, ActionType>(score, ActionType.Heal));

        score = ScoreEndTurn();
        total += score;

        actionScores.Sort((x, y) => (y.Key.CompareTo(x.Key)));

        foreach (KeyValuePair<float, ActionType> kv in actionScores)
            Debug.Log(kv.Key + ", " + kv.Value);

        return actionScores.ElementAt(0).Value;
    }
    private float ScoreAttack()
    {
        float score = BASE_ATTACK_WEIGHT;
        return score;
    }
    private float ScoreHeal()
    {
        return  (100 - agent.GetHealth()) / 100 ;
    }
    private float ScoreEndTurn()
    {
        return 1;
    }
   /* private Dictionary<float, ActionType> SortingFunction(Dictionary<float, ActionType> actionScores)
    {
   
    }*/
}
