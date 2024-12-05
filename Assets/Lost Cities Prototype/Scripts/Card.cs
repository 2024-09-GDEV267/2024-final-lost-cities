using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
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

    [Header("Card Properties")]
    public Colour       colour;
    public int          value;

    [Header("Card Data")]
    public GameObject   card;
    public bool         aggreement;
    public bool         in_hand;
    public bool         being_hovered;
    public Holder       holder;
    public bool         selected;
    public Vector3      hand_position;


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

        // Add Value text
        Text_Value();
        // Add Sprite Art
        Sprite_Art();
        // Add Count Pip

        card = this.transform.gameObject;

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

        // Before Sprite is Added
        switch (colour)
        {
            default:
                Debug.Log("ERROR COLOUR");
                sprite_render.color = Color.magenta;
                break;

            case Colour.Blue:
                sprite_render.color = Color.blue;
                break;

            case Colour.Green:
                sprite_render.color = Color.green;
                break;

            case Colour.White:
                sprite_render.color = Color.white;
                break;

            case Colour.Yellow:
                sprite_render.color = Color.yellow;
                break;

            case Colour.Red:
                sprite_render.color = Color.red;
                break;

        }
    }

    private void OnMouseEnter()
    {
        if (holder == Holder.Player && in_hand && !selected)
        {
            being_hovered = true;

            Vector3 new_position = card.transform.position;

            new_position.y += 1;

            card.transform.position = new_position;
        }
        
    }

    private void OnMouseExit()
    {
        if (being_hovered && !selected)
        {
            being_hovered = false;

            card.transform.position = hand_position;
        }

    }

    public void Select_Position()
    {
        if (holder == Holder.Player && in_hand)
        {
            selected = true;

            Vector3 new_position = card.transform.position;

            new_position.y += 1.2f;

            card.transform.position = new_position;
        }
    }

    public void Return_Position()
    {
        if (holder == Holder.Player && in_hand)
        {
            selected = false;

            card.transform.position = hand_position;
        }
    }

    private void OnMouseDown()
    {
        if (holder == Holder.Player)
        {
            FindObjectOfType<Player>().Select_Card(gameObject);
        }
    }

}
