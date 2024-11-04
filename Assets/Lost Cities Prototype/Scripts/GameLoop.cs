using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
    {
        public string player_name = "";
        public List<GameObject> hand = new List<GameObject>(8);

        public void Start(){
           hand.Add(null);
        }
        public void Add_Card_To_Hand(GameObject card)
        {
            if (hand.Count < 8)
            {
                hand.Add(card);
            } 
        }
        public void print_hand()
        {
            string hand_string = "";
            foreach (GameObject card in hand)
            {
                hand_string += card.ToString() + ", ";
            }
            Debug.Log(player_name + " Current Hand: " + hand_string);
        }
    }

public class GameLoop : MonoBehaviour
{

    public GameObject actionButtons;
    public GameObject drawButton;
    public GameObject endTurnButton;

    public Player active_player;
    public Player player_1;
    public Player player_2;
    public Deck deck_prefab;
    public Deck deck;
    public void Start()
    {
        deck = Instantiate(deck_prefab);
        Player player_1 = new Player();
        player_1.player_name = "Player 1";
        Player player_2 = new Player();
        player_2.player_name = "Player 2";

        active_player = player_1;
        Debug.Log("Active Player: " + active_player.player_name);
    }
    public void play_card()
    {
        Debug.Log(active_player.player_name + " played a card");
        actionButtons.SetActive(false);
        drawButton.SetActive(true);
    }
    public void discard_card()
    {
        Debug.Log(active_player.player_name + " discarded a card");
        actionButtons.SetActive(false);
        drawButton.SetActive(true);
    }
    public void draw()
    {
        GameObject drawn_card = deck.Draw_From_Deck();
        Debug.Log(active_player.player_name + " drew " + drawn_card.ToString());
        active_player.Add_Card_To_Hand(drawn_card);
        active_player.print_hand();
        drawButton.SetActive(false);
        endTurnButton.SetActive(true);
    }
    public void end_turn()
    {
        Debug.Log(active_player.player_name + " ended their turn");
        if(active_player == player_1) {
            active_player = player_2;
        } else {
            active_player = player_1;
        endTurnButton.SetActive(false);
        actionButtons.SetActive(true);
        }
    }
}
