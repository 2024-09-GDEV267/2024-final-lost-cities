using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Holder
{
    Player,
    Opponent,
    Expedition,
    Discard,
    Deck
}

public class Player : MonoBehaviour
{
    public bool has_acted;
    public bool has_drawn;

    public GameObject selected_card;

    public GameObject player_hand;
    public GameObject[] player_slots;
    public List<GameObject> player_cards;

    private void Update()
    {

    }

    public void First_Eight()
    {
        
        Sort_Cards();

        FindObjectOfType<UIControl>().Update_Player_Count(player_cards.Count);
    }

    public void Select_Card(GameObject card)
    {
        if (selected_card != null)
        {
            selected_card.GetComponent<Card>().Return_Position();
        }

        if (selected_card == card)
        {
            selected_card.GetComponent<Card>().Return_Position();

            selected_card = null;
        } else
        {
            selected_card = card;

            selected_card.GetComponent<Card>().Select_Position();
        }

        
    }

    public void Add_Card(GameObject card)
    {
        if (player_cards.Count == 8)
        {
            return;
        }

        player_cards.Add(card);

        FindObjectOfType<UIControl>().Update_Player_Count(player_cards.Count);

        Sort_Cards();
    }

    public void Sort_Cards()
    {
        int count = 0;

        foreach (GameObject card in player_cards)
        {
            Card script = card.GetComponent<Card>();

            script.holder = Holder.Player;
            script.in_hand = true;

            card.transform.position = Vector3.zero;

            card.transform.SetParent(player_slots[count].transform);

            card.transform.position = player_slots[count].transform.position;

            script.hand_position = card.transform.position;

            card.SetActive(true);

            count++;
        }

    }

    public void Card_Placed()
    {
        player_cards.Remove(selected_card);

        selected_card = null;

        FindObjectOfType<UIControl>().Update_Player_Count(player_cards.Count);

        Sort_Cards() ;
    }

}
