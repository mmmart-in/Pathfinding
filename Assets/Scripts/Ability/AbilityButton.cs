using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AbilityButton : MonoBehaviour
{
    public Button abilityButton1;
    public Button abilityButton2;

    public List<Ability> abilityList = new List<Ability>();
    public List<Button> abilityButtons = new List<Button>();

    //temp fix/test
    public PlayableActor PlayerActor;
    private EventMouseClick emc;

    private void Start()
    {
        abilityButton1.onClick.AddListener(OnAbility1);
        abilityButton2.onClick.AddListener(OnAbility2);
        emc = PlayerActor.GetComponent<EventMouseClick>();
        AssignAbilityButtons(PlayerActor);
    }

    public void AssignAbilityButtons(PlayableActor actor)
    {
        Debug.Log("abilityList count is : " +PlayerActor.abilityList.Count);

        for(int i = 0; i <= (PlayerActor.abilityList.Count - 1);)
        {
            Debug.Log("i: " + i);
            abilityList.Add(actor.abilityList[i]);
            abilityButtons[i].GetComponentInChildren<Text>().text = abilityList[i].name;
            i++;
        }
    }
    private void OnAbility1()
    {
        Debug.Log("OnAbility1");
        PlayerActor.ProjectAbilityRange(abilityList[0]);
        emc.LoadAbility(abilityList[0]);
    }
    private void OnAbility2()
    {
        Debug.Log("OnAbility2");
        PlayerActor.ProjectAbilityRange(abilityList[1]);
        emc.LoadAbility(abilityList[1]);
    }
}
