using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject touchController;
    public GameObject placementTools;

    void Start()
    {   
    }

    void Update()
    {
    }

    public void MapCreation()
    {
        touchController.SetActive(true);
        placementTools.SetActive(true);
        gameObject.SetActive(false);
    }
}
