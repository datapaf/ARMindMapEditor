using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationManager : MonoBehaviour
{
    private string tappedNodeTag = null;

    void Start()
    {
    }

    void Update()
    {
        // actions on the first touch
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform;
                var grandparent = hitObject.parent.parent;
                if (grandparent != null)
                {
                    switch (grandparent.tag)
                    {
                        case "CentralTopic":
                            Debug.Log("Tap onto Central Topic");
                            tappedNodeTag = "CentralTopic";
                            break;
                        default:
                            Debug.Log("Tap onto no node");
                            break;
                    }
                }
            }
        }
        // actions on dragging
        else if (Input.GetMouseButton(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform;
                var grandparent = hitObject.parent.parent;
                if (grandparent != null)
                {
                    switch (grandparent.tag)
                    {
                        case "CentralTopic":
                            
                            break;
                        default:
                            
                            break;
                    }
                }
            }
        }
        // actions on finger up
        else if (Input.GetMouseButtonUp(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hitObject = hit.transform;
                var grandparent = hitObject.parent.parent;
                if (grandparent != null)
                {
                    switch (grandparent.tag)
                    {
                        case "CentralTopic":
                            if (tappedNodeTag == "CentralTopic") 
                            {
                                Debug.Log("Deletion of the main topic");
                            }
                            break;
                        default:
                            if (tappedNodeTag != null)
                            { 
                                Debug.Log("Leave the new created node");
                            }
                            break;
                    }
                }
            }
        }
    }
}
