using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public delegate void PauseEvent();
    public static event PauseEvent OnPause;
    public static bool paused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private List<GameObject> uIScreens = new List<GameObject>();
    List<AudioSource> sources = new List<AudioSource>();

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            OnPause();
        }
    }

    public void Pause()
    {
        paused = !paused;
        foreach (var source in FindObjectsOfType<AudioSource>(true))
        {
            if(paused)
            {
                if (source.isPlaying)
                {
                    sources.Add(source);
                    source.Pause();
                }
            }
            else
            {
                if (sources.Contains(source))
                {
                    sources.Remove(source);
                    source.Play();
                }
            }
        }
        if (!paused)
        {
            foreach (GameObject screen in uIScreens)
            {
                screen.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1;
            Player.Instance.canMove = true;
        }
        else
        {
            pauseMenu.SetActive(true);
            mainMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
            Player.Instance.canMove = false;
        }   
    }
}