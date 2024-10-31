using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player: MonoBehaviour
    {
        public string player_name = "";
        public Card[] hand = new Card[8];
    }

public class GameLoop : MonoBehaviour
{

    public GameObject actionButtons;
    public GameObject drawButton;
    public GameObject endTurnButton;

    private string player_name = "Player 1";

    public void play_card()
    {
        Debug.Log(player_name + " played a card");
        actionButtons.SetActive(false);
        drawButton.SetActive(true);
    }
    public void discard_card()
    {
        Debug.Log(player_name + " discarded a card");
        actionButtons.SetActive(false);
        drawButton.SetActive(true);
    }
    public void draw()
    {
        Card drawn_card = deck.Draw_from_deck();
        Debug.Log(player_name + " drew " + drawn_card.ToString());
        drawButton.SetActive(false);
        endTurnButton.SetActive(true);
    }
    public void end_turn()
    {
        Debug.Log(player_name + " ended their turn");
        if(player_name.Equals("Player 1")) {
            player_name = "Player 2";
        } else {
            player_name = "Player 1";
        }
        endTurnButton.SetActive(false);
        actionButtons.SetActive(true);
    }
}
