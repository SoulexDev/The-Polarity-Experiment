using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFollowBounds : MonoBehaviour
{
    [SerializeField] private Transform audioObject;
    private Transform player;
    Bounds bounds;
    private void Start()
    {
        bounds = GetComponent<Renderer>().bounds;
    }
    void Update()
    {
        if(player == null)
        {
            player = GameObject.Find("Player(Clone)").transform;
            return;
        }
        audioObject.position = new Vector3(Mathf.Clamp(player.position.x, bounds.min.x, bounds.max.x), transform.position.y, Mathf.Clamp(player.position.z, bounds.min.z, bounds.max.z));
    }
}