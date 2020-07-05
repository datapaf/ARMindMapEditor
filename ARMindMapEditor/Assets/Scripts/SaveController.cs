using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private float startTime;
    public float timeTillNextSaving;

    private string prevName = "";

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= timeTillNextSaving)
        {
            // as the script works all the time we save only if there is a mind map in the scene
            if (GameObject.FindWithTag("MindMap"))
            {
                GameObject map = GameObject.FindWithTag("MindMap").gameObject;

                // here we prevent saving the map with changed name as a new map
                if (prevName != map.GetComponent<MindMap>().mapName)
                {
                    File.Delete("Assets/Resources/Maps/" + prevName + ".prefab");
                    //AssetDatabase.Refresh();
                    prevName = map.GetComponent<MindMap>().mapName;
                }

                PrefabUtility.SaveAsPrefabAsset(map, "Assets/Resources/Maps/" + map.GetComponent<MindMap>().mapName + ".prefab");
            }
            startTime = Time.time;
        }
    }
}
