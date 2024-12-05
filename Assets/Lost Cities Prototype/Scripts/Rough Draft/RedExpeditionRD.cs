using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedExpeditionRD : MonoBehaviour
{
    [Header("Attributes")]
    public Colour my_color = Colour.Red;

    [Header("Objects")]
    public GameObject expidition_plot;
    public GameObject expidition_top_deck;

    [Header("Plots")]
    public List<GameObject> human_plot;
    public List<GameObject> robot_plot;
    public List<GameObject> expedition_discard;

    public void Set_Top_Deck()
    {
        if (expedition_discard.Count == 0) return;

        foreach (GameObject card_object in expedition_discard)
        {
            card_object.transform.SetParent(expidition_top_deck.transform);

            card_object.transform.position = expidition_top_deck.transform.position;

            card_object.SetActive(false);

            if (card_object == expedition_discard[(expedition_discard.Count) - 1])
            {
                card_object.SetActive(true);
            }
        }
    }

    public bool Compare_Card(CardRD card)
    {
        if (card.colour != my_color) return false;

        if (human_plot.Count == 0) return true;

        foreach (GameObject game in human_plot)
        {
            CardRD compare = game.GetComponent<CardRD>();

            if (card.value == compare.value) return true;

            if (card.value < compare.value) return false;
        }

        return true;
    }

    public void Discard_Card(GameObject card)
    {
        expedition_discard.Add(card);

        Set_Top_Deck();
    }

    public void Human_Add_Card_To_Plot(GameObject card)
    {
        human_plot.Add(card);
    }

    public bool Discard_Check()
    {
        if (expedition_discard.Count == 0)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    public GameObject Draw_Card()
    {
        GameObject card = expedition_discard[(expedition_discard.Count) - 1];

        expedition_discard.RemoveAt((expedition_discard.Count) - 1);

        return card;

    }
}
