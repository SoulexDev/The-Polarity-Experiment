using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tabs : MonoBehaviour
{
    [SerializeField] private List<ButtonObjectPair> buttons;
    private void OnEnable()
    {
        foreach (ButtonObjectPair thisButton in buttons)
        {
            if (thisButton.currentlyActive)
                thisButton.button.Select();
        }
    }
    private void Awake()
    {
        buttons[0].currentlyActive = true;
        foreach (ButtonObjectPair button in buttons)
        {
            button.button.onClick.AddListener(() => SwitchTab(button));
        }
    }
    void SwitchTab(ButtonObjectPair button)
    {
        button.buttonObject.SetActive(true);
        button.currentlyActive = true;
        foreach (ButtonObjectPair thisButton in buttons)
        {
            if (thisButton != button)
            {
                thisButton.buttonObject.SetActive(false);
                thisButton.currentlyActive = false;
            }
        }
    }
}
[System.Serializable]
public class ButtonObjectPair
{
    public Button button;
    public GameObject buttonObject;
    public bool currentlyActive;
}