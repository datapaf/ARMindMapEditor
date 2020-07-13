using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public string text;
    
    public float size;
    private float prevSize;
    public float minSize;
    public float maxSize;

    public NodeColor.ColorType nodeColor;
    //private Color color;

    public DemonstrationMode mode;

    public Shape.ShapeType shapeType;

    public int level;

    public bool isHidden;

    public GameObject relationship = null;
    public List<GameObject> nextNodes = new List<GameObject>();

    // the model of the node
    private GameObject model = null;

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

            SetupVolumeNode();
        }
        else if (mode == DemonstrationMode.Flat)
        {
            // disable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(false);

            SetupFlatNode();
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
                modelRenderer.material.color = new Color32(NodeColor.GetColor(nodeColor).r,
                    NodeColor.GetColor(nodeColor).g, NodeColor.GetColor(nodeColor).b, 30);
            }
            else
            {
                modelRenderer.material.color = new Color32(NodeColor.GetColor(nodeColor).r,
                    NodeColor.GetColor(nodeColor).g, NodeColor.GetColor(nodeColor).b, 255);
            }
        }

        if (transform.parent.GetComponent<MindMap>().mode == DemonstrationMode.Volume && mode == DemonstrationMode.Flat)
        {
            Destroy(model);

            SetupVolumeNode();

            // enable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(true);

            mode = DemonstrationMode.Volume;
        }
        else if (transform.parent.GetComponent<MindMap>().mode == DemonstrationMode.Flat && mode == DemonstrationMode.Volume)
        {
            // disable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(false);

            Destroy(model);

            SetupFlatNode();

            mode = DemonstrationMode.Flat;
        }

        // applying changing of the size if it happens
        if ( prevSize != size ) 
        {
            gameObject.transform.localScale = Vector3.one * size;
            prevSize = size;
        }
    }

    private void SetupVolumeNode()
    {
        // loading the model depending on the chosen shape
        switch (Shape.GetVolumeShape(shapeType))
        {
            case Shape.VolumeShape.Sphere:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Sphere", typeof(GameObject)));
                break;
            case Shape.VolumeShape.Parallelopipedon:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Parallelopipedon", typeof(GameObject)));
                break;
            case Shape.VolumeShape.Capsule:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Capsule", typeof(GameObject)));
                break;
        }

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
        modelRenderer.material.SetColor("_Color", NodeColor.GetColor(nodeColor));

        // changing transparency if it is preview
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color32(NodeColor.GetColor(nodeColor).r, NodeColor.GetColor(nodeColor).g, NodeColor.GetColor(nodeColor).b, 127);
        }

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);
    }

    private void SetupFlatNode()
    {
        // loading the model depending on the chosen shape
        switch (Shape.GetFlatShape(shapeType))
        {
            case Shape.FlatShape.Circle:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Circle", typeof(GameObject)));
                break;
            case Shape.FlatShape.Rectangle:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Rectangle", typeof(GameObject)));
                break;
            case Shape.FlatShape.Ellipse:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Ellipse", typeof(GameObject)));
                break;
        }

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        modelRenderer.material.SetColor("_Color", NodeColor.GetColor(nodeColor));

        // changing transparency if it is preview
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color32(NodeColor.GetColor(nodeColor).r, NodeColor.GetColor(nodeColor).g, NodeColor.GetColor(nodeColor).b, 127);
        }

        // set the text on the shape
        model.transform.GetChild(0).GetChild(1).GetComponent<TextMesh>().text = text;

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);
    }

    public static void DeleteNode(GameObject node)
    {
        if (node.tag == "CentralTopic")
        {
            // remember the name of the map
            string mapName = GameObject.FindObjectOfType<MindMap>().mapName;
            EasySave.Save("ResetMapName", mapName);
            EasySave.Save("isMapReset", true);
            SceneManager.LoadScene(0);
        }

        foreach (var go in node.GetComponent<Node>().nextNodes)
        {
            if (go != null)
            {
                DeleteNode(go);
            }
        }

        //delete relationship
        Destroy(node.GetComponent<Node>().relationship);

        // delete node
        Destroy(node);
    }

    public void ChangeShape()
    {
        if (mode == DemonstrationMode.Volume)
        {
            // disable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(false);

            Destroy(model);

            SwitchShapeType();

            SetupVolumeNode();

            // enable caption as we will show text on the shape
            transform.GetChild(0).gameObject.SetActive(true);

        }
        else if (mode == DemonstrationMode.Flat)
        {
            Destroy(model);

            SwitchShapeType();

            SetupFlatNode();
        }
    }

    private void SwitchShapeType()
    {
        if (shapeType != Shape.ShapeType.Type3)
            shapeType++;
        else
            shapeType = Shape.ShapeType.Type1;
    }

    public void ChangeColor()
    {
        SwitchColor();

        if (mode == DemonstrationMode.Volume)
        {
            var modelRenderer = model.transform.GetChild(0).GetComponent<Renderer>();
            modelRenderer.material.SetColor("_Color", NodeColor.GetColor(nodeColor));
        }
        else if (mode == DemonstrationMode.Flat)
        {
            var modelRenderer = model.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
            modelRenderer.material.SetColor("_Color", NodeColor.GetColor(nodeColor));
        }
        
    }

    public void SwitchColor()
    {
        if (nodeColor != NodeColor.ColorType.Grey)
            nodeColor++;
        else
            nodeColor = NodeColor.ColorType.Red;
    }
}
