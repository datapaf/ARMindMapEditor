﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { CT, MT, Subtopic, Callout, FT, Relationship };

[System.Serializable]
public class ItemData
{
    public float xPosition;
    public float yPosition;
    public float zPosition;

    public ItemType itemType;
    public string text;
    public float size;
    public float minSize;
    public float maxSize;
    public NodeColor.ColorType nodeColor;
    public Shape.ShapeType shapeType;
    public int level;

    public int relationshipIndex;
    public List<int> nextNodesIndices;

    public int object1NumberAsChild;
    public int object2NumberAsChild;
}
