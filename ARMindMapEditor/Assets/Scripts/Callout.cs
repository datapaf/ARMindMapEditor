using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Callout : MonoBehaviour
{

    public string text;
    public float size;

    // the model of the node
    private GameObject model;
    // the caption containing the text and the background
    private GameObject caption;
    // the text of the caption
    private TextMesh captionText;

    void Start()
    {
        // instantiation
        caption = transform.GetChild(0).gameObject;
        captionText = caption.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Callout", typeof(GameObject)));

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

        // moving the model upward to place it on the model
        caption.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y + 0.1f, 0);

        // assigning the text to the caption
        captionText.text = text;
    }

    void Update()
    {
    }
}
