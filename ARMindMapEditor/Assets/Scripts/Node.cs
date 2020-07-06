using SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    // the list of possible shapes
    public enum Shape { Sphere, Parallelopipedon, Capsule };

    public string text;
    
    public float size;
    private float prevSize;
    public float minSize;
    public float maxSize;

    public Color color;
    
    public Shape shape;

    public int level;

    public GameObject relationship = null;
    public List<GameObject> nextNodes = new List<GameObject>();

    // the model of the node
    private GameObject model;

    void Start()
    {
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

        // if the node is a central topic then set up the special parameters
        if (gameObject.tag == "CentralTopic")
        {
            minSize = size * 0.5f;
            maxSize = gameObject.transform.parent.GetComponent<MindMap>().sizeMultiplier;
            level = 0;
        }

        // the size is not changing at the start
        prevSize = size;

    }

    void Update()
    {
        // applying changing of the size if it happens
        if ( prevSize != size ) 
        {
            gameObject.transform.localScale = Vector3.one * size;
            prevSize = size;
        }
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
            DeleteNode(go);
        }

        /*GameObject predNode = node.GetComponent<Node>().relationship.GetComponent<Relationship>().object1;
        foreach (var go in predNode.GetComponent<Node>().nextNodes)
        {
            if (GameObject.ReferenceEquals(node, go))
            {
                predNode.GetComponent<Node>().nextNodes.Remove(node);
                break;
            }
        }*/

        //delete relationship
        Destroy(node.GetComponent<Node>().relationship);

        // delete node
        Destroy(node);
    }
}
