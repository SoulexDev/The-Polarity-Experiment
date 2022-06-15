using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sensitivityText;
    [SerializeField] private Slider slider;

    public void ChangeSensitivity()
    {
        sensitivityText.text = slider.value.ToString();
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat("Sensitivity", slider.value);
        PlayerPrefs.Save();
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
        ChangeSensitivity();
    }
}