using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;

public class ResetGameObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string mapName = EasySave.Load<string>("ResetMapName");
        if (mapName != null)
        {
            GameObject.Find("Main Menu").GetComponent<MainMenu>().CreateMap();
            Spawner[] spawners = GameObject.FindObjectsOfType<Spawner>();
            foreach (var spawner in spawners)
            {
                spawner.doCreateWithCustomName = true;
                spawner.mapName = mapName;
            }
        }
        EasySave.Delete<string>("ResetMapName");
        Destroy(gameObject); 
    }
}
