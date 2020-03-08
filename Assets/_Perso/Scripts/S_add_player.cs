using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class S_add_player : MonoBehaviour
{
    public  Canvas canvas_list;
    public  Button player_name;
    public  Button player_race;
    private string[] races = { "Engineer", "Politic", "Pilot", "Android", "Military" };
    private int[] race_count = new int[5];
    private float[] race_rate = new float[5];
    private int total_players;
    private float less_common;
    struct st_ranges
    {
        public int range;
        public int race;
    };

    void Start()
    {
        Button add_player;
        add_player = gameObject.GetComponent<Button>();
        add_player.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        Button button;
        int race;

        button = gameObject.GetComponent<Button>();
        count_per_race();
        if (total_players > 48)
            button.GetComponent<Button>().interactable = false;
        new_rate_per_race();
        race = add_semirandom_player();
        put_list_down();
        create_player(race);
    }
    void new_rate_per_race()
    {
        /* Calculates how much percentages decrease in relation of less common and updates ratios */
        less_common = race_count.Min();
        System.Array.Clear(race_rate, 0, 5);
        if (less_common == 0) // switch if a race is absent
        {     
            zero_rate_case(less_common);
            return;
        }
        for (int i = 0; i < 5; i++)
        {
            float lost_value;
            float lost_rate = race_count[i] / less_common * 100f - 100f; // decrement rate (ex. 50%)
            lost_value = (lost_rate >= 100) ? 20 : 20f / 100f * lost_rate; // decrement value (ex. 50% of 20 = 10)
            race_rate[i] = 20f - lost_value;
        }
        percentages_balancer();
    }
    void percentages_balancer()
    {
        /* Ensures that the total race percentage is 100 after decrements*/
        float total_difference;
        int nb_less_common;

        total_difference = 0;
        nb_less_common = 0;
        for (int i = 0; i < 5; i++)
        {
            total_difference += race_rate[i]; // update temporary total
            if (race_count[i] == less_common) // count less common
                nb_less_common++;
        }
        total_difference = 100f - total_difference; // gap 
        total_difference /= nb_less_common; // in case there are two or more 'less common' species
        for (int i = 0; i < 5; i++) // increment less common rate
            if (race_count[i] == less_common)
                race_rate[i] += total_difference;
    }
    void zero_rate_case(float less_common)
    {
        /* Ensures correct percentage when one or more species are absent */
        float zeros;

        zeros = 0;
        for (int i = 0; i < 5; i++)
            if (race_count[i] == 0)
                zeros++;
        for (int i = 0; i < 5; i++)
        {    
            if (race_count[i] == 0)
                race_rate[i] = 100f / zeros; // To more than 1 species at 0
            else
                race_rate[i] = 0;
        }
    }
    int add_semirandom_player()
    {
        st_ranges[] ranges = new st_ranges[5];
        int off_set;

        for (int i = 0; i < 5; i++) // Put rates and races in a struct...*
        {
            ranges[i].range = (int)(race_rate[i] * 10 + 0.5); // Convert rate to int (for probabilty algo) and multiply by 10 to ensure precision
            ranges[i].race = i;
        }
        sort_ranges(ranges); //*... to sort rates without losing races information
        off_set = 0; 
        for (int i = 0; i < 5; i++) // convert rates to offsetted ranges (ex. 20% Salix and 30,5% Tilias = [0 to 200 Salix], [200 to 505 Salix])
        {
            ranges[i].range = off_set + ranges[i].range;
            off_set = ranges[i].range;
        }
        return (random_probability(ranges));
    }
    int random_probability(st_ranges[] ranges)
    {
        /* Generate a random number between 0 and 1000 and use it to pick the correct range */
        System.Random rnd;

        rnd = new System.Random();
        int nb = rnd.Next(0, 1000 + 1);
        if (nb < ranges[0].range && ranges[0].range != 0)
            return (ranges[0].race);
        else if (nb >= ranges[0].range && nb < ranges[1].range && ranges[1].range != 0)
            return (ranges[1].race);
        else if (nb >= ranges[1].range && nb < ranges[2].range && ranges[2].range != 0)
            return (ranges[2].race);
        else if (nb >= ranges[2].range && nb < ranges[3].range && ranges[3].range != 0)
            return (ranges[3].race);
        else
            return (ranges[4].race);
    }
    void count_per_race()
    {
        /* Count races and uptate total*/
        total_players = 0;
        System.Array.Clear(race_count, 0, 5);
        foreach (Transform b_race in canvas_list.transform)
        {
            if (b_race.tag != "Untagged")
            {
                total_players++;
                if (b_race.tag == "Engineer")
                    race_count[0]++;
                else if (b_race.tag == "Politic")
                    race_count[1]++;
                else if (b_race.tag == "Pilot")
                    race_count[2]++;
                else if (b_race.tag == "Android")
                    race_count[3]++;
                else if (b_race.tag == "Military")
                    race_count[4]++;
            }
        }
    }
    void sort_ranges(st_ranges[] ranges)
    {
        for (int j = 0; j < 5; j++)
            for (int i = j + 1; i < 5; ++i)
            {
                if (ranges[j].range > ranges[i].range)
                {
                    int range = ranges[j].range;
                    int race = ranges[j].race;
                    ranges[j].range = ranges[i].range;
                    ranges[j].race = ranges[i].race;
                    ranges[i].range = range;
                    ranges[i].race = race;
                }
            }
    }


    /*---------------------------Interface changes-------------------------- */
    void put_list_down()
    {
        RectTransform rectangle = canvas_list.GetComponent<RectTransform>();
        rectangle.sizeDelta = new Vector2(rectangle.rect.width, rectangle.rect.height + 0.7f);
        foreach (Transform child in canvas_list.transform)
        {
            child.position = child.position + new Vector3(0f, -0.8f, 0f);
        }
    }
    void create_player(int prob_race)
    {
        //Name
        Button player_name_cpy = Instantiate<Button>(player_name);
        player_name_cpy.GetComponentInChildren<Text>().text = player_name_cpy.GetComponentInChildren<Text>().text + " " + (total_players + 1);
        player_name_cpy.transform.SetParent(canvas_list.transform, false);
        //Race
        Button player_race_cpy = Instantiate<Button>(player_race);
        player_race_cpy.tag = races[prob_race];
        player_race_cpy.GetComponentInChildren<Text>().text = player_race_cpy.GetComponentInChildren<Text>().text + races[prob_race];
        player_race_cpy.transform.SetParent(canvas_list.transform, false);
    }


    /*-------------------Functions used to print and debug------------------ */
    void print_race_total()
    {
        print("Salix total: " + race_count[0]);
        print("Gaultherian total: " + race_count[1]);
        print("Vhalrax total: " + race_count[2]);
        print("Ga'Borgah total: " + race_count[3]);
        print("Tilia total: " + race_count[4]);
    }
    void print_race_rate()
    {
        print("Salix rate: " + race_rate[0]);
        print("Gaultherian rate: " + race_rate[1]);
        print("Vhalrax rate: " + race_rate[2]);
        print("Ga'Borgah rate: " + race_rate[3]);
        print("Tilia rate: " + race_rate[4]);
    }
    void print_ranges(st_ranges[] ranges)
    {
        print("ranges " + ranges[0].range + "race: " + ranges[0].race);
        print("ranges " + ranges[1].range + "race: " + ranges[1].race);
        print("ranges " + ranges[2].range + "race: " + ranges[2].race);
        print("ranges " + ranges[3].range + "race: " + ranges[3].race);
        print("ranges " + ranges[4].range + "race: " + ranges[4].race);
    }
}