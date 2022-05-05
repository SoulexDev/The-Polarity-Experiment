using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Death : MonoBehaviour
{
    [SerializeField] private GameObject magnetGun;
    private void Start()
    {
        if(GameProgression.Instance.checkPointPos != Vector3.zero)
        {
            transform.position = GameProgression.Instance.checkPointPos;

            Debug.Log("Spawned");

            if (GameProgression.Instance.level > 0)
                magnetGun.SetActive(true);
        }
        GameProgression.Instance.UpdateLeaderBoards();
    }
}