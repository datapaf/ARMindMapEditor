using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MindMapData
{
    public List<ItemData> items;
    public string mapName;
    public float sizeMultiplier;
    public bool isPreview;
    public DemonstrationMode mode;
}
