using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Deck : MonoBehaviour
{
    public GameObject deck_object;

    public GameObject card_prefab;

    public List<GameObject> deck;

    public void Initialize_Deck()
    {
        var colours = Enum.GetValues(typeof(Colour));

        foreach (Colour colour in colours)
        {

            for (int value = 1; value <= 10; value++)
            {

                if (value == 1)
                {

                    for (int agreement_counter = 1;  agreement_counter <= 3; agreement_counter++)
                    {

                        Create_Card(colour, value);

                    }
                }

                else
                {

                    Create_Card(colour, value);

                }

            }
        }

        Shuffle(ref deck);

        FindObjectOfType<UIControl>().Update_Deck_Count(deck.Count);

    }

    private void Create_Card(Colour colour, int value)
    {
        // Debug.Log(colour.ToString() + value.ToString());

        // Instantiate but remove out the way

        GameObject temp_card = Instantiate(card_prefab);
        Card card = temp_card.GetComponent<Card>();

        card.holder = Holder.Deck;

        card.Constructor(colour, value);

        temp_card.transform.SetParent(deck_object.transform);

        deck.Add(temp_card);
    }

    public void Shuffle(ref List<GameObject> deck)
    {
        List<GameObject> cards;

        int index;

        cards = new List<GameObject>();

        while (deck.Count > 0)
        {
            index = UnityEngine.Random.Range(0, deck.Count);

            cards.Add(deck[index]);

            deck.RemoveAt(index);
        }

        deck = cards;

    }

    public GameObject Draw_Card()
    {
        GameObject card = deck[0];

        deck.RemoveAt(0);
        
        // Potential Bug here. If card is not accepted the card count will be wrong
        // CHECK HERE IF THAT HAPPENS VVVVV
        FindObjectOfType<UIControl>().Update_Deck_Count(deck.Count);

        return card;
        
    }

    private void OnMouseDown()
    {
        //Logic to check whos turn it is

        // IF State != Draw Return
        FindObjectOfType<Player>().Add_Card(Draw_Card());
    }

}
