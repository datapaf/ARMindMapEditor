using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relationship : MonoBehaviour
{
    public GameObject object1 = null;
    public GameObject object2 = null;
    private LineRenderer lineRenderer;
    private CapsuleCollider capsuleCollider;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        capsuleCollider = gameObject.transform.GetChild(0).GetComponent<CapsuleCollider>();

        capsuleCollider.radius = lineRenderer.startWidth * 4;
        capsuleCollider.center = Vector3.zero;
        capsuleCollider.direction = 2;
    }

    void Update()
    {
        if (object1 != null && object2 != null)
        {
            GameObject start = object1.transform.GetChild(1).GetChild(0).gameObject;
            GameObject end = object2.transform.GetChild(1).GetChild(0).gameObject;

            capsuleCollider.transform.position = start.transform.position + (end.transform.position - start.transform.position) / 2;
            capsuleCollider.transform.LookAt(start.transform.position);
            capsuleCollider.height = (end.transform.position - start.transform.position).magnitude;

            lineRenderer.SetPosition(1, transform.InverseTransformPoint(start.transform.position));
            lineRenderer.SetPosition(0, transform.InverseTransformPoint(end.transform.position));
        }
    }
}
