using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    // the lists of possible shapes
    public enum VolumeShape { Sphere, Parallelopipedon, Capsule };
    public enum FlatShape { Circle, Rectangle, Ellipse };

    public string text;
    
    public float size;
    private float prevSize;
    public float minSize;
    public float maxSize;

    public Color color;

    private DemonstrationMode mode;

    public VolumeShape volumeShape;
    public FlatShape flatShape;

    public int level;

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
        switch (volumeShape)
        {
            case VolumeShape.Sphere:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Sphere", typeof(GameObject)));
                break;
            case VolumeShape.Parallelopipedon:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Parallelopipedon", typeof(GameObject)));
                break;
            case VolumeShape.Capsule:
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
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
        }

        // moving the model upward to place it on the surface
        model.transform.position += new Vector3(0, model.transform.GetChild(0).localScale.y / 2, 0);
    }

    private void SetupFlatNode()
    {
        // loading the model depending on the chosen shape
        switch (flatShape)
        {
            case FlatShape.Circle:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Circle", typeof(GameObject)));
                break;
            case FlatShape.Rectangle:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Rectangle", typeof(GameObject)));
                break;
            case FlatShape.Ellipse:
                model = Instantiate((GameObject)Resources.Load("Prefabs/Shapes/Ellipse", typeof(GameObject)));
                break;
        }

        // make instantiated model be child of the node object
        model.transform.SetParent(gameObject.transform, false);

        // stretching the model depending on the chosen size 
        model.transform.localScale *= size;

        // changing the color depending on the chosen color 
        var modelRenderer = model.transform.GetChild(0).GetComponent<SpriteRenderer>();
        modelRenderer.material.SetColor("_Color", color);

        // changing transparency if it is preview
        if (transform.parent != null && transform.parent.GetComponent<MindMap>().isPreview)
        {
            modelRenderer.material.color = new Color(color.r, color.g, color.b, 0.5f);
        }

        // set the text on the shape
        model.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMesh>().text = text;

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
}
