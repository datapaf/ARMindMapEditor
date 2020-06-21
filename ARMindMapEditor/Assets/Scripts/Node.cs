using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // the list of possible shapes
    public enum Shape { Sphere, Parallelopipedon, Capsule };
    public enum NodeType { CentralTopic, MainTopic, Subtopic, FloatingTopic };

    public NodeType nodeType;
    public string text;
    public float size;
    public Color color;
    public Shape shape;

    public bool isDragged = false;
    public GameObject nextNode = null;

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

        // loading the model depending on the chosen shape
        switch (shape)
        {
            case Shape.Sphere:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Sphere", typeof(GameObject)));
                break;
            case Shape.Parallelopipedon:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Parallelopipedon", typeof(GameObject)));
                break;
            case Shape.Capsule:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Capsule", typeof(GameObject)));
                break;
        }

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
        modelRenderer.material.SetColor("_Color", color);

        // changing transparency if it is preview
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview) {
            modelRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
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
