using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // mode determining whether the spawner have to open a map or have to create a new one
    public bool isLoadMode = false;
    public string mapName = null;
    public bool isPreview = false;
    public bool isNew = true;
    public bool doCreateWithCustomName = false;

    void Start()
    {        
    }

    void Update()
    {
        // if the placement is chosen and the spawner becomes active
        if (gameObject.activeInHierarchy) {

            GameObject newMindMap;

            if (doCreateWithCustomName)
            {
                newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/MindMap", typeof(GameObject)));
                newMindMap.GetComponent<MindMap>().mapName = mapName;
            }
            else if (isLoadMode)
            {
                newMindMap = GameObject.Find("SaveController").GetComponent<SaveController>().LoadMap(mapName);
            }
            else 
            {
                newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/MindMap", typeof(GameObject)));
                // pass the value to the parameter to generate the new name for the new map
                newMindMap.GetComponent<MindMap>().isNew = true;
            }
            
            if (isPreview)
            {
                newMindMap.GetComponent<MindMap>().isPreview = true;
                newMindMap.transform.SetParent(transform.parent, false);
            }
            newMindMap.transform.position = gameObject.transform.position;
            newMindMap.transform.rotation = gameObject.transform.rotation;

            Destroy(gameObject);
        }
    }
}
