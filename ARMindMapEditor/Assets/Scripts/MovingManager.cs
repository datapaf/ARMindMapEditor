using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingManager : MonoBehaviour
{
    public float fixedHoldTime = 0.5f;
    public float movingMultiplier = 25f;

    private float touchStartTime;

    private GameObject hitObject = null;
    private GameObject hitNode = null;

    private GameObject transformAxes;

    private bool isInMovingMode = false;

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
                // if we hit the shape of a node then the node gameobject should be the shape's grandparent 
                var hitObjectGrandparent = hitObject.transform.parent.parent;

                // if we actually tapped on a node then instantiate hitNode, leave hitNode null otherwise 
                if (hitObjectGrandparent != null && isNode(hitObjectGrandparent.gameObject))
                {
                    // save the node that was tapped
                    hitNode = hitObject.transform.parent.parent.gameObject;

                    touchStartTime = Time.time;
                }
            }
        }
        // actions on holding
        else if (Input.GetMouseButton(0))
        {

            float deltaTime = Time.time - touchStartTime;

            if (isInMovingMode == false)
            {
                if (startTouchPosition == Input.mousePosition && deltaTime >= fixedHoldTime)
                {
                    if (isNode(hitNode))
                    {
                        isInMovingMode = true;

                        transformAxes = Instantiate((GameObject)Resources.Load("Prefabs/Items/TransformAxes", typeof(GameObject)));

                        transformAxes.transform.position = hitNode.transform.GetChild(1).Find("AxesPlace").transform.position;

                        transformAxes.transform.SetParent(hitNode.transform, true);
                    }
                }
            }
            else 
            {
                if (isAxis(hitObject))
                {
                    string inputAxis =
                        GetInputAxis(startTouchPosition, Camera.main.WorldToScreenPoint(transformAxes.transform.Find("Origin").position));

                    switch (hitObject.name)
                    {
                        case "X":

                            if (inputAxis == "x")
                            {
                                hitNode.transform.position +=  Vector3.right * Input.GetAxis("Mouse X") / movingMultiplier;
                            }
                            else if (inputAxis == "y")
                            {
                                hitNode.transform.position += Vector3.right * Input.GetAxis("Mouse Y") / movingMultiplier;
                            }

                            break;
                        case "Y":

                            if (inputAxis == "x")
                            {
                                hitNode.transform.position += Vector3.up * Input.GetAxis("Mouse X") / movingMultiplier;
                            }
                            else if (inputAxis == "y")
                            {
                                hitNode.transform.position += Vector3.up * Input.GetAxis("Mouse Y") / movingMultiplier;
                            }

                            break;
                        case "Z":

                            if (inputAxis == "x")
                            {
                                hitNode.transform.position += Vector3.forward * Input.GetAxis("Mouse X") / movingMultiplier;
                            }
                            else if (inputAxis == "y")
                            {
                                hitNode.transform.position += Vector3.forward * Input.GetAxis("Mouse Y") / movingMultiplier;
                            }

                            break;
                    }

                }
            }
        }
        // actions on lifted finger
        else if (Input.GetMouseButtonUp(0))
        {
            hitObject = null;
        }
    }

    string GetInputAxis(Vector2 p, Vector2 o) 
    {
        Vector2 v = p - o;
        if ( Math.Abs(v.x) >= Math.Abs(v.y))
            return "x";
        else
            return "y";
    }

    bool isAxis(GameObject go)
    {
        if (go.name == "X")
            return true;
        if (go.name == "Y")
            return true;
        if (go.name == "Z")
            return true;
        return false;
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
}

