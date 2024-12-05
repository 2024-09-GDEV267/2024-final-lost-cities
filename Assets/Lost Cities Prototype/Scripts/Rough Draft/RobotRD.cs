using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RobotRD : MonoBehaviour
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
    public GameObject Game;
    private GameMaster game_script;

    public List<GameObject> slots;


    public void Awake()
    {
        game_script = Game.GetComponent<GameMaster>();
    }

    public String Hand_To_String()
    {
        String hand_string = "";
        foreach(GameObject card_in_hand in cards)
        {
            CardRD card_data = card_in_hand.GetComponent<CardRD>();
            if (hand_string == "")
            {   
                hand_string += card_data;
            } else {
                hand_string += ", " + card_data;
            }
        }
        return hand_string;
    }

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

    public bool Open_Spot_Check()
    {
        if (cards.Count < 8)
        {
            return true;
        }

        return false;
    }

    public void Take_Turn()
    {
        Debug.Log(Hand_To_String());
        Debug.Log("Robot Taking Turn ...");
        //Play or Discard
        Action();

        //Draw card
        GameMaster.S.Robot_Draw();
        Debug.Log(Hand_To_String());
        //End turn
        GameMaster.S.Robot_End_Turn();

    }

    public void Action(){
        GameObject card_object = cards[0];
        cards.RemoveAt(0);
        CardRD card = card_object.GetComponent<CardRD>();

        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                GameMaster.S.Robot_Play(card);
                break;
            case 1:
                GameMaster.S.Robot_Discard(card);
                break;
        }
        has_played = true;
    }

    public void Sort_Hand()
    {
        int count = 0;

        foreach (GameObject card in cards)
        {
            Card script = card.GetComponent<Card>();

            card.transform.SetParent(slots[count].transform);

            card.transform.position = slots[count].transform.position;

            card.SetActive(true);

            count++;
        }

    }
}
