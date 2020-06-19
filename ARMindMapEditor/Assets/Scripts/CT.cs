using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CT : MonoBehaviour
{
    // the list of possible shapes
    public enum Shape { Sphere };

    public string text;
    public float size = 1;
    public Color color = Color.red;
    public Shape shape = Shape.Sphere;

    // the model of the node
    private GameObject model;
    // the caption containing the text and the background
    private GameObject caption;
    // the text of the caption
    private TextMesh captionText;

    void Start()
    {
        // instantiation
        model = GameObject.Find("Model").gameObject;
        caption = GameObject.Find("Caption").gameObject;
        captionText = GameObject.Find("Text").GetComponent<TextMesh>();

        // loading the model depending on the chosen shape
        switch ( shape ) {
            case Shape.Sphere:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Sphere", typeof(GameObject)));
                break;
        }

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.GetComponent<Renderer>();
        modelRenderer.material.SetColor("_Color", color);
        
        // changing transparency if it is preview
        if (transform.parent.GetComponent<MindMap>().isPreview) {
            modelRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
        }
        // moving the model upward to place it on the surface
        model.transform.position = caption.transform.position;
        model.transform.position += new Vector3(0, model.transform.localScale.y/2, 0);

        // moving the model upward to place it on the model
        caption.transform.position += new Vector3(0, model.transform.localScale.y + 0.1f, 0);

        // assigning the text to the caption
        captionText.text = text;
    }

    void Update()
    {    
    }
}
