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
        string newText;
        if (transform.parent.parent.GetComponent<Node>())
        {
            newText = transform.parent.parent.GetComponent<Node>().text;
        }
        else
        {
            newText = transform.parent.parent.GetComponent<Callout>().text;
        }

        for (int i = 0; i < newText.Length; i++)
        {
            if (i != 0 && i % 18 == 0)
            {
                newText = newText.Substring(0, i + 1) + "\n" + newText.Substring(i + 1);
                i++;
            }
        }

        transform.GetChild(1).GetComponent<TextMesh>().text = newText;
        

        Vector3 targetPostition = new Vector3(Camera.main.transform.position.x,
                                       this.transform.position.y,
                                       Camera.main.transform.position.z);
        this.transform.LookAt(targetPostition, -Vector3.up);
    }
}
