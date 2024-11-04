using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Deck : MonoBehaviour
{

    public GameObject card_prefab;

    public List<GameObject> deck;


    public void Start()
    { 
        Initialize_Deck();

        Shuffle(ref deck);
    }

    private void Initialize_Deck()
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
        
    }

    private void Create_Card(Colour colour, int value)
    {
        // Debug.Log(colour.ToString() + value.ToString());

        // Instantiate but remove out the way

        GameObject temp_card = Instantiate(card_prefab);
        Card card = temp_card.GetComponent<Card>();

        card.Constructor(colour, value);

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
    public GameObject Draw_From_Deck()
    {
        GameObject card_to_draw = deck[0];
        deck.RemoveAt(0);
        return card_to_draw;
    }

}
