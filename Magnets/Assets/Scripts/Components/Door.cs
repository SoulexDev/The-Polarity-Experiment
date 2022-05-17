using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 0;
    [SerializeField] private float speed = 2;
    [SerializeField] private AudioClip[] clips;

    private AudioSource source;
    List<ISource> sources = new List<ISource>();
    [SerializeField] private Vector3 openPos = new Vector3(0, 3, 0);
    Vector3 closedPos;
    bool isActivated { get { return sources.Count >= connectionAmount; } }
    bool audioPlayed = false;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
        closedPos = transform.position;
        openPos += closedPos;
    }
    public void Activate(ISource source)
    {
        if (!sources.Contains(source))
            sources.Add(source);
    }

    public void Deactivate(ISource source)
    {
        if (sources.Contains(source))
            sources.Remove(source);
    }

    public void OverrideActivate()
    {
        connectionAmount = 0;
    }
    public void OverrideDeactivate()
    {
        connectionAmount = 50;
    }
    private void Update()
    {
        if (isActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPos, speed * Time.deltaTime * 10);

            if (clips.Length <= 0)
                return;
            if (!audioPlayed)
            {
                source.PlayOneShot(clips[0]);
                audioPlayed = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPos, speed * Time.deltaTime * 10);

            if (clips.Length <= 0)
                return;
            if (audioPlayed)
            {
                source.PlayOneShot(clips[1]);
                audioPlayed = false;
            }
        }
    }
}
