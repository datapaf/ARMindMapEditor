using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using SaveSystem;

public class SaveController : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
    }

    public void SaveMap(GameObject map)
    {   
        // create the entry containing the data about the whole map
        MindMapData mindMapData = new MindMapData();

        // initialize the fields of the new entry
        mindMapData.mapName = map.GetComponent<MindMap>().mapName;
        //mindMapData.sizeMultiplier = map.GetComponent<MindMap>().sizeMultiplier;
        mindMapData.isPreview = map.GetComponent<MindMap>().isPreview;
        mindMapData.mode = map.GetComponent<MindMap>().mode;

        // creathe the list of the items that the map contains
        mindMapData.items = new List<ItemData>();

        // going through each item
        for (int i = 0; i < map.transform.childCount; i++)
        {
            // get the item
            GameObject item = map.transform.GetChild(i).gameObject;
            
            // create the new entry about the current item
            ItemData data = new ItemData();

            // collecting data
            data.xPosition = item.transform.position.x;
            data.yPosition = item.transform.position.y;
            data.zPosition = item.transform.position.z;

            data.xRotation = item.transform.rotation.x;
            data.yRotation = item.transform.rotation.y;
            data.zRotation = item.transform.rotation.z;


            switch (item.tag)
            {
                case "CentralTopic":
                    data.itemType = ItemType.CT;
                    break;
                case "MainTopic":
                    data.itemType = ItemType.MT;
                    break;
                case "Subtopic":
                    data.itemType = ItemType.Subtopic;
                    break;
                case "FloatingTopic":
                    data.itemType = ItemType.FT;
                    break;
                case "Callout":
                    data.itemType = ItemType.Callout;
                    break;
                case "Relationship":
                    data.itemType = ItemType.Relationship;
                    break;
            }

            // collect other data depending on the type of the item
            if (item.GetComponent<Node>())
            {
                data.text = item.GetComponent<Node>().text;
                data.size = item.GetComponent<Node>().size;
                data.minSize = item.GetComponent<Node>().minSize;
                data.maxSize = item.GetComponent<Node>().maxSize;
                data.r = item.GetComponent<Node>().color.r;
                data.g = item.GetComponent<Node>().color.g;
                data.b = item.GetComponent<Node>().color.b;
                data.shape = item.GetComponent<Node>().shape;
                data.level = item.GetComponent<Node>().level;

                if (item.GetComponent<Node>().relationship != null)
                {
                    data.relationshipIndex = item.GetComponent<Node>().relationship.transform.GetSiblingIndex();
                }

                data.nextNodesIndices = new List<int>();
                foreach (var nextNode in item.GetComponent<Node>().nextNodes)
                {
                    if (nextNode != null)
                    {
                        data.nextNodesIndices.Add(nextNode.transform.GetSiblingIndex());
                    }
                }
            }
            else if (item.GetComponent<Callout>())
            {
                data.text = item.GetComponent<Callout>().text;
                data.size = item.GetComponent<Callout>().size;
                data.minSize = item.GetComponent<Callout>().minSize;
                data.maxSize = item.GetComponent<Callout>().maxSize;
                data.level = item.GetComponent<Callout>().level;
            }
            else if (item.GetComponent<Relationship>())
            {
                data.object1NumberAsChild = item.GetComponent<Relationship>().object1.transform.GetSiblingIndex();
                data.object2NumberAsChild = item.GetComponent<Relationship>().object2.transform.GetSiblingIndex();
            }

            // adding the collected data to the item entry
            mindMapData.items.Add(data);
        }

        // prepare for saving to the file
        FileSave fileSave = new FileSave(FileFormat.Json);

        // here we prevent saving renamed file as a new one
        if (File.Exists(Application.persistentDataPath + "/" + mindMapData.mapName + ".json"))
        {
            // the file with the map name exists then just rewrite it
            fileSave.WriteToFile(Application.persistentDataPath + "/" + mindMapData.mapName + ".json", mindMapData);
        }
        else 
        {
            // if the file with the map name does not exists but there exists the file with the old name of the map
            // then rewrite that file and rename it
            if (map.GetComponent<MindMap>().prevName != "" &&
                File.Exists(Application.persistentDataPath + "/" + map.GetComponent<MindMap>().prevName + ".json"))
            {
                fileSave.WriteToFile(Application.persistentDataPath + "/" + map.GetComponent<MindMap>().prevName + ".json", mindMapData);
                File.Move(Application.persistentDataPath + "/" + map.GetComponent<MindMap>().prevName + ".json",
                    Application.persistentDataPath + "/" + mindMapData.mapName + ".json");
            }
            // if the file with the map does not exists at all then create the new file and write there
            else
            {
                fileSave.WriteToFile(Application.persistentDataPath + "/" + mindMapData.mapName + ".json", mindMapData);
            }
        }

        Debug.Log("MAP SAVED");
    }

    public GameObject LoadMap(string mapName)
    {
        // get the entry conaining the data about the whole map 
        FileSave fileSave = new FileSave(FileFormat.Json);
        MindMapData mindMapData = fileSave.ReadFromFile<MindMapData>(Application.persistentDataPath + "/" + mapName + ".json");

        // instantiate the new empty map
        GameObject newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/EmptyMindMap", typeof(GameObject)));

        // retrieve the info about the map from the entry 
        newMindMap.GetComponent<MindMap>().mapName = mindMapData.mapName;
        //newMindMap.GetComponent<MindMap>().sizeMultiplier = mindMapData.sizeMultiplier;
        newMindMap.GetComponent<MindMap>().isPreview = mindMapData.isPreview;
        newMindMap.GetComponent<MindMap>().mode = mindMapData.mode;

        // here we will store the position of the central topic to set up the position of the other nodes
        Vector3 CTposition = Vector3.zero;

        // going through each item that is in the map we set up their parameters
        foreach (ItemData data in mindMapData.items)
        {
            GameObject item;

            if (data.itemType == ItemType.CT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/CT", typeof(GameObject)));

                Node itemNodeComponent = item.GetComponent<Node>();

                CTposition = new Vector3(data.xPosition, data.yPosition, data.zPosition);

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.transform.SetParent(newMindMap.transform);
            }
            else if (data.itemType == ItemType.MT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/MT", typeof(GameObject)));
                
                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition) - CTposition;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.transform.SetParent(newMindMap.transform);
            }
            else if (data.itemType == ItemType.Subtopic)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Subtopic", typeof(GameObject)));

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition) - CTposition;


                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.transform.SetParent(newMindMap.transform);
            }
            else if (data.itemType == ItemType.FT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition) - CTposition;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.transform.SetParent(newMindMap.transform);
            }
            else if (data.itemType == ItemType.Callout)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition) - CTposition;

                Callout itemCalloutComponent = item.GetComponent<Callout>();

                itemCalloutComponent.text = data.text;
                itemCalloutComponent.size = data.size;
                itemCalloutComponent.minSize = data.minSize;
                itemCalloutComponent.maxSize = data.maxSize;
                itemCalloutComponent.level = data.level;

                item.transform.SetParent(newMindMap.transform);
            }
            else if (data.itemType == ItemType.Relationship)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Relationship", typeof(GameObject)));

                Relationship itemRelationshipComponent = item.GetComponent<Relationship>();

                // here we obtain the objects that the relationship connects
                itemRelationshipComponent.object1 = newMindMap.transform.GetChild(data.object1NumberAsChild).gameObject;
                itemRelationshipComponent.object2 = newMindMap.transform.GetChild(data.object2NumberAsChild).gameObject;

                item.transform.SetParent(newMindMap.transform);
            }
        }

        int index = 0;
        foreach (ItemData data in mindMapData.items)
        {
            Node item = newMindMap.transform.GetChild(index).gameObject.GetComponent<Node>();
            if (data.itemType != ItemType.Relationship && data.itemType != ItemType.Callout)
            {
                item.relationship = newMindMap.transform.GetChild(data.relationshipIndex).gameObject;
                item.nextNodes = new List<GameObject>();
                foreach (var i in data.nextNodesIndices)
                {
                    item.nextNodes.Add(newMindMap.transform.GetChild(i).gameObject);
                }
            }

            index++;
        }

        return newMindMap;
    }

}
