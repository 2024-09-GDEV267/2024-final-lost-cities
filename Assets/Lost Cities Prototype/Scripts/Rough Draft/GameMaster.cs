using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Order
{
    Human,
    Robot
}

public enum Round
{
    First,
    Second,
    Third,
}

public class GameMaster : MonoBehaviour
{
    public static GameMaster S;

    public Dictionary<Round, Order> Best_of_Three = new();

        [Header("Game State")]
    // NEW: Added discardPiles dictionary to manage discard piles for each expedition color or type
    public Dictionary<string, Stack<GameObject>> discardPiles = new(); 

    [Header("Turn Details")]
    public Order current_turn;
    public Order[] turn_order = new Order[2];
    private bool human_turn_start = false;
    private bool robot_turn_start = false;

    [Header("Object References")]
    public GameObject Deck;
    private DeckRD deck_script;
    public GameObject Human;
    private HumanRD human_script;
    public GameObject Robot;
    private RobotRD robot_script;
    public GameObject action_slot;

    [Header("Expiditions")]
    public GameObject Blue_Expedition;
    private BlueExpeditionRD blue_script;
    public GameObject Green_Expedition;
    private GreenExpeditionRD green_script;
    public GameObject White_Expedition;
    private WhiteExpeditionRD white_script;
    public GameObject Yellow_Expedition;
    private YellowExpeditionRD yellow_script;
    public GameObject Red_Expedition;
    private RedExpeditionRD red_script;

    public GameObject expedition_object;

    [Header("Game Details")]
    public bool last_card_drawn = false;
    public GameObject[] card_slot;
    public GameObject card_selected = null;
    public GameObject robot_slected;
    public Colour discarded_color = Colour.Null;


    private void Awake()
    {
        deck_script = Deck.GetComponent<DeckRD>();
        human_script = Human.GetComponent<HumanRD>();
        robot_script = Robot.GetComponent<RobotRD>();

        blue_script = Blue_Expedition.GetComponent<BlueExpeditionRD>();
        green_script = Green_Expedition.GetComponent<GreenExpeditionRD>();
        white_script = White_Expedition.GetComponent<WhiteExpeditionRD>();
        yellow_script = Yellow_Expedition.GetComponent<YellowExpeditionRD>();
        red_script = Red_Expedition.GetComponent<RedExpeditionRD>();

        S = this;
    }

    private void Start()
    {
     // Coin Flip Scene
        int coin_flip = UnityEngine.Random.Range(0, 2);

        //current_turn = turn_order[coin_flip];
        current_turn = Order.Human;

                foreach (string color in new[] { "Red", "Green", "Blue", "Yellow", "White" })
        {
            discardPiles[color] = new Stack<GameObject>();
        }

        // Deck
        // Create all cards and shuffle

        deck_script.Initialize_Deck();


     // Deal 8 Cards to Human Order
        for (int count = 1; count <= 8; count++)
        {
            GameObject card = deck_script.Draw_Card();

            CardRD cardRD = card.GetComponent<CardRD>();

            cardRD.current_pile = Pile.Human_Hand;

            human_script.Add_Card_to_Hand(card);
        }


     // Deal 8 Cards to Robot Order
        for (int count = 1; count <= 8; count++)
        {
            GameObject card = deck_script.Draw_Card();

            CardRD cardRD = card.GetComponent<CardRD>();

            cardRD.current_pile = Pile.Robot_Hand;

            robot_script.Add_Card_to_Hand(card);
        }

        deck_script.Set_Top_Deck();

        Expedition expedition_script = expedition_object.GetComponent<Expedition>();

        expedition_script.populate_expedition();

        Game_Loop();

    }

