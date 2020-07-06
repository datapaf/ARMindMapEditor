using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DeleteMapButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DeleteMap()
    {
        File.Delete(Application.persistentDataPath + "/" + transform.parent.GetComponent<MapButton>().mapName + ".json");

        Destroy(transform.parent.gameObject);
    }
}
