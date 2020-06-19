using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // mode determining whether the spawner have to open a map or have to create a new one
    public bool isLoadMode = false;

    public GameObject FTButton;
    public GameObject CalloutButton;

    void Start()
    {        
    }

    void Update()
    {
        // if the placement is chosen and the spawner becomes active
        if (gameObject.activeInHierarchy) {
            // if it needs to create a new map 
            if (!isLoadMode) {
                GameObject newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/MindMap", typeof(GameObject)));
                newMindMap.transform.position = gameObject.transform.position;
                newMindMap.transform.rotation = gameObject.transform.rotation;
                newMindMap.SetActive(true);
                FTButton.SetActive(true);
                CalloutButton.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
