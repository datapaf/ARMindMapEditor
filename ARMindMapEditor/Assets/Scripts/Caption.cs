using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Caption : MonoBehaviour
{
    public float sizeMultiplier = 2;

    private GameObject text;
    private GameObject background;
    private GameObject model;

    private bool isSetupDone;

    private void OnEnable()
    {
        isSetupDone = false;
    }

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
            background.transform.position = transform.position - background.transform.forward * radius;

            isSetupDone = true;
        }

        string newText;
        if (transform.parent.GetComponent<Node>())
        {
            newText = transform.parent.GetComponent<Node>().text;
        }
        else
        {
            newText = transform.parent.GetComponent<Callout>().text;
        }

        for (int i = 0; i < newText.Length; i++)
        {
            if (i != 0 && i % 18 == 0)
            {
                newText = newText.Substring(0, i+1) + "\n" + newText.Substring(i+1);
                i++;
            }
        }

        text.GetComponent<TextMesh>().text = newText;

        /*if (transform.parent.GetComponent<Node>())
        {
            text.GetComponent<TextMesh>().text = transform.parent.GetComponent<Node>().text;
        }
        else
        {
            text.GetComponent<TextMesh>().text = transform.parent.GetComponent<Callout>().text;
        }*/


        background.transform.localScale = sizeMultiplier * new Vector3(1,.5f,1) *
            Mathf.Min(model.transform.localScale.x, model.transform.localScale.y, model.transform.localScale.z);

        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}
