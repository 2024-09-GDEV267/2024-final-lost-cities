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

    // Dictionary to track the winner of each round in a Best-of-Three match
    public Dictionary<Round, Order> Best_of_Three = new();

    [Header("Game State")]
    // Dictionary for discard piles keyed by expedition color or type
    public Dictionary<string, Stack<GameObject>> discardPiles = new();

    [Header("Turn Details")]
    public Order current_turn;
    public Order[] turn_order = new Order[2];
    private bool human_turn_start = false;
    private bool robot_turn_start = false;

    [Header("Object References")]
    public GameObject Deck; // Reference to the deck object
    private DeckRD deck_script;
    public GameObject Human; // Reference to the human player object
    private HumanRD human_script;
    public GameObject Robot; // Reference to the robot player object
    private RobotRD robot_script;

    [Header("Game Details")]
    public bool last_card_drawn = false; // Flag indicating the last card has been drawn

    private void Awake()
    {
        deck_script = Deck.GetComponent<DeckRD>();
        human_script = Human.GetComponent<HumanRD>();
        robot_script = Robot.GetComponent<RobotRD>();

        // Singleton pattern for the GameMaster
        S = this;
    }

    void Start()
    {
        // Initialize discard piles for each expedition color
        foreach (string color in new[] { "Red", "Green", "Blue", "Yellow", "White" })
        {
            discardPiles[color] = new Stack<GameObject>();
        }

        // Initialize the deck and shuffle it
        deck_script.Initialize_Deck();

        // Deal 8 cards to the human player
        for (int count = 1; count <= 8; count++)
        {
            GameObject card = deck_script.Draw_Card();
            CardRD cardRD = card.GetComponent<CardRD>();
            cardRD.pile = Pile.Human_Hand; // Mark the card's pile as the human's hand
            human_script.Add_Card_to_Hand(card);
        }

        // Deal 8 cards to the robot player
        for (int count = 1; count <= 8; count++)
        {
            GameObject card = deck_script.Draw_Card();
            CardRD cardRD = card.GetComponent<CardRD>();
            cardRD.pile = Pile.Robot_Hand; // Mark the card's pile as the robot's hand
            robot_script.Add_Card_to_Hand(card);
        }
    }

    private void Update()
    {
        if (last_card_drawn)
        {
            Score_Scene(); // Transition to scoring scene
        }
        else
        {
            switch (current_turn)
            {
                case Order.Human:
                    if (!human_turn_start)
                    {
                        human_turn_start = true;
                        human_script.my_turn = true; // Enable human turn
                        Debug.Log("Human Turn");
                    }

                    // End turn logic for the human player
                    if (human_script.has_played && human_script.has_drawed)
                    {
                        human_turn_start = false;
                        human_script.my_turn = false;
                        human_script.has_played = false;
                        human_script.has_drawed = false;

                        current_turn = Order.Robot; // Switch to robot's turn
                    }
                    break;

                case Order.Robot:
                    if (!robot_turn_start)
                    {
                        robot_turn_start = true;
                        robot_script.my_turn = true; // Enable robot turn
                        Debug.Log("Robot Turn");
                    }

                    // Robot turn logic
                    robot_script.Take_Turn();

                    // End turn logic for the robot
                    if (robot_script.has_played && robot_script.has_drawed)
                    {
                        robot_turn_start = false;
                        robot_script.my_turn = false;
                        robot_script.has_played = false;
                        robot_script.has_drawed = false;

                        current_turn = Order.Human; // Switch to human's turn
                    }
                    break;
            }
        }
    }

    public GameObject GetTopDiscard(string color)
    {
        // Returns the top card from the discard pile of the specified color, if available
        if (discardPiles[color].Count > 0)
        {
            return discardPiles[color].Peek();
        }
        return null;
    }

    public bool IsDiscardAvailable(string color)
    {
        // Checks if the discard pile for a given color has any cards
        return discardPiles[color].Count > 0;
    }

    public MonoBehaviour Current_Turn_Holder()
    {
        // Returns the current player script (human or robot) based on the turn
        return current_turn == Order.Human ? (MonoBehaviour)human_script : robot_script;
    }

    public void Score_Scene()
    {
        // Placeholder for transitioning to the scoring scene
        Debug.LogWarning("End Game Scene");
    }

    public void Robot_Play(CardRD card)
    {
        Debug.Log("Robot Played " + card);
    }

    public void Robot_Discard(CardRD card)
    {
        Debug.Log("Robot Discarded " + card);

        // Add the discarded card to the appropriate discard pile
        discardPiles[card.color].Push(card.gameObject);
    }

    public GameObject GetBestDiscardForRobot()
    {
        GameObject bestCard = null;
        int bestScore = int.MinValue;

        // Evaluate each discard pile to find the best card for the robot to draw
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

    private int EvaluateCardForDraw(CardRD card)
    {
        // Evaluates the desirability of a card for the robot to draw
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
