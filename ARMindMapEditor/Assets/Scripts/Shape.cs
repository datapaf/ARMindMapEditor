using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public enum VolumeShape { Sphere, Parallelopipedon, Capsule };
    public enum FlatShape { Circle, Rectangle, Ellipse };
    public enum ShapeType { Type1, Type2, Type3 };

    public static FlatShape GetFlatShape(ShapeType shapeType)
    {
        if (shapeType == ShapeType.Type1)
            return FlatShape.Circle;
        else if (shapeType == ShapeType.Type2)
            return FlatShape.Rectangle;
        else
            return FlatShape.Ellipse;
    }

    public static VolumeShape GetVolumeShape(ShapeType shapeType)
    {
        if (shapeType == ShapeType.Type1)
            return VolumeShape.Sphere;
        else if (shapeType == ShapeType.Type2)
            return VolumeShape.Parallelopipedon;
        else
            return VolumeShape.Capsule;
    }
}
