using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlatShape : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
                                       this.transform.position.y,
                                       Camera.main.transform.position.z);
        this.transform.LookAt(targetPostition, -Vector3.up);
    }
}
