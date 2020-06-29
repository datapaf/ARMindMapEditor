﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relationship : MonoBehaviour
{

    public GameObject object1 = null;
    public GameObject object2 = null;
    private LineRenderer lineRenderer;
    //private CapsuleCollider collider;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
/*        collider = gameObject.GetComponent<CapsuleCollider>();

        collider.radius = lineRenderer.startWidth;
        collider.center = Vector3.zero;
        collider.direction = 2;*/
    }

    void Update()
    {
        if (object1 != null && object2 != null)
        {
            /*collider.transform.position = object1.transform.position + (object2.transform.position - object1.transform.position) / 2;
            collider.transform.LookAt(object1.transform.position);
            collider.height = (object2.transform.position - object1.transform.position).magnitude;
*/
            lineRenderer.SetPosition(1, object1.transform.position);
            lineRenderer.SetPosition(0, object2.transform.position);
        }
    }
}
