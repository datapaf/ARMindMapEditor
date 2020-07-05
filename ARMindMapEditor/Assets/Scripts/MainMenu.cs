using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject touchController;
    public GameObject placementTools;
    public GameObject cursorSpawner;
    public GameObject objectToPlaceSpawner;

    void Start()
    {   
    }

    void Update()
    {
    }
    public void OpenMap(string mapName)
    {
        cursorSpawner.GetComponent<Spawner>().isLoadMode = true;
        cursorSpawner.GetComponent<Spawner>().mapName = mapName;

        objectToPlaceSpawner.GetComponent<Spawner>().isLoadMode = true;
        objectToPlaceSpawner.GetComponent<Spawner>().mapName = mapName;

        GameObject.Find("Main Menu").GetComponent<MainMenu>().MapCreation();
    }
    public void MapCreation()
    {
        touchController.SetActive(true);
        placementTools.SetActive(true);
        gameObject.SetActive(false);
    }
}
