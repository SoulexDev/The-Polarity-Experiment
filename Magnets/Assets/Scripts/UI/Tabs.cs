using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tabs : MonoBehaviour
{
    [SerializeField] private List<ButtonObjectPair> buttons;
    private void Start()
    {
        foreach (ButtonObjectPair button in buttons)
        {
            button.button.onClick.AddListener(() => SwitchTab(button));
        }
    }
    void SwitchTab(ButtonObjectPair button)
    {
        button.buttonObject.SetActive(true);
        foreach (ButtonObjectPair thisButton in buttons)
        {
            if (thisButton != button)
                thisButton.buttonObject.SetActive(false);
        }
    }
}
[System.Serializable]
public class ButtonObjectPair
{
    public Button button;
    public GameObject buttonObject;
}