using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GetMapsScript : MonoBehaviour
{
    void Start()
    {
        // here we create the buttons for each saved map

        // get information about all .json files in the directory
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles("*.json");
        foreach (var file in fileInfo)
        {
            // instatiate the button
            GameObject newButton = Instantiate((GameObject)Resources.Load("Prefabs/UI/MapButton", typeof(GameObject)));
            newButton.transform.SetParent(gameObject.transform, false);

            // get the name of the map that is stored in the current file
            string mapName = Path.GetFileNameWithoutExtension(file.FullName);
            
            // initialize texr on the button with the name of the map 
            newButton.transform.GetChild(0).GetComponent<Text>().text = mapName;
            
            // transfer the name of the map to the script on the button for the further loading by name
            newButton.GetComponent<MapButton>().mapName = mapName;
        }
    }

    void Update()
    {
    }
}
