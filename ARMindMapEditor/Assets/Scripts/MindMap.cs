using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public enum DemonstrationMode { Volume, Flat };

public class MindMap : MonoBehaviour
{
    public string prevName = "";
    public string mapName;

    // sizeMultiplier is the scalar that determines the size of all the objects
    public float sizeMultiplier = 1;

    // the variable determines the preview mode
    public bool isPreview = false;

    public DemonstrationMode mode;

    public bool isNew;
    private bool isInputFiledSetup = false;

    void Start()
    {
    }

    void Update()
    {
        if (GameObject.Find("Editor Menu"))
        {
            if (isNew)
            {
                mapName = GenerateNewName();
                prevName = mapName;
                Debug.LogWarning(mapName);
                isNew = false;
            }
            if (isInputFiledSetup == false)
            {
                GameObject.Find("Editor Menu").transform.Find("InputField").GetComponent<InputField>().text = mapName;
                isInputFiledSetup = true;
            }
        }
    }

    public string GenerateNewName()
    {
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.json");

        int number = -1;

        foreach (FileInfo f in fileInfo)
        {
            string mapName = Path.GetFileNameWithoutExtension(f.FullName);
            if (mapName.StartsWith("MyMindMap"))
            {
                if (mapName.Length > 9 )
                {
                    number = Math.Max(number, Convert.ToInt32(mapName.Substring(9)));
                }
                else
                {
                    number = Math.Max(number, 0);
                }
            }
        }

        if (number == -1)
        {
            return "MyMindMap";
        }
        else
        {
            return "MyMindMap" + (number + 1).ToString();
        }
    }
}