    private void Action_Slot_Card(GameObject card)
    {
        CardRD card_script = card.GetComponent<CardRD>();

        if (card_script.current_pile != Pile.Human_Hand) return; 

        if (card_slot[0] == null)
        {
            card_selected.transform.position = action_slot.transform.position;
            card_slot[0] = card;
        } 
        
        else if ( card == card_slot[0])
        {
            human_script.Readd_Card(card_slot[0]);

            card_selected = null;
        }

        else
        {
            human_script.Readd_Card(card_slot[0]);

            card_selected.transform.position = action_slot.transform.position;
            card_slot[0] = card;
        }
        
    }

    private void Game_Loop()
    {
        if (last_card_drawn)
        {
            Score_Scene();
        } else
        {
            switch (current_turn)
            {
                case Order.Human:
                    if (!human_turn_start)
                    {
                        human_turn_start = true;
                        human_script.my_turn = true;

                        Debug.Log("Human Turn");

                        // Enable UI Elements
                    }

                    break;

                case Order.Robot:
                    if (!robot_turn_start)
                    {
                        robot_turn_start = true;
                        robot_script.my_turn = true;

                        Debug.Log("Robot Turn");
                    }

                    // Wait For Outcome
                    robot_script.Take_Turn();


                    break;
            }

        }

    }

    public void Human_End_Turn()
    {
        if (human_script.has_played && human_script.has_drawed)
        {
            UIMasterRD.S.Hide_Draw_Buttons();

            human_turn_start = false;

            human_script.my_turn = false;
            human_script.has_played = false;
            human_script.has_drawed = false;

            discarded_color = Colour.Null;

            current_turn = Order.Robot;
        }

        Game_Loop();
    }

    public void Robot_End_Turn()
    {
        robot_turn_start = false;

        robot_script.my_turn = false;
        robot_script.has_played = false;
        robot_script.has_drawed = false;

        current_turn = Order.Human;

        Game_Loop();
    }

    public void Score_Scene()
    {
        Debug.LogWarning("End Game Scene");
    }
        public GameObject GetTopDiscard(string color)
    {
        if (discardPiles[color].Count > 0)
        {
            return discardPiles[color].Peek();
        }
        return null;
    }

        // NEW: Checks if the discard pile for a given color has any cards
    public bool IsDiscardAvailable(string color)
    {
        return discardPiles[color].Count > 0;
    }

        // NEW: Determines the best discard card for the robot to draw based on an evaluation function
    public GameObject GetBestDiscardForRobot()
    {
        GameObject bestCard = null;
        int bestScore = int.MinValue;

        foreach (var color in discardPiles.Keys)
        {
            if (discardPiles[color].Count > 0)
            {
                GameObject topCard = discardPiles[color].Peek();
                CardRD cardData = topCard.GetComponent<CardRD>();
                int score = EvaluateCardForDraw(cardData);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestCard = topCard;
                }
            }
        }

