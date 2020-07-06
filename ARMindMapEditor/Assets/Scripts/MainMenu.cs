using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // entities that must be enabled to go to placement selection
    public GameObject touchController;
    public GameObject placementTools;
    
    // spawners that will be notified about loading a map in case 
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
        // notify cursor to show the opening map
        cursorSpawner.GetComponent<Spawner>().isLoadMode = true;
        cursorSpawner.GetComponent<Spawner>().mapName = mapName;

        // notify objec placer to place the opening map
        objectToPlaceSpawner.GetComponent<Spawner>().isLoadMode = true;
        objectToPlaceSpawner.GetComponent<Spawner>().mapName = mapName;

        GoToPlacementSeletion();
    }

    public void CreateMap()
    {
        // map creation does not demand any additional actions
        GoToPlacementSeletion();
    }

    private void GoToPlacementSeletion()
    {
        // enables all the necessary objects to perform placement selection
        touchController.SetActive(true);
        placementTools.SetActive(true);
        gameObject.SetActive(false);
    }
}
