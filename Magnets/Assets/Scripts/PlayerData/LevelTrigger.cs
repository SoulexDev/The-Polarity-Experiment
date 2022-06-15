using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelTrigger : MonoBehaviour
{
    public UnityEvent levelTrigger;
    bool eventPlayed = false;
    private void Start()
    {
        //if(GameProgression.Instance != null)
        //    levelTrigger.AddListener(() => GameProgression.Instance.EnterNewLevel(transform.position));
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 && !eventPlayed)
        {
            levelTrigger.Invoke();
            eventPlayed = true;
        }
    }
}