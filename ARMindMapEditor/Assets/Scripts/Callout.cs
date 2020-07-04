using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callout : MonoBehaviour
{
    public string text;

    public float size;
    private float prevSize;
    public float minSize;
    public float maxSize;

    public int level;

    // the model of the node
    private GameObject model;

    void Start()
    {
        model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/CalloutShape", typeof(GameObject)));

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing transparency if it is preview
        var modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color(modelRenderer.material.color.r, modelRenderer.material.color.g, modelRenderer.material.color.b, 0.5f);
        }

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);

        // the size is not changing at the start
        prevSize = size;

    }

    void Update()
    {
        // applying changing of the size if it happens
        if (prevSize != size)
        {
            gameObject.transform.localScale = Vector3.one * size;
            prevSize = size;
        }
    }

    /*public string text;
    public float size;

    // the model of the node
    private GameObject model;

    void Start()
    {
        // instantiation
        model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/CalloutShape", typeof(GameObject)));

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing transparency if it is preview
        var modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color(modelRenderer.material.color.r, modelRenderer.material.color.g, modelRenderer.material.color.b, 0.5f);
        }

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);
    }

    void Update()
    {
    }*/
}
