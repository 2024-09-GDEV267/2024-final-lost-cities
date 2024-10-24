using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Colour
{
    Blue,
    Green,
    White,
    Yellow,
    Red
}

public class Card : MonoBehaviour
{
    [Header("Card Properties")]
    public Colour       colour;
    public int          value;

    [Header("Card Data")]
    public GameObject   card;
    public bool         aggreement;


    public void Constructor(Colour colour, int value)
    {
        // Work Around
        card.SetActive(false);

        if (value == 1)
        {
            aggreement = true;

            name = colour.ToString() + " Agreement Card";
        }
        else
        {
            name = colour.ToString() + " " + value.ToString();
        }
        

        this.colour = colour;
        this.value = value;

        // Add Sprite Art
        // Add Value text
        // Add Count Pip

        

    }

    private void Text_Value()
    {
        // Change Text to equal value
        // text = value.toString()
    }

}
