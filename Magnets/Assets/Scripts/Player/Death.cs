using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject magnetGun;
    private void Awake()
    {
        if(GameProgression.Instance != null && GameProgression.Instance.checkPointPos != Vector3.zero)
        {
            transform.position = GameProgression.Instance.checkPointPos;

            Debug.Log("Spawned");

            if (GameProgression.Instance.level > 0)
                magnetGun.SetActive(true);
        }
        else
        {
            transform.position = new Vector3(0, 1, 0);
        }
        if(GameProgression.Instance != null)
            GameProgression.Instance.UpdateLeaderBoards();
    }
}