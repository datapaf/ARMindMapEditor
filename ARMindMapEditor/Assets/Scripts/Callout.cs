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

    public bool isHidden;

    // the model of the node
    private GameObject model;

    public DemonstrationMode mode;

    void Start()
    {

        while (!GameObject.FindObjectOfType<MindMap>())
        {
            mode = GameObject.FindObjectOfType<MindMap>().GetComponent<Node>().mode;
        }

        if (mode == DemonstrationMode.Volume)
        {
            // enable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(true);

            SetupVolumeCallout();
        }
        else if (mode == DemonstrationMode.Flat)
        {
            // disable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(false);

            SetupFlatCallout();
        }

        // the size is not changing at the start
        prevSize = size;
    }

    void Update()
    {
        if (!transform.parent.GetComponent<MindMap>().isPreview)
        {
            Renderer modelRenderer;
            if (mode == DemonstrationMode.Volume)
            {
                modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
            }
            else
            {
                modelRenderer = model.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            }

            if (isHidden)
            {
                modelRenderer.material.color = new Color(modelRenderer.material.color.r,
                    modelRenderer.material.color.g, modelRenderer.material.color.b, 0.1f);
            }
            else
            {

                modelRenderer.material.color = new Color(modelRenderer.material.color.r,
                    modelRenderer.material.color.g, modelRenderer.material.color.b, 1);
            }
        }

        if (transform.parent.GetComponent<MindMap>().mode == DemonstrationMode.Volume && mode == DemonstrationMode.Flat)
        {
            Destroy(model);

            SetupVolumeCallout();

            // enable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(true);

            mode = DemonstrationMode.Volume;
        }
        else if (transform.parent.GetComponent<MindMap>().mode == DemonstrationMode.Flat && mode == DemonstrationMode.Volume)
        {
            // disable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(false);

            Destroy(model);

            SetupFlatCallout();

            mode = DemonstrationMode.Flat;
        }

        // applying changing of the size if it happens
        if (prevSize != size)
        {
            gameObject.transform.localScale = Vector3.one * size;
            prevSize = size;
        }
    }

    private void SetupVolumeCallout()
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
    }

    private void SetupFlatCallout()
    {
        // loading the model depending on the chosen shape
        model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/CalloutShapeFlat", typeof(GameObject)));

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();

        // changing transparency if it is preview
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color(modelRenderer.material.color.r, modelRenderer.material.color.g, modelRenderer.material.color.b, 0.5f);
        }

        // set the text on the shape
        model.transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = text;

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);
    }
}
