﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caption : MonoBehaviour
{
    private GameObject text;
    private GameObject background;
    private GameObject model;

    private bool isSetupDone = false;

    void Update()
    {
        if (isSetupDone == false)
        {
            background = transform.GetChild(0).gameObject;
            text = background.transform.GetChild(0).gameObject;
            model = transform.parent.GetChild(1).gameObject;

            transform.position = model.transform.position;
            transform.LookAt(Camera.main.transform.position, -Vector3.up);

            var radius = Mathf.Max(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z) / 4;
            background.transform.position -= background.transform.forward * radius;

            isSetupDone = true;
        }

        text.GetComponent<TextMesh>().text = transform.parent.GetComponent<Node>().text;

        background.transform.localScale = new Vector3(2, 1, 2) *
            Mathf.Min(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z);

        transform.LookAt(Camera.main.transform.position, -Vector3.up);
        
    }
}