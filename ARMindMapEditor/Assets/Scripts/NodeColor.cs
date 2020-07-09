using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeColor 
{
    public enum ColorType { 
        Red,
        Blue,
        White,
        Green,
        Purple,
        Indigo,
        Turquoise,
        Yellow,
        Orange,
        DarkRed,
        Grey
    };

    public static Color32 GetColor(ColorType colorType)
    {
        if (colorType == ColorType.Red)
            return new Color32(255, 0, 0, 255);
        else if (colorType == ColorType.Blue)
            return new Color32(0, 0, 255, 255);
        else if (colorType == ColorType.White)
            return new Color32(255, 255, 255, 255);
        else if (colorType == ColorType.Green)
            return new Color32(34, 177, 76, 255);
        else if (colorType == ColorType.Purple)
            return new Color32(163, 73, 164, 255);
        else if (colorType == ColorType.Indigo)
            return new Color32(63, 72, 204, 255);
        else if (colorType == ColorType.Turquoise)
            return new Color32(0, 162, 232, 255);
        else if (colorType == ColorType.Yellow)
            return new Color32(255, 242, 0, 255);
        else if (colorType == ColorType.Orange)
            return new Color32(255, 127, 39, 255);
        else if (colorType == ColorType.DarkRed)
            return new Color32(136, 0, 21, 255);
        else if (colorType == ColorType.Grey)
            return new Color32(127, 127, 127, 255);
        else
            return new Color32(0, 0, 0, 255);
    }
}
