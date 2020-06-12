using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject elements;

    private void Start()
    {
        elements = transform.GetChild(0).gameObject;        
    }

    public void StartNewMap()
    {
        elements.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
