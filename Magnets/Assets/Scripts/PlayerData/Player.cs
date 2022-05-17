using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector3 levelLoadPosition;
    public float camX, camY;
    public static Player Instance { get; private set; }
    bool dontDestroy = false;
    public MagnetGuns[] magnetGuns;
    public Vector3[] magnetPosRelative = new Vector3[2];
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
    private void Update()
    {
        if (transform.parent == null)
        {
            if (dontDestroy)
                return;
            DontDestroyOnLoad(gameObject);
            dontDestroy = true;
        }
        else 
            dontDestroy = false;
    }
    public void RetractMagnets()
    {
        foreach (MagnetGuns magnet in magnetGuns)
        {
            magnet.RetractMagnet();
        }
    }
    public void SaveMagnetState(Vector3 levelPos)
    {
        for (int i = 0; i < magnetGuns.Length; i++)
        {
            if(magnetGuns[i].magnetBone.parent == null)
                magnetPosRelative[i] = magnetGuns[i].magnetBone.position - levelPos;
        }
    }
    public void LoadMagnetState()
    {
        for (int i = 0; i < magnetGuns.Length; i++)
        {
            if (magnetGuns[i].magnetBone.parent == null)
                magnetGuns[i].magnetBone.position = magnetPosRelative[i];
        }
    }
}