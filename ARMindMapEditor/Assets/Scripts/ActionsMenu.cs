using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsMenu : MonoBehaviour
{
    public GameObject node;

    public GameObject CTActionsMenu;
    public GameObject MTActionsMenu;
    public GameObject SubtopicActionsMenu;
    public GameObject RelationshipActionsMenu;

    private GameObject menu;
    private Slider slider;
    private GameObject inputField;

    private bool isSliderValueSetup;

    void Start()
    {   
    }

    void Update()
    {
    }

    public void ChangeSize()
    {
        if (isSliderValueSetup == false)
        {
            node.GetComponent<Node>().size = slider.value;
        }
        else
        {
            isSliderValueSetup = false;
        }
    }

    public void SetNode(GameObject go)
    {
        node = go;
    }

    public void ShowMenu()
    {
        menu = GetMenu();

        isSliderValueSetup = true;

        slider = menu.transform.Find("Slider").GetComponent<Slider>();
        slider.minValue = node.GetComponent<Node>().minSize;
        slider.maxValue = node.GetComponent<Node>().maxSize;
        slider.value = node.GetComponent<Node>().size;
        
        menu.SetActive(true);
    }

    public void HideMenu()
    {
        menu.SetActive(false);
    }

    GameObject GetMenu()
    {
        if (node.tag == "CentralTopic")
            return CTActionsMenu;
        else if (node.tag == "MainTopic")
            return MTActionsMenu;
        else if (node.tag == "Subtopic")
            return SubtopicActionsMenu;
        else if (node.tag == "Relationship")
            return RelationshipActionsMenu;
        
        return null;
    }

    public void CreateFT()
    {
        GameObject mindMap = GameObject.Find("MindMap(Clone)").gameObject;
        GameObject CT = mindMap.transform.Find("CT").gameObject;
        GameObject CTModel = CT.transform.Find("Sphere(Clone)").gameObject;
        GameObject FT = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));
        FT.transform.SetParent(mindMap.transform, false);
        FT.transform.position = CT.transform.position + new Vector3(0, CTModel.transform.GetChild(0).localScale.y + 0.2f, 0);
        FT.transform.rotation = CT.transform.rotation;
    }

    public void CreateCallout()
    {
        GameObject mindMap = GameObject.Find("MindMap(Clone)").gameObject;
        GameObject CT = mindMap.transform.Find("CT").gameObject;
        GameObject CTModel = CT.transform.Find("Sphere(Clone)").gameObject;
        GameObject FT = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
        FT.transform.SetParent(mindMap.transform, false);
        FT.transform.position = CT.transform.position + new Vector3(0, CTModel.transform.GetChild(0).localScale.y + 0.2f, 0);
        FT.transform.rotation = CT.transform.rotation;
    }

    public void StartChangingText()
    {
        inputField = menu.transform.Find("InputField").gameObject;
        inputField.SetActive(true);
        inputField.GetComponent<InputField>().Select();
        inputField.GetComponent<InputField>().ActivateInputField();

        menu.transform.Find("Buttons").gameObject.SetActive(false);
        menu.transform.Find("Slider").gameObject.SetActive(false);
    }

    public void ChangeText()
    {
        node.transform.Find("Caption").Find("Text").GetComponent<TextMesh>().text = inputField.GetComponent<InputField>().text;
    }

    public void EndChangingText()
    {
        inputField.SetActive(false);
        menu.transform.Find("Buttons").gameObject.SetActive(true);
        menu.transform.Find("Slider").gameObject.SetActive(true);
    }
}
