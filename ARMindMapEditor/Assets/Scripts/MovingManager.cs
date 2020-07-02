using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingManager : MonoBehaviour
{
    public float movingMultiplier = 25f;

    private GameObject hitObject = null;
    private GameObject hitNode = null;

    private GameObject transformAxes;

    private Vector3 startTouchPosition;

    public void ShowAxesForMoving()
    {
        // obtain the object after raycasting
        hitObject = GetPointedObject();

        if (hitObject != null)
        {
            // if we actually tapped on a node then instantiate hitNode, leave hitNode null otherwise 
            if (GetGrandparent(hitObject) != null)
            {
                // save the node that was tapped
                hitNode = GetGrandparent(hitObject);

                transformAxes = Instantiate((GameObject)Resources.Load("Prefabs/Items/TransformAxes", typeof(GameObject)));

                transformAxes.transform.position = hitNode.transform.GetChild(1).Find("AxesPlace").transform.position;
                transformAxes.transform.rotation = hitNode.transform.rotation;
                transformAxes.transform.Rotate(new Vector3(0, 180, 0));


                transformAxes.transform.SetParent(hitNode.transform, true);
            }
        }
    }

    public void PrepareForMoving()
    {
        startTouchPosition = Input.GetTouch(0).position;
        hitObject = GetPointedObject();
    }

    public void Moving()
    {
        string inputAxis =
            GetInputAxis(startTouchPosition, Camera.main.WorldToScreenPoint(transformAxes.transform.Find("Origin").position));

        switch (hitObject.name)
        {
            case "X":

                if (inputAxis == "x")
                {
                    hitNode.transform.position += hitNode.transform.right * Input.GetTouch(0).deltaPosition.x / Screen.currentResolution.width; // * movingMultiplier;
                }
                else if (inputAxis == "y")
                {
                    hitNode.transform.position += hitNode.transform.right * Input.GetTouch(0).deltaPosition.y / Screen.currentResolution.height;// * movingMultiplier;
                }

                break;

            case "Y":

                if (inputAxis == "x")
                {
                    hitNode.transform.position += hitNode.transform.up * Input.GetTouch(0).deltaPosition.x / Screen.currentResolution.width;// * movingMultiplier;
                }
                else if (inputAxis == "y")
                {
                    hitNode.transform.position += hitNode.transform.up * Input.GetTouch(0).deltaPosition.y / Screen.currentResolution.height;// * movingMultiplier;
                }

                break;

            case "Z":

                if (inputAxis == "x")
                {
                    hitNode.transform.position += hitNode.transform.forward * Input.GetTouch(0).deltaPosition.x / Screen.currentResolution.width;// * movingMultiplier;
                }
                else if (inputAxis == "y")
                {
                    hitNode.transform.position += hitNode.transform.forward * Input.GetTouch(0).deltaPosition.y / Screen.currentResolution.height;// * movingMultiplier;
                }

                break;
        }
    }

    public void EndMoving()
    {
        Destroy(transformAxes);
        hitObject = null;
    }

    string GetInputAxis(Vector2 p, Vector2 o)
    {
        Vector2 v = p - o;
        if (Math.Abs(v.x) >= Math.Abs(v.y))
            return "x";
        else
            return "y";
    }

    public GameObject GetPointedObject()
    {
        GameObject hitObject = null;

        var ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitObject = hit.transform.gameObject;
        }

        return hitObject;
    }

    public GameObject GetGrandparent(GameObject go)
    {
        return go.transform.parent.parent.gameObject;
    }
}