        return bestCard;
    }

    // NEW: Evaluates the desirability of a card for the robot to draw
    private int EvaluateCardForDraw(CardRD card)
    {
        int score = 0;

        if (robot_script.game_script.IsExpeditionActive(card.color))
        {
            score += card.value; // Prefer higher-value cards for active expeditions
        }
        else
        {
            score -= 10; // Penalize cards requiring a new expedition
        }

        return score;
    }

    // NEW: Handles the robot's draw logic, prioritizing discard piles over the deck
    public void Robot_Draw()
    {
        GameObject cardToDraw = GetBestDiscardForRobot();

        if (cardToDraw != null)
        {
            string color = cardToDraw.GetComponent<CardRD>().color;
            discardPiles[color].Pop(); // Remove the card from the discard pile
            Debug.Log("Robot drew from discard pile: " + cardToDraw);
        }
        else
        {
            cardToDraw = deck_script.Draw_Card(); // Draw from the deck if no useful discard is found
            Debug.Log("Robot drew from the deck: " + cardToDraw);
        }

        robot_script.Add_Draw_to_Hand(cardToDraw);
    }
}

    public void Select(GameObject card)
    {
        CardRD card_script = card.GetComponent<CardRD>();

        switch (card_script.current_pile)
        {
            case Pile.Deck:
                break;

            case Pile.Human_Hand:
                card_selected = card;

                Action_Slot_Card(card);
                break;

            case Pile.Robot_Hand:
                break;

            case Pile.Expedition_Plot:
                break;

            case Pile.Expedition_Discard:
                break;
        }


    }

    public void Play_Action()
    {
        if (current_turn != Order.Human) return;

        if (human_script.has_played) return;

        if (card_selected == null) return;

        CardRD card = card_selected.GetComponent<CardRD>();

        switch (card.colour)
        {
            case Colour.Blue:
                Debug.Log("Playing a Blue Card");
                if (!blue_script.Compare_Card(card))
                {
                    return;
                } else
                {
                    card.current_pile = Pile.Expedition_Plot;

                    human_script.Remove_Card(card_selected);

                    blue_script.Human_Add_Card_To_Plot(card_selected);

                    card_selected.SetActive(false);
                    card_selected.transform.parent = null;
                    card_selected.transform.position = Vector3.zero;

                    card_selected = null;
                    card_slot[0] = null;

                    human_script.has_played = true;
                }
                
                break;

            case Colour.Green:
                Debug.Log("Playing a Green Card");
                if (!green_script.Compare_Card(card))
                {
                    return;
                }
                else
                {
                    card.current_pile = Pile.Expedition_Plot;

                    human_script.Remove_Card(card_selected);

                    green_script.Human_Add_Card_To_Plot(card_selected);

                    card_selected.SetActive(false);
                    card_selected.transform.parent = null;
                    card_selected.transform.position = Vector3.zero;

                    card_selected = null;
                    card_slot[0] = null;

                    human_script.has_played = true;
                }
                break;

            case Colour.White:
                Debug.Log("Playing a White Card");
                if (!white_script.Compare_Card(card))
                {
                    return;
                }
                else
                {
                    card.current_pile = Pile.Expedition_Plot;

                    human_script.Remove_Card(card_selected);

                    white_script.Human_Add_Card_To_Plot(card_selected);

                    card_selected.SetActive(false);
                    card_selected.transform.parent = null;
                    card_selected.transform.position = Vector3.zero;

                    card_selected = null;
                    card_slot[0] = null;

                    human_script.has_played = true;
                }
                break;

            case Colour.Yellow:
                Debug.Log("Playing a Yellow Card");
                if (!yellow_script.Compare_Card(card))
                {
                    return;
                }
                else
                {
                    card.current_pile = Pile.Expedition_Plot;

                    human_script.Remove_Card(card_selected);

                    yellow_script.Human_Add_Card_To_Plot(card_selected);

                    card_selected.SetActive(false);
                    card_selected.transform.parent = null;
                    card_selected.transform.position = Vector3.zero;

                    card_selected = null;
                    card_slot[0] = null;

                    human_script.has_played = true;
                }
                break;

            case Colour.Red:
                Debug.Log("Playing a Red Card");
                if (!red_script.Compare_Card(card))
                {
                    return;
                }
                else
                {
                    card.current_pile = Pile.Expedition_Plot;

                    human_script.Remove_Card(card_selected);

                    red_script.Human_Add_Card_To_Plot(card_selected);

                    card_selected.SetActive(false);
                    card_selected.transform.parent = null;
                    card_selected.transform.position = Vector3.zero;

                    card_selected = null;
                    card_slot[0] = null;

                    human_script.has_played = true;
                }
                break;

        }

        Draw_Action();
    }

    public void Discard_Action()
    {
        if (current_turn != Order.Human) return;

        if (human_script.has_played) return;

        if (card_selected == null) return;

        CardRD card = card_selected.GetComponent<CardRD>();

        switch (card.colour)
        {
            case Colour.Blue:
                Debug.Log("Discarding a Blue Card");
                blue_script.Discard_Card(card_selected);

                human_script.Remove_Card(card_selected);

                card.current_pile = Pile.Expedition_Discard;

                card_selected = null;
                card_slot[0] = null;

                human_script.has_played = true;

                discarded_color = Colour.Blue;

                break;

            case Colour.Green:
                Debug.Log("Discarding a Green Card");
                green_script.Discard_Card(card_selected);

                human_script.Remove_Card(card_selected);

                card.current_pile = Pile.Expedition_Discard;

                card_selected = null;
                card_slot[0] = null;

                human_script.has_played = true;

                discarded_color = Colour.Green;
                break;

            case Colour.White:
                Debug.Log("Discarding a White Card");
                white_script.Discard_Card(card_selected);

                human_script.Remove_Card(card_selected);

                card.current_pile = Pile.Expedition_Discard;

                card_selected = null;
                card_slot[0] = null;

                human_script.has_played = true;

                discarded_color = Colour.White;
                break;

            case Colour.Yellow:
                Debug.Log("Discarding a Yellow Card");
                yellow_script.Discard_Card(card_selected);

                human_script.Remove_Card(card_selected);

                card.current_pile = Pile.Expedition_Discard;

                card_selected = null;
                card_slot[0] = null;

                human_script.has_played = true;

                discarded_color = Colour.Yellow;
                break;

            case Colour.Red:
                Debug.Log("Discarding a Red Card");
                red_script.Discard_Card(card_selected);

                human_script.Remove_Card(card_selected);

                card.current_pile = Pile.Expedition_Discard;

                card_selected = null;
                card_slot[0] = null;

                human_script.has_played = true;

                discarded_color = Colour.Red;
                break;

        }

        Draw_Action();
    }

    private void Draw_Action()
    {
        bool blue;
        bool green;
        bool white;
        bool yellow;
        bool red;

        if (discarded_color == Colour.Blue)
        {
            blue = false;
        } else
        {
            blue = blue_script.Discard_Check();
        }

        if (discarded_color == Colour.Green)
        {
            green = false;
        }
        else
        {
            green = green_script.Discard_Check();
        }

        if (discarded_color == Colour.White)
        {
            white = false;
        }
        else
        {
            white = white_script.Discard_Check();
        }

        if (discarded_color == Colour.Yellow)
        {
            yellow = false;
        }
        else
        {
            yellow = yellow_script.Discard_Check();
        }

        if (discarded_color == Colour.Red)
        {
            red = false;
        }
        else
        {
            red = red_script.Discard_Check();
        }


        UIMasterRD.S.Display_Draw_Buttons(blue, green, white, yellow, red);
    }

    public void Draw_Card(Colour colour)
    {
        switch (colour)
        {
            case Colour.Blue:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(deck_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;

            case Colour.Green:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(white_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;

            case Colour.White:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(white_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;

            case Colour.Yellow:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(yellow_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;

            case Colour.Red:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(red_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;

            case Colour.Null:
                if (human_script.Open_Spot_Check())
                {
                    human_script.Add_Draw_to_Hand(deck_script.Draw_Card());

                    human_script.has_drawed = true;
                }

                Human_End_Turn();
                break;
        }

    }

    public void Robot_Play(CardRD card)
    {
        Debug.Log("Robot Played " + card);
    }
    public void Robot_Discard(CardRD card)
    {
        Debug.Log("Robot Discarded " + card);
    }
    public void Robot_Draw()
    {
        GameObject card_to_draw = deck_script.Draw_Card();
        robot_script.Add_Draw_to_Hand(card_to_draw);
        Debug.Log("Robot drew " + card_to_draw);
    }
    public void Robot_End_Turn()
    {
        Debug.Log("Robot Ended Turn");
        robot_turn_start = false;

        robot_script.my_turn = false;
        robot_script.has_played = false;
        robot_script.has_drawed = false;

        current_turn = Order.Human;
    }

}
