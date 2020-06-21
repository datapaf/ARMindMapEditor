using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnMouseDrag()
    {
        gameObject.transform.parent.parent.GetComponent<Node>().isDragged = true;
    }

    private void OnMouseUp()
    {
        gameObject.transform.parent.parent.GetComponent<Node>().isDragged = false;
    }
}
