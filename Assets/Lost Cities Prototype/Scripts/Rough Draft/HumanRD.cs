using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    Play,
    Draw
}


public class HumanRD : MonoBehaviour
{
    public bool my_turn;

    [Header("Play/Discard, Draw")]
    public Action[] action = new Action[2];
    public Action current_action;

    [Header("Action Checks")]
    public bool has_played;
    public bool has_drawed;

    [Header("Cards in Hand")]
    public List<GameObject> cards;
    public List<GameObject> slots;


    public void Add_Card_to_Hand(GameObject card)
    {
        if (Open_Spot_Check())
        {
            card.transform.SetParent(this.transform);

            cards.Add(card);
        }

        Sort_Hand();

    }

    public void Add_Draw_to_Hand(GameObject card)
    {
        if (Open_Spot_Check() && has_played)
        {
            card.transform.SetParent(this.transform);

            cards.Add(card);
        }

        Sort_Hand();

    }

    public void Sort_Hand()
    {
        int count = 0;

        foreach (GameObject card in cards)
        {
            CardRD script = card.GetComponent<CardRD>();

            script.current_pile = Pile.Human_Hand;

            card.transform.SetParent(slots[count].transform);

            card.transform.position = slots[count].transform.position;

            card.SetActive(true);

            count++;
        }

    }

    public void Readd_Card(GameObject card)
    {
        int count = 0;

        foreach (GameObject object_search in cards) {
            if (object_search == card)
            {
                card.transform.position = slots[count].transform.position;
            }

            count++;
        }
    }

    public bool Open_Spot_Check()
    {
        if (cards.Count < 8)
        {
            return true;
        }

        return false;
    }

    public void Remove_Card(GameObject card)
    {
        cards.Remove(card);
    }

}
