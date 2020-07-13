using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public Material nodeSelectionMaterial;
    private Material materialBeforeNodeSelection;

    public Material nodeSelectionMaterialFlat;
    private Material materialBeforeNodeSelectionFlat;

    public Color relationshipSelectionColor;
    private Color colorBeforeRelationshipSelection;

    public GameObject actionsMenu;

    private GameObject hitObject = null;
    public GameObject hitNode = null;
    private GameObject hitRelationship = null;

    private GameObject selectedObject;

    public void PrepareForSelection()
    {
        // obtain the object after raycasting
        GetHitObject();

        if (hitObject != null)
        {
            if (isRelationship(hitObject.transform.parent.gameObject))
            {
                hitRelationship = hitObject.transform.parent.gameObject;
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

    public void Select()
    {
        if (isRelationship(hitRelationship))
        {
            selectedObject = hitRelationship;

            Highlight(selectedObject);

            actionsMenu.GetComponent<ActionsMenu>().SetNode(selectedObject);
            actionsMenu.GetComponent<ActionsMenu>().ShowMenu();

            hitRelationship = null;
        }
        else if (isNode(hitNode))
        {
            selectedObject = hitNode;

            Highlight(selectedObject);

            actionsMenu.GetComponent<ActionsMenu>().SetNode(selectedObject);
            actionsMenu.GetComponent<ActionsMenu>().ShowMenu();

            hitNode = null;
        }
    }

    public void Deselect()
    {
        DeHighlight(selectedObject);

        actionsMenu.GetComponent<ActionsMenu>().HideMenu();

        selectedObject = null;

        hitNode = null;
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
        if (go.tag == "FloatingTopic")
            return true;
        if (go.tag == "Callout")
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

    public void Highlight(GameObject go)
    {
        if (isNode(go))
        {
            if (go.tag == "Callout")
            {
                if (go.GetComponent<Callout>().mode == DemonstrationMode.Volume)
                {
                    HighlightVolume(go);
                }
                else if (go.GetComponent<Callout>().mode == DemonstrationMode.Flat)
                {
                    HighlightFlat(go);
                }
            }
            else
            {
                if (go.GetComponent<Node>().mode == DemonstrationMode.Volume)
                {
                    HighlightVolume(go);
                }
                else if (go.GetComponent<Node>().mode == DemonstrationMode.Flat)
                {
                    HighlightFlat(go);
                }
            }
        }
        else if (isRelationship(go))
        {
            LineRenderer relationshipRenderer = go.GetComponent<LineRenderer>();
            colorBeforeRelationshipSelection = relationshipRenderer.startColor;
            relationshipRenderer.SetColors(relationshipSelectionColor, relationshipSelectionColor);
        }

    }

    void HighlightVolume(GameObject go)
    {
        MeshRenderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetComponent<MeshRenderer>();
        materialBeforeNodeSelection = nodeRenderer.material;
        Color nodeColor = nodeRenderer.material.color;
        nodeRenderer.material = nodeSelectionMaterial;
        nodeRenderer.material.color = nodeColor;
    }

    void HighlightFlat(GameObject go)
    {
        SpriteRenderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        materialBeforeNodeSelectionFlat = nodeRenderer.material;
        Color nodeColor = nodeRenderer.material.color;
        nodeRenderer.material = nodeSelectionMaterialFlat;
        nodeRenderer.material.color = nodeColor;
    }

    public void DeHighlight(GameObject go)
    {
        if (isNode(go))
        {
            if (go.tag == "Callout")
            {
                if (go.GetComponent<Callout>().mode == DemonstrationMode.Volume)
                {
                    DeHighlightVolume(go);
                }
                else if (go.GetComponent<Callout>().mode == DemonstrationMode.Flat)
                {
                    DeHighlightFlat(go);
                }
            }
            else
            {
                if (go.GetComponent<Node>().mode == DemonstrationMode.Volume)
                {
                    DeHighlightVolume(go);
                }
                else if (go.GetComponent<Node>().mode == DemonstrationMode.Flat)
                {
                    DeHighlightFlat(go);
                }
            }
        }
        else if (isRelationship(go))
        {
            LineRenderer relationshipRenderer = go.GetComponent<LineRenderer>();
            relationshipRenderer.SetColors(colorBeforeRelationshipSelection, colorBeforeRelationshipSelection);
        }
    }

    void DeHighlightVolume(GameObject go)
    {
        Renderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetComponent<Renderer>();
        nodeRenderer.material = materialBeforeNodeSelection;
    }

    void DeHighlightFlat(GameObject go)
    {
        SpriteRenderer nodeRenderer = go.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        nodeRenderer.material = materialBeforeNodeSelectionFlat;
    }

    void GetHitObject()
    {
        // create a ray that goes from the camera  
        var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        // cast the ray 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // getting the information
            hitObject = hit.transform.gameObject;
        }
    }
}
