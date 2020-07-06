using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActionsMenu : MonoBehaviour
{
    public GameObject node;

    public GameObject CTActionsMenu;
    public GameObject MTActionsMenu;
    public GameObject SubtopicActionsMenu;
    public GameObject RelationshipActionsMenu;
    public GameObject FTActionsMenu;
    public GameObject CalloutActionsMenu;


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
            if (node.GetComponent<Node>())
            {
                node.GetComponent<Node>().size = slider.value;
            }
            else 
            {
                node.GetComponent<Callout>().size = slider.value;
            }
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

        if (menu.transform.Find("Slider"))
        {
            isSliderValueSetup = true;

            slider = menu.transform.Find("Slider").GetComponent<Slider>();

            if (node.GetComponent<Node>())
            {
                slider.minValue = node.GetComponent<Node>().minSize;
                slider.maxValue = node.GetComponent<Node>().maxSize;
                slider.value = node.GetComponent<Node>().size;
            }
            else 
            {
                slider.minValue = node.GetComponent<Callout>().minSize;
                slider.maxValue = node.GetComponent<Callout>().maxSize;
                slider.value = node.GetComponent<Callout>().size;
            }
            
        }
        
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
        else if (node.tag == "FloatingTopic")
            return FTActionsMenu;
        else if (node.tag == "Callout")
            return CalloutActionsMenu;

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

        FT.GetComponent<Node>().level = 2;
        FT.GetComponent<Node>().maxSize = CT.GetComponent<Node>().size;
        FT.GetComponent<Node>().minSize = CT.GetComponent<Node>().size * 0.5f;
        FT.GetComponent<Node>().size = CT.GetComponent<Node>().size;
    }

    public void CreateCallout()
    {
        GameObject mindMap = GameObject.Find("MindMap(Clone)").gameObject;
        GameObject CT = mindMap.transform.Find("CT").gameObject;
        GameObject CTModel = CT.transform.Find("Sphere(Clone)").gameObject;
        GameObject Callout = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
        Callout.transform.SetParent(mindMap.transform, false);
        Callout.transform.position = CT.transform.position + new Vector3(0, CTModel.transform.GetChild(0).localScale.y + 0.2f, 0);
        Callout.transform.rotation = CT.transform.rotation;

        Callout.GetComponent<Callout>().level = 2;
        Callout.GetComponent<Callout>().maxSize = CT.GetComponent<Node>().size;
        Callout.GetComponent<Callout>().minSize = CT.GetComponent<Node>().size * 0.5f;
        Callout.GetComponent<Callout>().size = CT.GetComponent<Node>().size;
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
        if (node.GetComponent<Node>())
        {
            node.GetComponent<Node>().text = inputField.GetComponent<InputField>().text;
        }
        else 
        {
            node.GetComponent<Callout>().text = inputField.GetComponent<InputField>().text;
        }
    }

    public void EndChangingText()
    {
        inputField.SetActive(false);
        menu.transform.Find("Buttons").gameObject.SetActive(true);
        menu.transform.Find("Slider").gameObject.SetActive(true);
    }

    public void DeleteSelectedNode()
    {
        // deselect
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        Node.DeleteNode(node);
        GameObject.FindObjectOfType<TouchController>().state = 0;
    }

    public void DeleteSelectedCallout()
    {
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        Destroy(node);
        GameObject.FindObjectOfType<TouchController>().state = 0;
    }
}
