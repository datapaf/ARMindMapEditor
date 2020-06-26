using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public Material nodeSelectionMaterial;
    private Material materialBeforeNodeSelection;

    public Color relationshipSelectionColor;
    private Color colorBeforeRelationshipSelection;

    public GameObject CTActionsMenu;
    public GameObject MTActionsMenu;
    public GameObject SubtopicActionsMenu;
    public GameObject RelationshipActionsMenu;

    private GameObject hitObject = null;
    private GameObject hitNode = null;
    private GameObject hitRelationship = null;

    private GameObject selectedObject;

    private bool isItemChosen = false;

    private UnityEngine.Vector3 startTouchPosition;

    void Update()
    {
        // actions on the first touch
        if (Input.GetMouseButtonDown(0))
        {
            // remember the position of the first touch 
            startTouchPosition = Input.mousePosition;

            // obtain the object after raycasting
            GetHitObject();

            if (hitObject != null)
            {
                if (isRelationship(hitObject))
                {
                    hitRelationship = hitObject;
                }
                else
                {
                    // if we hit the shape of a node then the node gameobject should be the shape's grandparent 
                    var hitObjectGrandparent = hitObject.transform.parent.parent;

                    // if we actually tapped on a node then instantiate hitNode, leave hitNode null otherwise 
                    if (hitObjectGrandparent != null && isNode(hitObjectGrandparent.gameObject))
                    {
                        // save the node that was tapped
                        hitNode = hitObject.transform.parent.parent.gameObject;
                    }
                }
            }
        }
        // actions on lifted finger 
        else if (Input.GetMouseButtonUp(0))
        {
            // if nothing is selected
            if (isItemChosen == false)
            {
                // if the finger wasn't moved and we obtained the tag of the tapped node
                if (startTouchPosition == Input.mousePosition)
                {
                    if (isNode(hitNode))
                    {
                        isItemChosen = true;

                        selectedObject = hitNode;

                        Debug.Log(selectedObject.tag + " selected");

                        Highlight(selectedObject);

                        ActivateActionsMenu(selectedObject);

                        hitNode = null;
                    }
                    else if (isRelationship(hitRelationship))
                    {
                        isItemChosen = true;

                        selectedObject = hitRelationship;

                        Debug.Log(selectedObject.tag + " selected");

                        Highlight(selectedObject);

                        ActivateActionsMenu(selectedObject);

                        hitRelationship = null;
                    }
                }
            }
            // if something is selected
            else
            {
                if (startTouchPosition == Input.mousePosition && hitNode != selectedObject)
                {
                    isItemChosen = false;

                    Debug.Log(selectedObject.tag + " deselected");

                    DeHighlight(selectedObject);

                    DeActivateActionsMenu(selectedObject);

                    selectedObject = null;

                    hitNode = null;
                }
            }
        }
    }

    bool isMindMapItem(GameObject go)
    {
        if (isNode(go) || isRelationship(go))
            return true;

        return false;
    }

    bool isNode(GameObject go)
    {
        if (go == null)
            return false;
        if (go.tag == "CentralTopic")
            return true;
        if (go.tag == "MainTopic")
            return true;
        if (go.tag == "Subtopic")
            return true;

        return false;
    }

    bool isRelationship(GameObject go)
    {
        if (go == null)
            return false;
        if (go.tag == "Relationship")
            return true;
        return false;
    }

    void ActivateActionsMenu(GameObject go)
    {
        switch (go.tag)
        {
            case "CentralTopic":
                CTActionsMenu.SetActive(true);
                break;
            case "MainTopic":
                MTActionsMenu.SetActive(true);
                break;
            case "Subtopic":
                SubtopicActionsMenu.SetActive(true);
                break;
            case "Relationship":
                RelationshipActionsMenu.SetActive(true);
                break;
            default:
                break;
        }
    }

    void DeActivateActionsMenu(GameObject go)
    {
        switch (go.tag)
        {
            case "CentralTopic":
                CTActionsMenu.SetActive(false);
                break;
            case "MainTopic":
                MTActionsMenu.SetActive(false);
                break;
            case "Subtopic":
                SubtopicActionsMenu.SetActive(false);
                break;
            case "Relationship":
                RelationshipActionsMenu.SetActive(false);
                break;
            default:
                break;
        }
    }

    void Highlight(GameObject go)
    {
        if (isNode(go))
        {
            Renderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetComponent<Renderer>();
            materialBeforeNodeSelection = nodeRenderer.material;
            Color nodeColor = nodeRenderer.material.color;
            nodeRenderer.material = nodeSelectionMaterial;
            nodeRenderer.material.color = nodeColor;
        }
        else if (isRelationship(go))
        {
            LineRenderer relationshipRenderer = go.GetComponent<LineRenderer>();
            colorBeforeRelationshipSelection = relationshipRenderer.startColor;
            relationshipRenderer.SetColors(relationshipSelectionColor, relationshipSelectionColor);
        }

    }

    void DeHighlight(GameObject go)
    {
        if (isNode(go))
        {
            Renderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetComponent<Renderer>();
            nodeRenderer.material = materialBeforeNodeSelection;
        }
        else if (isRelationship(go))
        {
            LineRenderer relationshipRenderer = go.GetComponent<LineRenderer>();
            relationshipRenderer.SetColors(colorBeforeRelationshipSelection, colorBeforeRelationshipSelection);
        }
    }

    void GetHitObject()
    {
        // create a ray that goes from the camera  
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // cast the ray 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // getting the information
            hitObject = hit.transform.gameObject;
        }
    }
}
