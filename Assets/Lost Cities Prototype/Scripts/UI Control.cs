using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject player_ui;
    public GameObject deck_ui;

    public Text player_hand_count;
    public Text deck_count;

    // Start is called before the first frame update
    void Start()
    {
        player_hand_count = player_ui.GetComponent<Text>();
        deck_count = deck_ui.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Update_Deck_Count(int count)
    {
        deck_count.text = count.ToString();
    }

    public void Update_Player_Count(int count)
    {
        player_hand_count.text = count.ToString();
    }

}
