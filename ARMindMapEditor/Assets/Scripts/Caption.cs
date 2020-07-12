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
            model = transform.parent.GetChild(1).GetChild(0).gameObject;

            transform.position = model.transform.position;
            transform.LookAt(Camera.main.transform.position, -Vector3.up);

            var modelScale = model.transform.lossyScale;
            var radius = Mathf.Sqrt(Mathf.Pow(modelScale.x/2,2) + Mathf.Pow(modelScale.y/2, 2) + Mathf.Pow(modelScale.z/2, 2));

            background.transform.position = model.transform.position - background.transform.forward * radius;

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


        var scale = model.transform.parent.localScale;
        background.transform.localScale = 
            sizeMultiplier * 
            Mathf.Min(scale.x, scale.y, scale.z) *
            new Vector3(1, 0.5f, 1);    

        transform.LookAt(Camera.main.transform.position, -Vector3.up);
    }
}
