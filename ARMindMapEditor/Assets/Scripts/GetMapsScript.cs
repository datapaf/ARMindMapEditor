using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetMapsScript : MonoBehaviour
{
    void Start()
    {
        var info = new DirectoryInfo("Assets/Resources/Maps");
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            GameObject newButton = Instantiate((GameObject)Resources.Load("Prefabs/UI/MapButton", typeof(GameObject)));
            newButton.transform.SetParent(gameObject.transform, false);

            string mapName = Path.GetFileNameWithoutExtension(file.FullName);
            
            newButton.transform.GetChild(0).GetComponent<Text>().text = mapName;
            newButton.GetComponent<MapButton>().mapName = mapName;
        }
    }

    void Update()
    {
        
    }
}
