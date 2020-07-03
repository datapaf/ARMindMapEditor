using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    private float startTime;
    public float holdingTime = 0.5f;

    public GameObject editorMenu;
    public GameObject tapToPlaceText;
    public GameObject cursor;

    int prevState = -2;
    int state = -1;

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
        if (prevState != state)
        {
            Debug.Log(state);
            prevState = state;
        }

        if (state == -1)
        {
            if (cursor.activeInHierarchy)
            {
                tapToPlaceText.SetActive(true);
            }
            else
            {
                tapToPlaceText.SetActive(false);
            }
            

            if (IsTapped() && cursor.activeInHierarchy)
            {
                tapToPlaceText.SetActive(false);
                editorMenu.SetActive(true);
                state = 0;
            }
        }
        else if (state == 0)
        {
            if (IsTapped() && IsPointedToRelationship())
            {
                state = 10;
            }
            else if (IsTapped() && IsPointedToNode())
            {
                startTime = Time.time; 
                creationManager.PrepareForCreation();
                state = 1;
            }
        }
        else if (state == 1)
        {

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
        else if (state == 10)
        {
            selectionManager.PrepareForSelection();

            if (IsReleased())
            {
                selectionManager.Select();
                state = 3;
            }
        }
        else
        {
            state = 0;
        }
    }

    public bool IsTappedForExitSelection()
    {
        return IsTapped() && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
    }

    public bool IsTappedForExitMoving()
    {
        return IsTapped() && GetPointedObject().tag != "Axis";
    }

    public bool IsPointedToNode()
    {
        GameObject pointedObject = GetPointedObject();

        return isNode(pointedObject);
    }

    public bool IsPointedToRelationship()
    {
        GameObject pointedObject = GetPointedObject();

        return isRelationship(pointedObject);
    }

    public bool IsMoved()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
    }

    public bool IsHeld(float sec)
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Stationary && (Time.time - startTime) > sec;
    }

    public bool IsTapped()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0);
    }

    public bool IsReleased()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended;
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
        if (go.transform.parent == null)
            return null;
        if (go.transform.parent.parent == null)
            return null;

        return go.transform.parent.parent.gameObject;
    }

    public GameObject GetParent(GameObject go)
    {
        if (go.transform.parent == null)
            return null;

        return go.transform.parent.gameObject;
    }

    bool isNode(GameObject go)
    {
        go = GetGrandparent(go);
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
        go = GetParent(go);
        if (go == null)
            return false;
        if (go.tag == "Relationship")
            return true;
        return false;
    }
}
