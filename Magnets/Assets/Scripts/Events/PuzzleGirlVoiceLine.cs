using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGirlVoiceLine : MonoBehaviour
{
    AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void PlayVoiceLine(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}