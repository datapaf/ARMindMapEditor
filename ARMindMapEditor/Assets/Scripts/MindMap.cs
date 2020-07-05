using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DemonstrationMode { Volume, Flat };

public class MindMap : MonoBehaviour
{
    
    public string mapName;

    // sizeMultiplier is the scalar that determines the size of all the objects
    public float sizeMultiplier = 1;

    // the variable determines the preview mode
    public bool isPreview = false;

    public DemonstrationMode mode;

    void Start()
    {
        if (GameObject.Find("Editor Menu"))
        {
            GameObject.Find("Editor Menu").transform.Find("InputField").GetComponent<InputField>().text = mapName;
        }
    }

    void Update()
    {
    }
}
