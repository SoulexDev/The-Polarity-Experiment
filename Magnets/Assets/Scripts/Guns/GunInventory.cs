using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons = new List<GameObject>();
    private List<GameObject> registeredWeapons = new List<GameObject>();
    private void Awake()
    {
        registeredWeapons.Add(weapons[0]);
        registeredWeapons.Add(weapons[1]);
    }
    void Update()
    {
        PlayerInput();
    }
    public void PickUpWeapon(GameObject weapon)
    {
        if (weapons.Contains(weapon))
            registeredWeapons.Add(weapons[weapons.IndexOf(weapon)]);
    }
    void PlayerInput()
    {
        //SwitchWeapons(int.Parse(Input.inputString) - 1);
        switch (Input.inputString)
        {
            case "1":
                SwitchWeapons(0);
                break;
            case "2":
                SwitchWeapons(1);
                break;
            default:
                break;
        }
    }
    void SwitchWeapons(int weaponID)
    {
        if(weaponID <= registeredWeapons.Count - 1)
        {
            GameObject gun = registeredWeapons[weaponID];
            gun.SetActive(true);
            for (int i = 0; i < registeredWeapons.Count; i++)
            {
                if(registeredWeapons[i] != gun)
                    registeredWeapons[i].SetActive(false);
            }
        }
    }
}