using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    private float startTime;
    public float holdingTime = 0.5f;

    int state = 0;

    CreationManager creationManager;
    SelectionManager selectionManager;
    MovingManager movingManager;

    void Start()
    {
        creationManager = GameObject.Find("Managers").transform.Find("Creation Manager").GetComponent<CreationManager>();
        selectionManager = GameObject.Find("Managers").transform.Find("Selection Manager").GetComponent<SelectionManager>();
        movingManager = GameObject.Find("Managers").transform.Find("Moving Manager").GetComponent<MovingManager>();
    }

    void Update()
    {
        if (state == 0)
        {
            if (IsTapped() && IsPointedToItem())
            {
                startTime = Time.time;
                state = 1;
            }
        }
        else if (state == 1)
        {
            creationManager.PrepareForCreation();
            selectionManager.PrepareForSelection();

            if (IsMoved())
            {
                state = 2;
            }
            else if (IsReleased())
            {
                selectionManager.Select();
                state = 3;
            }
            else if (IsHeld(holdingTime))
            {
                movingManager.ShowAxesForMoving();
                state = 4;
            }
        }
        else if (state == 2)
        {
            creationManager.Creation();

            if (IsReleased())
            {
                creationManager.EndCreation();
                state = 5;
            }
        }
        else if (state == 3)
        {
            if (IsTappedForExitSelection())
            {
                selectionManager.Deselect();
                state = 6;
            }
        }
        else if (state == 4)
        {
            if (IsReleased())
            {
                state = 7;
            }
        }
        else if (state == 7)
        {
            if (IsTappedForExitMoving())
            {
                state = 9;
            }
            else if (IsTapped())
            {
                movingManager.PrepareForMoving();
                state = 8;
            }
        }
        else if (state == 8)
        {
            movingManager.Moving();

            if (IsTappedForExitMoving())
            {
                state = 9;
            }
            else if (IsReleased())
            {
                state = 7;
            }
        }
        else if (state == 9)
        {
            movingManager.EndMoving();
            state = 0;
        }
        else
        {
            state = 0;
        }
    }

    public bool IsTappedForExitSelection()
    {
        return IsTapped() && !EventSystem.current.IsPointerOverGameObject();
    }

    public bool IsTappedForExitMoving()
    {
        return IsTapped() && GetPointedObject().tag != "Axis";
    }

    public bool IsPointedToItem()
    {
        GameObject pointedObject = GetPointedObject();

        return isMindMapItem(GetGrandparent(pointedObject));
    }

    public bool IsMoved()
    {
        return (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0);
    }

    public bool IsHeld(float sec)
    {
        return !IsMoved() && Input.GetMouseButton(0) && (Time.time - startTime) > sec;
    }

    public bool IsTapped()
    {
        return Input.GetMouseButtonDown(0);
    }

    public bool IsReleased()
    {
        return Input.GetMouseButtonUp(0);
    }

    public GameObject GetPointedObject()
    {
        GameObject hitObject = null;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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

    bool isMindMapItem(GameObject go)
    {
        if (go == null)
            return false;
        if (go.tag == "CentralTopic")
            return true;
        if (go.tag == "MainTopic")
            return true;
        if (go.tag == "Subtopic")
            return true;
        if (go.tag == "Relationship")
            return true;

        return false;
    }
}
