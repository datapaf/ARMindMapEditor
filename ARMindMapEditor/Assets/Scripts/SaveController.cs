using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using SaveSystem;

public class SaveController : MonoBehaviour
{
    private float startTime;
    public float timeTillNextSaving;

    private string prevName = "";

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime >= timeTillNextSaving)
        {
            // as the script works all the time we save only if there is a mind map in the scene
            if (GameObject.FindWithTag("MindMap"))
            {
                GameObject map = GameObject.FindWithTag("MindMap").gameObject;

                // here we prevent saving the map with changed name as a new map
                /*if (prevName != map.GetComponent<MindMap>().mapName)
                {
                    File.Delete("Assets/Resources/Maps/" + prevName + ".prefab");
                    //AssetDatabase.Refresh();
                    prevName = map.GetComponent<MindMap>().mapName;
                }*/

                //PrefabUtility.SaveAsPrefabAsset(map, "Assets/Resources/Maps/" + map.GetComponent<MindMap>().mapName + ".prefab");
                SaveMap(map);
            }
            startTime = Time.time;
        }
    }

    public void SaveMap(GameObject map)
    {
        MindMapData mindMapData = new MindMapData();

        mindMapData.mapName = map.GetComponent<MindMap>().mapName;
        mindMapData.sizeMultiplier = map.GetComponent<MindMap>().sizeMultiplier;
        mindMapData.isPreview = map.GetComponent<MindMap>().isPreview;
        mindMapData.mode = map.GetComponent<MindMap>().mode;

        mindMapData.items = new List<ItemData>();

        for (int i = 0; i < map.transform.childCount; i++)
        {
            GameObject item = map.transform.GetChild(i).gameObject;
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
                data.object1x = item.GetComponent<Relationship>().object1.transform.position.x;
                data.object1y = item.GetComponent<Relationship>().object1.transform.position.y;
                data.object1z = item.GetComponent<Relationship>().object1.transform.position.z;

                data.object2x = item.GetComponent<Relationship>().object2.transform.position.x;
                data.object2y = item.GetComponent<Relationship>().object2.transform.position.y;
                data.object2z = item.GetComponent<Relationship>().object2.transform.position.z;
            }

            // adding the collected data
            mindMapData.items.Add(data);
        }

        FileSave fileSave = new FileSave(FileFormat.Json);
        fileSave.WriteToFile("Assets/Resources/Maps/" + mindMapData.mapName + ".json", mindMapData);
    }

    public GameObject LoadMap(string mapName)
    {
        FileSave fileSave = new FileSave(FileFormat.Json);
        MindMapData mindMapData = fileSave.ReadFromFile<MindMapData>("Assets/Resources/Maps/" + mapName + ".json");

        GameObject newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/EmptyMindMap", typeof(GameObject)));
        
        newMindMap.GetComponent<MindMap>().mapName = mindMapData.mapName;
        newMindMap.GetComponent<MindMap>().sizeMultiplier = mindMapData.sizeMultiplier;
        newMindMap.GetComponent<MindMap>().isPreview = mindMapData.isPreview;
        newMindMap.GetComponent<MindMap>().mode = mindMapData.mode;

        foreach (ItemData data in mindMapData.items)
        {
            GameObject item;

            if (data.itemType == ItemType.CT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/CT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.SetActive(true);
            }
            else if (data.itemType == ItemType.MT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/MT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.SetActive(true);
            }
            else if (data.itemType == ItemType.Subtopic)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Subtopic", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.SetActive(true);
            }
            else if (data.itemType == ItemType.FT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.color = new Vector4(data.r, data.g, data.b, 1);
                itemNodeComponent.shape = data.shape;
                itemNodeComponent.level = data.level;

                item.SetActive(true);
            }
            else if (data.itemType == ItemType.Callout)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Callout itemCalloutComponent = item.GetComponent<Callout>();

                itemCalloutComponent.text = data.text;
                itemCalloutComponent.size = data.size;
                itemCalloutComponent.minSize = data.minSize;
                itemCalloutComponent.maxSize = data.maxSize;
                itemCalloutComponent.level = data.level;

                item.SetActive(true);
            }
            else if (data.itemType == ItemType.Relationship)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Relationship", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform, false);
                item.SetActive(false);

                item.transform.position = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                item.transform.rotation = Quaternion.Euler(new Vector3(data.xRotation, data.yRotation, data.zRotation));

                Relationship itemRelationshipComponent = item.GetComponent<Relationship>();

                foreach (GameObject go in GameObject.FindObjectsOfType(typeof(GameObject)))
                {
                    Debug.Log(go.name);
                    if (go.transform.position == new Vector3(data.object1x, data.object1y, data.object1z))
                    {
                        itemRelationshipComponent.object1 = go;
                    }
                    else if (go.transform.position == new Vector3(data.object2x, data.object2y, data.object2z))
                    {
                        itemRelationshipComponent.object2 = go;
                    }
                }

                item.SetActive(true);
            }
        }

        return newMindMap;
    }
}
