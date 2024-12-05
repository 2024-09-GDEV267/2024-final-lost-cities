using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum Colour
{
    Blue,
    Green,
    White,
    Yellow,
    Red,
    Null
}

public enum Pile
{
    Deck,
    Human_Hand,
    Robot_Hand,
    Expedition_Plot,
    Expedition_Discard
}

public class CardRD : MonoBehaviour
{
    [Header("Card Art")]
    public Sprite[]     values;
    public Sprite[]     images;

    [Header("Card Parts")]
    public GameObject   value_object;
    public GameObject   value_UD_object;
    public Sprite       value_sprite;
    public GameObject   image_object;
    public Sprite       card_art;
    public GameObject   back_card;

    [Header("Card Properties")]
    public Colour       colour;
    public int          value;

    [Header("Card Data")]
    public GameObject   card;
    public bool         aggreement;
    public Pile         current_pile;

    public void Constructor(Colour colour, int value)
    {
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

        Text_Value();
        Sprite_Art();

        card = this.transform.gameObject;

        Show_Visibility();

    }


    private void Text_Value()
    {
        value_sprite = values[value - 1];


        SpriteRenderer sprite_render = value_object.GetComponent<SpriteRenderer>();
        sprite_render.sprite = value_sprite;

        SpriteRenderer sprite_UD_render = value_UD_object.GetComponent<SpriteRenderer>();
        sprite_UD_render.sprite = value_sprite;
    }


    private void Sprite_Art()
    {
        SpriteRenderer sprite_render = image_object.GetComponent<SpriteRenderer>();

        switch (colour)
        {
            default:
                Debug.Log("ERROR COLOUR");
                sprite_render.color = Color.magenta;
                break;

            case Colour.Blue:
                sprite_render.sprite = images[1];
                break;

            case Colour.Green:
                sprite_render.sprite = images[3];
                break;

            case Colour.White:
                sprite_render.sprite = images[0];
                break;

            case Colour.Yellow:
                sprite_render.sprite = images[3];
                break;

            case Colour.Red:
                sprite_render.sprite = images[4];
                break;

        }
    }

    private void OnMouseUpAsButton()
    {
        GameMaster.S.Select(card);
    }

    public void Show_Visibility()
    {
        if (current_pile == Pile.Human_Hand || current_pile == Pile.Expedition_Discard)
        {
            back_card.SetActive(false);
            value_object.SetActive(true);
            value_UD_object.SetActive(true);
        } else
        {
            back_card.SetActive(true);
            value_object.SetActive(false);
            value_UD_object.SetActive(false);
        }
    }

}
