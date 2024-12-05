using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Debug")]
    public GameObject robot_skip;



    private void Awake()
    {
        S = this;
    }

    public void Set_Human_Active()
    {
        
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
}
