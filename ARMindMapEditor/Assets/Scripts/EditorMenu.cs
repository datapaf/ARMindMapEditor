using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour
{
    public GameObject mapNameInputField;

    public void ChangeMapName()
    {
        GameObject mindMap = FindObjectOfType<MindMap>().gameObject;

        bool doExist = false;

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            string mapName = Path.GetFileNameWithoutExtension(file.FullName);
            if (mapName == mapNameInputField.GetComponent<InputField>().text)
            {
                doExist = true;
            }
        }

        if (doExist == false)
        {
            mindMap.GetComponent<MindMap>().mapName = mapNameInputField.GetComponent<InputField>().text;
        }
        else 
        {
            mapNameInputField.GetComponent<InputField>().text = mindMap.GetComponent<MindMap>().mapName;
        }
    }
}
