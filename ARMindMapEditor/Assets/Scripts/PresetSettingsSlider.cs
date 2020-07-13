using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetSettingsSlider : MonoBehaviour
{
    private bool isAllowedToUpdateValue = true;

    private void Update()
    {
        if (isAllowedToUpdateValue)
        {
            gameObject.GetComponent<Slider>().value = GameObject.FindObjectOfType<PresetSettings>().presetMapSize;
        }
    }

    public void ChangeSize()
    {
        isAllowedToUpdateValue = false;
        GameObject.FindObjectOfType<PresetSettings>().presetMapSize = gameObject.GetComponent<Slider>().value;
        isAllowedToUpdateValue = true;
    }
}
