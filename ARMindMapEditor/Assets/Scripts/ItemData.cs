using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Shape { Sphere, Parallelopipedon, Capsule };
public enum ItemType { CT, MT, Subtopic, Callout, FT, Relationship };

public class ItemData
{
    public ItemType itemType;
    public string text;
    public float size;
    public float minSize;
    public float maxSize;
    public float r;
    public float g;
    public float b;
    public Shape shape;
    public int level;

    public float object1x;
    public float object1y;
    public float object1z;

    public float object2x;
    public float object2y;
    public float object2z;
}
