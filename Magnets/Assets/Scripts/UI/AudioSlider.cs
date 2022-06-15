using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private string parameterName;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI valueText;
    public PlayerPreferenceManager.ValueType valueType;

    public void ChangeVolume()
    {
        float adjustedValue = Mathf.Log10(slider.value) * 20;
        if (slider.value == 0)
            adjustedValue = -80;
        mixer.SetFloat(parameterName, adjustedValue);
        valueText.text = slider.value.ToString("f1");
    }
    private void OnEnable()
    {
        slider.value = PlayerPrefs.GetFloat(parameterName);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(parameterName, slider.value);
        PlayerPrefs.Save();
        slider.value = PlayerPrefs.GetFloat(parameterName);
        ChangeVolume();
    }
}