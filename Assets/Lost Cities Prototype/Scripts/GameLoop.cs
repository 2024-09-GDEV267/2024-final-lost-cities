using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public GameObject deck_object;
    public Deck deck;

    public GameObject player_hand;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        deck = deck_object.GetComponent<Deck>();
        player = player_hand.GetComponent<Player>();

        deck.Initialize_Deck();

        for (int count = 1; count <= 8; count++)
        {
            GameObject card = deck.Draw_Card();

            player.player_cards.Add(card);
        }

        player.First_Eight();
    }
}
