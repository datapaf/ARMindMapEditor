using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour
{
    public GameObject mapNameInputField;
    public Sprite FlatModeButtonSprite;
    public Sprite VolumeModeButtonSprite;

    void Update()
    {
        if (GameObject.FindObjectOfType<MindMap>())
        {
            MindMap mindMap = GameObject.FindObjectOfType<MindMap>();

            if (mindMap.mode == DemonstrationMode.Flat)
            {
                transform.Find("ModeButton").GetComponent<Image>().sprite = VolumeModeButtonSprite;
            }
            else if (mindMap.mode == DemonstrationMode.Volume)
            {
                transform.Find("ModeButton").GetComponent<Image>().sprite = FlatModeButtonSprite;
            }
        }
    }

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

        if (doExist == false && mapNameInputField.GetComponent<InputField>().text != "")
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
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeDemonstrationMode()
    {
        MindMap mindMap = GameObject.FindObjectOfType<MindMap>();
        if (mindMap.mode == DemonstrationMode.Flat)
        {
            mindMap.mode = DemonstrationMode.Volume;
        }
        else if (mindMap.mode == DemonstrationMode.Volume)
        {
            mindMap.mode = DemonstrationMode.Flat;
        }

        GameObject.Find("SaveController").GetComponent<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
    }

}
