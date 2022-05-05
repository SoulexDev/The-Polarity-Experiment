using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToxicGoo : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            GameProgression.Instance.minutes = 0;
            GameProgression.Instance.seconds = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
