using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;

    private void Awake()
    {

    }
    public void SwitchQuality()
    {
        switch (dropDown.value)
        {
            case 0:
                QualitySettings.SetQualityLevel(2);
                break;
            case 1:
                QualitySettings.SetQualityLevel(1);
                break;
            case 2:
                QualitySettings.SetQualityLevel(0);
                break;
            default:
                break;
        }
    }
    private void OnEnable()
    {
        dropDown.value = PlayerPrefs.GetInt("GraphicsQuality");
    }
    private void OnDisable()
    {
        PlayerPrefs.SetInt("GraphicsQuality", dropDown.value);
        PlayerPrefs.Save();
        dropDown.value = PlayerPrefs.GetInt("GraphicsQuality");

        SwitchQuality();
    }
}