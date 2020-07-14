using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EditorMenu : MonoBehaviour
{
    public GameObject mapNameInputField;
    public Sprite FlatModeButtonSprite;
    public Sprite VolumeModeButtonSprite;
    public GameObject FindNodeInputField;

    void Update()
    {
        if (GameObject.FindObjectOfType<MindMap>())
        {
            MindMap mindMap = GameObject.FindObjectOfType<MindMap>();

            if (mindMap.mode == DemonstrationMode.Flat)
            {
                transform.Find("ModeButton").GetComponent<Image>().sprite = VolumeModeButtonSprite;
            }
            else if (mindMap.mode == DemonstrationMode.Volume)
            {
                transform.Find("ModeButton").GetComponent<Image>().sprite = FlatModeButtonSprite;
            }
        }
    }

    public void Undo()
    {
        if (GameObject.FindObjectOfType<History>().currentStateIndex > 0)
        {
            GameObject.FindObjectOfType<History>().currentStateIndex--;
            Restore(GameObject.FindObjectOfType<History>().GetCurrentState());
            GameObject.FindObjectOfType<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
        }
    }

    public void Redo()
    {
        if (GameObject.FindObjectOfType<History>().currentStateIndex + 1 < GameObject.FindObjectOfType<History>().states.Count)
        {
            GameObject.FindObjectOfType<History>().currentStateIndex++;
            Restore(GameObject.FindObjectOfType<History>().GetCurrentState());
            GameObject.FindObjectOfType<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
        }
    }

    public static MindMapData CreateState()
    {
        GameObject map = GameObject.FindObjectOfType<MindMap>().gameObject;

        // create the entry containing the data about the whole map
        MindMapData mindMapData = new MindMapData();

        mindMapData.xPosition = map.transform.position.x;
        mindMapData.yPosition = map.transform.position.y;
        mindMapData.zPosition = map.transform.position.z;

        mindMapData.xRotation = map.transform.eulerAngles.x;
        mindMapData.yRotation = map.transform.eulerAngles.y;
        mindMapData.zRotation = map.transform.eulerAngles.z;

        // initialize the fields of the new entry
        mindMapData.mapName = map.GetComponent<MindMap>().mapName;
        mindMapData.sizeMultiplier = map.GetComponent<MindMap>().sizeMultiplier;
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
            data.xPosition = item.transform.localPosition.x;
            data.yPosition = item.transform.localPosition.y;
            data.zPosition = item.transform.localPosition.z;


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
                data.nodeColor = item.GetComponent<Node>().nodeColor;
                data.shapeType = item.GetComponent<Node>().shapeType;
                data.level = item.GetComponent<Node>().level;
                data.isHidden = item.GetComponent<Node>().isHidden;

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
                data.isHidden = item.GetComponent<Callout>().isHidden;
            }
            else if (item.GetComponent<Relationship>())
            {
                data.object1NumberAsChild = item.GetComponent<Relationship>().object1.transform.GetSiblingIndex();
                data.object2NumberAsChild = item.GetComponent<Relationship>().object2.transform.GetSiblingIndex();
            }

            // adding the collected data to the item entry
            mindMapData.items.Add(data);
        }

        return mindMapData;
    }

    public static void Restore(MindMapData state)
    {
        Destroy(GameObject.FindObjectOfType<MindMap>().gameObject);

        // instantiate the new empty map
        GameObject newMindMap = Instantiate((GameObject)Resources.Load("Prefabs/EmptyMindMap", typeof(GameObject)));

        newMindMap.transform.position = new Vector3(state.xPosition, state.yPosition, state.zPosition);

        // retrieve the info about the map from the entry 
        newMindMap.GetComponent<MindMap>().mapName = state.mapName;
        newMindMap.GetComponent<MindMap>().sizeMultiplier = state.sizeMultiplier;
        newMindMap.GetComponent<MindMap>().mode = state.mode;

        // going through each item that is in the map we set up their parameters
        foreach (ItemData data in state.items)
        {
            GameObject item;

            if (data.itemType == ItemType.CT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/CT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                item.transform.localPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                //item.transform.localRotation = newMindMap.transform.localRotation;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.nodeColor = data.nodeColor;
                itemNodeComponent.shapeType = data.shapeType;
                itemNodeComponent.level = data.level;
                itemNodeComponent.isHidden = data.isHidden;
            }
            else if (data.itemType == ItemType.MT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/MT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                item.transform.localPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                //item.transform.localRotation = newMindMap.transform.localRotation;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.nodeColor = data.nodeColor;
                itemNodeComponent.shapeType = data.shapeType;
                itemNodeComponent.level = data.level;
                itemNodeComponent.isHidden = data.isHidden;
            }
            else if (data.itemType == ItemType.Subtopic)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Subtopic", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                item.transform.localPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                //item.transform.localRotation = newMindMap.transform.localRotation;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.nodeColor = data.nodeColor;
                itemNodeComponent.shapeType = data.shapeType;
                itemNodeComponent.level = data.level;
                itemNodeComponent.isHidden = data.isHidden;
            }
            else if (data.itemType == ItemType.FT)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                item.transform.localPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                //item.transform.localRotation = newMindMap.transform.localRotation;

                Node itemNodeComponent = item.GetComponent<Node>();

                itemNodeComponent.text = data.text;
                itemNodeComponent.size = data.size;
                itemNodeComponent.minSize = data.minSize;
                itemNodeComponent.maxSize = data.maxSize;
                itemNodeComponent.nodeColor = data.nodeColor;
                itemNodeComponent.shapeType = data.shapeType;
                itemNodeComponent.level = data.level;
                itemNodeComponent.isHidden = data.isHidden;
            }
            else if (data.itemType == ItemType.Callout)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                item.transform.localPosition = new Vector3(data.xPosition, data.yPosition, data.zPosition);
                //item.transform.localRotation = newMindMap.transform.localRotation;

                Callout itemCalloutComponent = item.GetComponent<Callout>();

                itemCalloutComponent.text = data.text;
                itemCalloutComponent.size = data.size;
                itemCalloutComponent.minSize = data.minSize;
                itemCalloutComponent.maxSize = data.maxSize;
                itemCalloutComponent.level = data.level;
                itemCalloutComponent.isHidden = data.isHidden;
            }
            else if (data.itemType == ItemType.Relationship)
            {
                item = Instantiate((GameObject)Resources.Load("Prefabs/Items/Relationship", typeof(GameObject)));
                item.transform.SetParent(newMindMap.transform);

                Relationship itemRelationshipComponent = item.GetComponent<Relationship>();

                // here we obtain the objects that the relationship connects
                itemRelationshipComponent.object1 = newMindMap.transform.GetChild(data.object1NumberAsChild).gameObject;
                itemRelationshipComponent.object2 = newMindMap.transform.GetChild(data.object2NumberAsChild).gameObject;
            }
        }

        int index = 0;
        foreach (ItemData data in state.items)
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


        newMindMap.transform.eulerAngles = new Vector3(state.xRotation, state.yRotation, state.zRotation);
    }

    public void ChangeMapName()
    {
        GameObject mindMap = FindObjectOfType<MindMap>().gameObject;

        bool doExist = false;

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            string mapName = Path.GetFileNameWithoutExtension(file.FullName);
            if (mapName == mapNameInputField.GetComponent<InputField>().text)
            {
                doExist = true;
            }
        }

        if (doExist == false && mapNameInputField.GetComponent<InputField>().text != "")
        {
            mindMap.GetComponent<MindMap>().prevName = mindMap.GetComponent<MindMap>().mapName;
            mindMap.GetComponent<MindMap>().mapName = mapNameInputField.GetComponent<InputField>().text;
        }
        else
        {
            mapNameInputField.GetComponent<InputField>().text = mindMap.GetComponent<MindMap>().mapName;
        }

        // save the new name
        GameObject.Find("SaveController").GetComponent<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeDemonstrationMode()
    {
        MindMap mindMap = GameObject.FindObjectOfType<MindMap>();
        if (mindMap.mode == DemonstrationMode.Flat)
        {
            mindMap.mode = DemonstrationMode.Volume;
        }
        else if (mindMap.mode == DemonstrationMode.Volume)
        {
            mindMap.mode = DemonstrationMode.Flat;
        }

        GameObject.FindObjectOfType<SaveController>().SaveMap(GameObject.FindObjectOfType<MindMap>().gameObject);
        GameObject.FindObjectOfType<History>().Push(EditorMenu.CreateState());
    }

    public void ActivateFindNodeInputField()
    {
        FindNodeInputField.SetActive(true);
        FindNodeInputField.GetComponent<InputField>().Select();
        FindNodeInputField.GetComponent<InputField>().ActivateInputField();
    }

    public void FindNode()
    {

        if (FindNodeInputField.GetComponent<InputField>().text.Length != 0)
        {
            var nodes = GameObject.FindObjectsOfType<Node>();
            foreach (Node node in nodes)
            {
                if (node.text.Contains(FindNodeInputField.GetComponent<InputField>().text))
                {
                    GameObject.FindObjectOfType<SelectionManager>().hitNode = node.gameObject;
                    GameObject.FindObjectOfType<SelectionManager>().Select();
                    GameObject.FindObjectOfType<TouchController>().state = 3;
                    break;
                }
            }
        }


        FindNodeInputField.SetActive(false);
    }
}
