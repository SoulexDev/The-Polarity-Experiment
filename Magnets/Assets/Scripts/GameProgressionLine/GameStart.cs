using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private InteractEvents[] events;
    private void Start()
    {
        if(Player.Instance == null)
        {
            Instantiate(player, new Vector3(0, 1, 0), Quaternion.identity);
        }
        GameObject mP = GameObject.Find("MagnetGunP");
        GameObject mN = GameObject.Find("MagnetGunN");
        events[0].interactEvent.AddListener(() => mP.SetActive(true));
        events[1].interactEvent.AddListener(() => mN.SetActive(true));
        if(mP != null)
            mP.SetActive(false);
        if(mN != null)
            mN.SetActive(false);
    }
}