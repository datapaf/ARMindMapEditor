using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetSettings : MonoBehaviour
{
    public float presetMapSize = 1;

    public void ChangeMapSize()
    {
        presetMapSize = GameObject.Find("PresetMenuUI").transform.Find("Slider").GetComponent<Slider>().value;
    }
}
