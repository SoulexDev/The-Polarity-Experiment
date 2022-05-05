using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFollowBounds : MonoBehaviour
{
    [SerializeField] private Transform audioObject;
    private Transform player;
    Bounds bounds;
    private void Awake()
    {
        bounds = GetComponent<Renderer>().bounds;
        player = GameObject.Find("Player").transform;
    }
    void Update()
    {
        audioObject.position = new Vector3(Mathf.Clamp(player.position.x, bounds.min.x, bounds.max.x), transform.position.y, Mathf.Clamp(player.position.z, bounds.min.z, bounds.max.z));
    }
}