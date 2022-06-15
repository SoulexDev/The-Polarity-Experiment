using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreferenceManager : MonoBehaviour
{
    public static PlayerPreferenceManager Instance;
    public enum ValueType { Sensitivity, MasterVolume, SFXVolume, MusicVolume, GraphicsQuality }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void SetValue(ValueType valueType, object value)
    {
        switch (valueType)
        {
            case ValueType.Sensitivity:
                PlayerPrefs.SetFloat("Sensitivity", (float)value);
                break;
            case ValueType.MasterVolume:
                PlayerPrefs.SetFloat("Master", (float)value);
                break;
            case ValueType.SFXVolume:
                PlayerPrefs.SetFloat("SFXcontroller", (float)value);
                break;
            case ValueType.MusicVolume:
                PlayerPrefs.SetFloat("Music", (float)value);
                break;
            case ValueType.GraphicsQuality:
                PlayerPrefs.SetInt("GraphicsQuality", (int)value);
                break;
            default:
                break;
        }
    }
}