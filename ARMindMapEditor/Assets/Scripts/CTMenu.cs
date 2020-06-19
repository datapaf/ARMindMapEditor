using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void CreateFT()
    {
        GameObject mindMap = GameObject.Find("MindMap(Clone)").gameObject;
        GameObject CT = mindMap.transform.Find("CT").gameObject;
        GameObject CTModel = CT.transform.Find("Sphere(Clone)").gameObject;
        GameObject FT = Instantiate((GameObject)Resources.Load("Prefabs/Items/FT", typeof(GameObject)));
        FT.transform.SetParent(mindMap.transform, false);
        FT.transform.position = CT.transform.position + new Vector3(0, CTModel.transform.GetChild(0).localScale.y + 0.2f, 0);
        FT.transform.rotation = CT.transform.rotation;
    }

    public void CreateCallout()
    {
        GameObject mindMap = GameObject.Find("MindMap(Clone)").gameObject;
        GameObject CT = mindMap.transform.Find("CT").gameObject;
        GameObject CTModel = CT.transform.Find("Sphere(Clone)").gameObject;
        GameObject FT = Instantiate((GameObject)Resources.Load("Prefabs/Items/Callout", typeof(GameObject)));
        FT.transform.SetParent(mindMap.transform, false);
        FT.transform.position = CT.transform.position + new Vector3(0, CTModel.transform.GetChild(0).localScale.y + 0.2f, 0);
        FT.transform.rotation = CT.transform.rotation;
    }
}
