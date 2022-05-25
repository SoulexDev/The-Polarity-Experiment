using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : MonoBehaviour
{
    private LineRenderer lineRend;
    public float timer = 1;
    [SerializeField] private float secondsAlive = 1;
    private void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
    }
    public void PlayEffect(Vector3[] positions)
    {
        lineRend.SetPositions(positions);
    }
    private void Update()
    {
        if(timer < 1)
        {
            lineRend.startWidth =  (1 - timer) / 5;
            timer += Time.deltaTime / secondsAlive;
        }
        else
        {
            timer = 1;
            gameObject.SetActive(false);
        }
    }
}