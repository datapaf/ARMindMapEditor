using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapButton : MonoBehaviour
{
    public string mapName;

    void Start()
    {
        mapName = transform.GetChild(0).GetComponent<Text>().text;
    }

    void Update()
    {
    }

    public void PressedToOpen()
    {
        GameObject.Find("Main Menu").GetComponent<MainMenu>().OpenMap(mapName);
    }
}
