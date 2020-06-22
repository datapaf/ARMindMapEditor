using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MindMap : MonoBehaviour
{
    public enum DemonstrationMode { Volume, Flat };

    public new string name = "NewMindMap";

    // sizeMultiplier is the scalar that determines the size of all the objects
    public float sizeMultiplier = 1;

    // the variable determines the preview mode
    public bool isPreview = false;

    public DemonstrationMode mode;

    void Start()
    {

    }

    void Update()
    {
    }
}
