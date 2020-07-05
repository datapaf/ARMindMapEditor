using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // mode determining whether the spawner have to open a map or have to create a new one
    public bool isLoadMode = false;
    public string mapName;
    public bool isPreview = false;

    void Start()
    {        
    }

    void Update()
    {
        // if the placement is chosen and the spawner becomes active
        if (gameObject.activeInHierarchy) {

            if (isLoadMode)
            {
                GameObject newMindMap = Instantiate((GameObject)Resources.Load("Maps/" + mapName, typeof(GameObject)));
                if (isPreview)
                {
                    newMindMap.GetComponent<MindMap>().isPreview = true;
                    newMindMap.transform.SetParent(transform.parent);
                }
                newMindMap.transform.position = gameObject.transform.position;
                newMindMap.transform.rotation = gameObject.transform.rotation;
                newMindMap.SetActive(true);
                Destroy(gameObject);
            }
            // if it needs to create a new map 
            else 
            {
                GameObject newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/MindMap", typeof(GameObject)));
                if (isPreview)
                {
                    newMindMap.GetComponent<MindMap>().isPreview = true;
                    newMindMap.transform.SetParent(transform.parent);
                }
                newMindMap.transform.position = gameObject.transform.position;
                newMindMap.transform.rotation = gameObject.transform.rotation;
                newMindMap.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}
