using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raisable : MonoBehaviour, IReceptical
{
    [SerializeField] private int connectionAmount = 0;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private Vector3 direction;
    private AudioSource source;
    List<ISource> sources = new List<ISource>();
    bool isActivated { get { return sources.Count >= connectionAmount; } }
    private Vector3 openPosition;
    private Vector3 closedPosition;
    bool audioPlayed = false;
    private void Awake()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + direction;
        source = GetComponent<AudioSource>();
    }
    public void Activate(ISource source)
    {
        if (sources.Contains(source))
            return;
        else
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
    private void Update()
    {
        if (isActivated)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, speed);
            if (!audioPlayed)
            {
                if (clips.Length <= 0)
                    return;
                source.PlayOneShot(clips[0]);
                audioPlayed = true;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, speed);
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