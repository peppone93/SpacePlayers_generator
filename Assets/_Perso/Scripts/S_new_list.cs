using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_new_list : MonoBehaviour
{
    public Canvas canvas_list;
    public Button player_name;
    public Button player_race;
    public Button add_player;
    private string[] races = { "Engineer", "Politic", "Pilot", "Android", "Military" };
    private int player_nb;
    void Start()
    {
        Button new_list;
        new_list = gameObject.GetComponent<Button>();
        new_list.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        clean_old_players();
        create_new_players();
    }
    void clean_old_players()
    {
        foreach (Transform child in canvas_list.transform)
        {
            Destroy(child.gameObject);
        }
    }
    int ranged_random(int start, int end)
    {
        System.Random rnd;

        rnd = new System.Random();
        return (rnd.Next(start, end + 1));
    }
    void create_new_players()
    {
        float pos = 0;
        player_nb = ranged_random(20, 50);
        resize_canvas();
        handle_adding_button();
        Button[] player_name_list = new Button[player_nb];
        Button[] player_race_list = new Button[player_nb];
        System.Random rnd_race = new System.Random();
        for (int i = 0; i < player_nb; i++)
        {
            //Name
            player_name_list[i] = Instantiate<Button>(player_name);
            player_name_list[i].GetComponentInChildren<Text>().text = player_name_list[i].GetComponentInChildren<Text>().text + " " + (i + 1);
            player_name_list[i].transform.SetParent(canvas_list.transform, false);
            player_name_list[i].transform.position = player_name_list[i].transform.position + new Vector3(0f, -pos, 0f);
            //Race
            player_race_list[i] = Instantiate<Button>(player_race);
            //---> assign random race
            int race = rnd_race.Next(0, 5);
            player_race_list[i].tag = races[race];
            player_race_list[i].GetComponentInChildren<Text>().text = player_race_list[i].GetComponentInChildren<Text>().text + races[race];
            //---> canvas information
            player_race_list[i].transform.SetParent(canvas_list.transform, false);
            player_race_list[i].transform.position = player_race_list[i].transform.position + new Vector3(0f, -pos, 0f);
            pos = pos + 0.8f;
        }
    }
    void resize_canvas()
    {
        RectTransform rectangle = canvas_list.GetComponent<RectTransform>();
        rectangle.sizeDelta = new Vector2(rectangle.rect.width, 0.15f + (float)player_nb * 0.7f);
    }
    void handle_adding_button()
    {
        if (player_nb < 50)
            add_player.interactable = true;
        else
            add_player.interactable = false;
    }

    /*------------------Function used to debug percentages------------------ */
    void create_test_players()
    {
        float pos = 0;
        player_nb = 45;
        resize_canvas();
        Button[] player_name_list = new Button[player_nb];
        Button[] player_race_list = new Button[player_nb];
        System.Random rnd_race = new System.Random();
        for (int i = 0; i < player_nb; i++)
        {
            //Name
            player_name_list[i] = Instantiate<Button>(player_name);
            player_name_list[i].GetComponentInChildren<Text>().text = player_name_list[i].GetComponentInChildren<Text>().text + " " + (i + 1);
            player_name_list[i].transform.SetParent(canvas_list.transform, false);
            player_name_list[i].transform.position = player_name_list[i].transform.position + new Vector3(0f, -pos, 0f);
            //Race
            player_race_list[i] = Instantiate<Button>(player_race);
            //---> assign precise race
            if (i < 10)
            {
                player_race_list[i].tag = races[0];
                player_race_list[i].GetComponentInChildren<Text>().text = player_race_list[i].GetComponentInChildren<Text>().text + races[0];
            }
            else if (i >= 10 && i < 25)
            {
                player_race_list[i].tag = races[2];
                player_race_list[i].GetComponentInChildren<Text>().text = player_race_list[i].GetComponentInChildren<Text>().text + races[2];
            }
            else
            {
                player_race_list[i].tag = races[4];
                player_race_list[i].GetComponentInChildren<Text>().text = player_race_list[i].GetComponentInChildren<Text>().text + races[4];
            }
            //---> canvas information
            player_race_list[i].transform.SetParent(canvas_list.transform, false);
            player_race_list[i].transform.position = player_race_list[i].transform.position + new Vector3(0f, -pos, 0f);
            pos = pos + 0.8f;
        }
    }
}
