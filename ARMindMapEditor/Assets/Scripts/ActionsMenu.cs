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

    private string prevName;

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

        if (node.GetComponent<Node>())
        {
            if (node.GetComponent<Node>().isHidden)
            {
                menu.transform.Find("ShowButton").gameObject.SetActive(true);
                menu.transform.Find("HideButton").gameObject.SetActive(false);
            }
            else 
            {
                menu.transform.Find("ShowButton").gameObject.SetActive(false);
                menu.transform.Find("HideButton").gameObject.SetActive(true);
            }
        }
        else
        {
            if (node.GetComponent<Callout>().isHidden)
            {
                menu.transform.Find("ShowButton").gameObject.SetActive(true);
                menu.transform.Find("HideButton").gameObject.SetActive(false);
            }
            else
            {
                menu.transform.Find("ShowButton").gameObject.SetActive(false);
                menu.transform.Find("HideButton").gameObject.SetActive(true);
            }
        }

        GameObject.FindObjectOfType<EditorMenu>().transform.Find("SearchButton").GetComponent<Button>().interactable = false;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("ModeButton").GetComponent<Button>().interactable = false;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("RedoButton").GetComponent<Button>().interactable = false;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("UndoButton").GetComponent<Button>().interactable = false;
    }

    public void HideMenu()
    {
        menu.SetActive(false);

        GameObject.FindObjectOfType<EditorMenu>().transform.Find("SearchButton").GetComponent<Button>().interactable = true;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("ModeButton").GetComponent<Button>().interactable = true;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("RedoButton").GetComponent<Button>().interactable = true;
        GameObject.FindObjectOfType<EditorMenu>().transform.Find("UndoButton").GetComponent<Button>().interactable = true;
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
        GameObject mindMap = GameObject.FindObjectOfType<MindMap>().gameObject;
        GameObject CT = GameObject.FindGameObjectWithTag("CentralTopic").gameObject;
        GameObject CTModel = CT.transform.GetChild(1).GetChild(0).gameObject;
        GameObject FT = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));
        FT.transform.SetParent(mindMap.transform, false);
        FT.transform.position = CT.transform.position +
                                CT.transform.forward * CTModel.transform.lossyScale.z/2 +
                                CT.transform.up * CTModel.transform.lossyScale.y;
        FT.transform.rotation = CT.transform.rotation;
    }

    public void CreateCallout()
    {
        GameObject mindMap = GameObject.FindObjectOfType<MindMap>().gameObject;
        GameObject CT = GameObject.FindGameObjectWithTag("CentralTopic").gameObject;
        GameObject CTModel = CT.transform.GetChild(1).GetChild(0).gameObject;
        GameObject Callout = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
        Callout.transform.SetParent(mindMap.transform, false);
        Callout.transform.position = CT.transform.position + 
                                     -CT.transform.forward * CTModel.transform.lossyScale.z/2 +
                                     CT.transform.up * CTModel.transform.lossyScale.y;
        Callout.transform.rotation = CT.transform.rotation;
    }

    public void StartChangingText()
    {
        if (node.GetComponent<Node>())
        {
            prevName = node.GetComponent<Node>().text;
        }
        else 
        {
            prevName = node.GetComponent<Callout>().text;
        }
        inputField = menu.transform.Find("InputField").gameObject;
        inputField.GetComponent<InputField>().text = "";
        inputField.SetActive(true);
        inputField.GetComponent<InputField>().Select();
        inputField.GetComponent<InputField>().ActivateInputField();
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
        if (node.GetComponent<Node>())
        {
            if (CheckForEmptyText(node.GetComponent<Node>().text))
            {
                node.GetComponent<Node>().text = prevName;
            }
        }
        else
        {
            if (CheckForEmptyText(node.GetComponent<Callout>().text))
            {
                node.GetComponent<Callout>().text = prevName;
            }
        }
        
        inputField.SetActive(false);
    }

    public void DeleteSelectedNode()
    {
        Node.DeleteNode(node);
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 6;
    }

    public void DeleteSelectedCallout()
    {
        Destroy(node);
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 6;
    }

    public void ChangeNodeShape()
    {
        node.GetComponent<Node>().ChangeShape();
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 0;
    }

    public void ChangeNodeColor()
    {
        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 0;
        node.GetComponent<Node>().ChangeColor();
    }

    public void Hide()
    {
        menu.transform.Find("HideButton").gameObject.SetActive(false);
        menu.transform.Find("ShowButton").gameObject.SetActive(true);

        if (node.GetComponent<Node>())
        {
            node.GetComponent<Node>().isHidden = true;
        }
        else
        {
            node.GetComponent<Callout>().isHidden = true;
        }

        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 6;
    }

    public void Show()
    {
        menu.transform.Find("HideButton").gameObject.SetActive(true);
        menu.transform.Find("ShowButton").gameObject.SetActive(false);

        if (node.GetComponent<Node>())
        {
            node.GetComponent<Node>().isHidden = false;
        }
        else
        {
            node.GetComponent<Callout>().isHidden = false;
        }


        GameObject.FindObjectOfType<SelectionManager>().Deselect();
        GameObject.FindObjectOfType<TouchController>().state = 6;
    }

    bool CheckForEmptyText(string text)
    {
        foreach (char s in text)
        {
            if (s != ' ')
                return false;
        }

        return true;
    }
}
