using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { CT, MT, Subtopic, Callout, FT, Relationship };

[System.Serializable]
public class ItemData
{
    public float xPosition;
    public float yPosition;
    public float zPosition;

    public float xRotation;
    public float yRotation;
    public float zRotation;

    public ItemType itemType;
    public string text;
    public float size;
    public float minSize;
    public float maxSize;
    public float r;
    public float g;
    public float b;
    public Node.Shape shape;
    public int level;

    public float object1x;
    public float object1y;
    public float object1z;

    public float object2x;
    public float object2y;
    public float object2z;

    public ItemData()
    { }
}
