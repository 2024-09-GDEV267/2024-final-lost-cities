using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMasterRD : MonoBehaviour
{
    public static UIMasterRD S;

    [Header("Action Buttons")]
    public GameObject play_object;
    public GameObject discard_object;

    [Header("Draw Buttons")]
    public GameObject blue_draw;
    public GameObject green_draw;
    public GameObject white_draw;
    public GameObject yellow_draw;
    public GameObject red_draw;
    public GameObject deck_draw;

    [Header("Score Texts")]
    public GameObject blue_score;
    public GameObject green_score;
    public GameObject white_score;
    public GameObject yellow_score;
    public GameObject red_score;

    private string blue_score_text;
    private string green_score_text;
    private string white_score_text;
    private string yellow_score_text;
    private string red_score_text;

    [Header("Debug")]
    public GameObject robot_skip;
    public GameObject win_message;
    public GameObject robot_action;



    private void Awake()
    {
        S = this;

        blue_score_text = blue_score.GetComponent<Text>().text;

        blue_score_text = "";

        blue_score.GetComponent<Text>().text = blue_score_text;

        green_score_text = green_score.GetComponent<Text>().text;

        green_score_text = "";

        green_score.GetComponent<Text>().text = green_score_text;

        white_score_text = white_score.GetComponent<Text>().text;

        white_score_text = "";

        white_score.GetComponent<Text>().text = white_score_text;

        yellow_score_text = yellow_score.GetComponent<Text>().text;

        yellow_score_text = "";

        yellow_score.GetComponent<Text>().text = yellow_score_text;

        red_score_text = red_score.GetComponent<Text>().text;

        red_score_text = "";

        red_score.GetComponent<Text>().text = red_score_text;
    }

    public void On_Play_Pressed()
    {
        GameMaster.S.Play_Action();
    }

    public void On_Discard_Pressed()
    {
        GameMaster.S.Discard_Action();
    }

    public void Display_Draw_Buttons(bool blue, bool green, bool white, bool yellow, bool red)
    {
        if (blue) blue_draw.SetActive(true);

        if (green) green_draw.SetActive(true);

        if (white) white_draw.SetActive(true);

        if (yellow) yellow_draw.SetActive(true);

        if (red) red_draw.SetActive(true);

        deck_draw.SetActive(true);

    }

    public void Hide_Draw_Buttons()
    {
        blue_draw.SetActive(false);

        green_draw.SetActive(false);

        white_draw.SetActive(false);

        yellow_draw.SetActive(false);

        red_draw.SetActive(false);

        deck_draw.SetActive(false);
    }

    public void Blue_Draw()
    {
        Debug.Log("Blue draw");
        GameMaster.S.Draw_Card(Colour.Blue);
    }

    public void Green_Draw()
    {
        Debug.Log("Green draw");
        GameMaster.S.Draw_Card(Colour.Green);
    }

    public void White_Draw()
    {
        Debug.Log("White draw");
        GameMaster.S.Draw_Card(Colour.White);
    }

    public void Yellow_Draw()
    {
        Debug.Log("Yellow draw");
        GameMaster.S.Draw_Card(Colour.Yellow);
    }

    public void Red_Draw()
    {
        Debug.Log("Red draw");
        GameMaster.S.Draw_Card(Colour.Red);
    }

    public void Deck_Draw()
    {
        Debug.Log("Deck draw");
        GameMaster.S.Draw_Card(Colour.Null);
    }

    public void Skip_Robot()
    {
        GameMaster.S.Robot_End_Turn();
    }

    public void Update_Blue(string message)
    {
        blue_score_text = message;

        blue_score.GetComponent<Text>().text = blue_score_text;
    }

    public void Update_Green(string message)
    {
        green_score_text = message;

        green_score.GetComponent<Text>().text = green_score_text;
    }

    public void Update_White(string message)
    {
        white_score_text = message;

        white_score.GetComponent<Text>().text = white_score_text;
    }

    public void Update_Yellow(string message)
    {
        yellow_score_text = message;

        yellow_score.GetComponent<Text>().text = yellow_score_text;
    }

    public void Update_Red(string message)
    {
        red_score_text = message;

        red_score.GetComponent<Text>().text = red_score_text;
    }

    public void Update_Robot_Text(string message)
    {
        robot_action.SetActive(true);

        robot_action.GetComponent<Text>().text = message;
    }

    public void End_Scene(string winner)
    {
        robot_action.SetActive(false);

        win_message.SetActive(true);

        play_object.SetActive(false);
        discard_object.SetActive(false);

        win_message.GetComponent<Text>().text = winner + " Won!";
    }
}
