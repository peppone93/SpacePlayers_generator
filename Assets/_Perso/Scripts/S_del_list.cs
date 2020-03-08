using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class S_del_list : MonoBehaviour
{
    public Canvas canvas_list;
    public Button add_player;
    void Start()
    {
        Button del_list;
        del_list = gameObject.GetComponent<Button>();
        del_list.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        foreach (Transform child in canvas_list.transform)
        {
            Destroy(child.gameObject);
        }
        RectTransform rectangle = canvas_list.GetComponent<RectTransform>();
        rectangle.sizeDelta = new Vector2(rectangle.rect.width, 0.15f);
        add_player.interactable = true;
    }
}