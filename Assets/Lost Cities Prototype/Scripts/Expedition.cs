using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Expedition : MonoBehaviour
{


    public List<GameObject> cards;
    private DeckRD deck_script;
    public GameObject Deck;

    private void Awake()
    {
        deck_script = Deck.GetComponent<DeckRD>();
    }

    public void populate_expedition()
    {
        if(!cards.Any())
        {
            for (int count = 1; count <= 5; count++)
            {
                GameObject card_to_draw = deck_script.Draw_Card();
                Debug.Log(card_to_draw);
                Add_Card(card_to_draw);
            }
        }
    }
    void Add_Card(GameObject card){
        cards.Add(card);
        Calculate_Score();
    }

    void Calculate_Score(){

        int score = -20;
        int multiplier = 1;
        int total_score = 0;

        foreach(GameObject card in cards) {
            CardRD card_data = card.GetComponent<CardRD>();
            if(card_data.value == 1){
                multiplier++;
            } else {
                score += card_data.value;
            }
        }
        total_score = score * multiplier;
        Debug.Log("Score: " + score + ", Multiplier: " +  multiplier + ", Total Score: " + total_score);
    }
}
