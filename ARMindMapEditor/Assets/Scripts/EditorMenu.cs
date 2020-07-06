using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            mindMap.GetComponent<MindMap>().prevName = mindMap.GetComponent<MindMap>().mapName;
            mindMap.GetComponent<MindMap>().mapName = mapNameInputField.GetComponent<InputField>().text;
        }
        else
        {
            mapNameInputField.GetComponent<InputField>().text = mindMap.GetComponent<MindMap>().mapName;
        }

        // save the new name
        GameObject.Find("SaveController").GetComponent<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
        Debug.Log("MAP SAVED");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
