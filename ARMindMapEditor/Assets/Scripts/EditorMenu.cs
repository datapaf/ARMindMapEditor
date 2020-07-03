using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour
{
    public GameObject mapNameInputField;

    public void ChangeMapName()
    {
        GameObject mindMap = FindObjectOfType<MindMap>().gameObject;
        mindMap.GetComponent<MindMap>().mapName = mapNameInputField.GetComponent<InputField>().text;
    }
}
