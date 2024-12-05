using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueExpedition : MonoBehaviour
{
    public GameObject expedition_plot;
    public Colour my_colour = Colour.Blue;

    public GameObject player_object;
    public GameObject oppopnent_object;

    public Player player;

    public List<GameObject> expedition_player_list;
    public List<GameObject> expedition_opponent_list;


    private void Start()
    {
        player = player_object.GetComponent<Player>();
    }

    private void OnMouseDown()
    {
        if (player.selected_card == null) return;

        GameObject card = player.selected_card;

        Card compare_to = card.GetComponent<Card>();

        if (compare_to.colour != my_colour) return;

        player.Card_Placed();

        Add_Card(card);

    }

    public void Add_Card(GameObject card)
    {
        expedition_player_list.Add(card);

        //For Now : Demo
        card.SetActive(false);
    }
}
