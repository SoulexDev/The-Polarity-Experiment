using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ToxicGoo : MonoBehaviour
{
    [SerializeField] private List<MagnetGuns> mg = new List<MagnetGuns>();
    private void Start()
    {
        MagnetGuns[] temp = FindObjectsOfType<MagnetGuns>();
        foreach (MagnetGuns magGun in temp)
        {
            mg.Add(magGun);
        }
        if(mg.Count > 2)
        {
            mg.RemoveAt(3);
            mg.RemoveAt(2);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if(GameProgression.Instance != null)
            {
                GameProgression.Instance.minutes = 0;
                GameProgression.Instance.seconds = 0;
            }
            foreach (MagnetGuns magnetGun in mg)
            {
                magnetGun.ConnectMagnet();
            }
            mg.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
