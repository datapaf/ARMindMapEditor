using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class CreationManager : MonoBehaviour
{
    // tag of the object that the user tapped on
    private string      tappedNodeTag   = null;
    // the gameobjects of the new created nodes 
    private GameObject  newNode         = null;
    // the gameobject that was hit after raycasting
    private GameObject  hitObject       = null;
    // the grandparent of hitObject (necessary to determine the type of the node)
    private GameObject  hitNode         = null;

    // distance between the camera and the new node
    private float       ZCoord;

    // variable indicating that the creation of the new node is going
    private bool        isCreationGoing = false;

    // the screen coordinates of the first touch (necessary to detect dragging)
    //private UnityEngine.Vector3 startTouchPosition;

    void Update()
    {
        // actions on the first touch
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // remember the position of the first touch 
            //startTouchPosition = Input.GetTouch(0).position;

            // create a ray that goes from the camera  
            var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            // cast the ray 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                // getting the information
                hitObject = hit.transform.gameObject;
                if (hitObject.transform.parent.parent != null) 
                {
                    hitNode = hitObject.transform.parent.parent.gameObject;
                }

                // if the hitObject has a grandparent (we can determine on which node we actually tapped)
                if (hitNode != null)
                {
                    // remember on which node we tapped
                    tappedNodeTag = hitNode.tag;
                }
            }
        }
        // actions on dragging
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // if the new node has not been created and the user moved the finger
            if (newNode == null) 
            {
                // create a new node according to what node we tapped at the beginning
                if (tappedNodeTag == "CentralTopic")
                {
                    // here we start the creation of the new node
                    isCreationGoing = true;

                    // instantiation
                    newNode = Instantiate((GameObject)Resources.Load("Prefabs/Items/MT", typeof(GameObject)));
                    newNode.transform.SetParent(hitNode.transform.parent.transform, false);
                    newNode.transform.position = hitObject.transform.position;
                    newNode.transform.rotation = hitObject.transform.rotation;

                    // set up the predecessor node of the new node
                    newNode.GetComponent<Node>().predNode = hitNode;

                    // getting the distance from the camera to the node 
                    ZCoord = Camera.main.WorldToScreenPoint(newNode.transform.position).z;
                }
                else if (tappedNodeTag == "MainTopic" || tappedNodeTag == "Subtopic")
                {
                    // here we start the creation of the new node
                    isCreationGoing = true;

                    // instantiation
                    newNode = Instantiate((GameObject)Resources.Load("Prefabs/Items/Subtopic", typeof(GameObject)));
                    newNode.transform.SetParent(hitNode.transform.parent.transform, false);
                    newNode.transform.position = hitObject.transform.position;

                    // set up the predecessor node of the new node
                    newNode.GetComponent<Node>().predNode = hitNode;

                    // getting the distance from the camera to the node 
                    ZCoord = Camera.main.WorldToScreenPoint(newNode.transform.position).z;
                }
            } 
            // if the creation is going then change position of the new node according to the finger
            else if (isCreationGoing)
            {
                newNode.transform.position = GetTouchWorldPos();
            }

        }
        // actions on lifted finger 
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            // if a new node has been created and creation is going
            if (newNode != null && isCreationGoing) 
            {
                // if we can obtain its shape
                if (newNode.transform.GetChild(1) != null && newNode.transform.GetChild(1).transform.GetChild(0) != null)
                {
                    // get the colliders of hitObject and newNode and check their intersection
                    Collider hitObjectCollider, newNodeCollider;
                    hitObjectCollider = hitObject.GetComponent<Collider>();
                    newNodeCollider = newNode.transform.GetChild(1).transform.GetChild(0).GetComponent<Collider>();
                    if (newNodeCollider.bounds.Intersects(hitObjectCollider.bounds))
                    {
                        // if the colliders intersect then destroy newNode
                        Destroy(newNode);
                    }
                }
                // clear the tag of the tapped node
                tappedNodeTag = null;

                // clear the newNode gameobject indicating that we are ready to create a new node
                newNode = null;

                // after lifting up the finger the creation is done
                isCreationGoing = false;
            }
        }
    }

    private UnityEngine.Vector3 GetTouchWorldPos()
    {
        // xy coordinates
        UnityEngine.Vector3 touchPoint = Input.GetTouch(0).position;
        // convertion to xyz coordinates
        touchPoint.z = ZCoord;

        // returning a worldspace point at the provided distance z from the camera plane
        return Camera.main.ScreenToWorldPoint(touchPoint);
    }
}
