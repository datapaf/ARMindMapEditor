using SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchController : MonoBehaviour
{
    private float startTime;
    public float holdingTime = 0.5f;

    private bool isSaved = true;

    public GameObject editorMenu;
    public GameObject presetMenuUI;
    public GameObject cursor;

    private int prevState = -2;
    public int state = -1;

    private CreationManager creationManager;
    private SelectionManager selectionManager;
    private MovingManager movingManager;

    private bool notMoved;

    void Start()
    {
        creationManager = GameObject.FindObjectOfType<CreationManager>();
        selectionManager = GameObject.FindObjectOfType<SelectionManager>();
        movingManager = GameObject.FindObjectOfType<MovingManager>();

        if (EasySave.Load<bool>("isMapReset"))
        {
            isSaved = false;
            EasySave.Delete<bool>("isMapReset");
        }
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
                presetMenuUI.SetActive(true);
            }
            else
            {
                presetMenuUI.SetActive(false);
            }
            
            if (IsTappedNotOnUI() && cursor.activeInHierarchy)
            { 
                presetMenuUI.SetActive(false);
                editorMenu.SetActive(true);
                state = 0;
            }
        }
        else if (state == 0)
        {
            if (GameObject.FindObjectOfType<MindMap>() && isSaved == false)
            {
                GameObject.FindObjectOfType<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
                isSaved = true;
            }

            if (IsTappedNotOnUI() && IsPointedToRelationship())
            {
                isSaved = false;
                state = 10;
            }
            else if (IsTappedNotOnUI() && IsPointedToNode())
            {
                startTime = Time.time;
                creationManager.PrepareForCreation();
                notMoved = true;
                isSaved = false; 
                state = 1;
            }
        }
        else if (state == 1)
        {
            if (IsMoved())
            {
                notMoved = false;
            }

            selectionManager.PrepareForSelection();

            if (IsMovedSignificantly())
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
            if (IsTappedNotOnUI())
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

    public static bool IsTappedNotOnUI()
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

    public bool IsMovedSignificantly()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved &&
            ( Mathf.Abs(Input.GetTouch(0).deltaPosition.x) > Screen.width / 20 || Mathf.Abs(Input.GetTouch(0).deltaPosition.y) > Screen.width / 20);
    }

    public bool IsMoved()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
    }

    public bool IsHeld(float sec)
    {
        return Input.touchCount == 1 && notMoved && (Time.time - startTime) > sec;
    }

    public static bool IsTapped()
    {
        return Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began;
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
