using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private UnityEvent preLoadEvent;
    [SerializeField] private UnityEvent postLoadEvent;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private bool levelStart;
    float vol;
    bool entered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3 && !entered)
        {
            preLoadEvent.Invoke();
            entered = true;
        }
    }
    private void Awake()
    {
        audioMixer.GetFloat("Volume", out vol);
        vol = Mathf.Pow(10, vol / 20);
        vol = Mathf.Clamp01(vol);
    }
    private void Start()
    {
        postLoadEvent.Invoke();
        if (levelStart)
            LoadState();
    }
    public void SaveState()
    {
        Player.Instance.camX = PlayerController.camX;
        Player.Instance.camY = PlayerController.camY;
        Player.Instance.levelLoadPosition = Player.Instance.transform.position - transform.position;
        Player.Instance.RetractMagnets();

        Player.Instance.SaveMagnetState(transform.position);
    }
    public void LoadState()
    {
        PlayerController.camX = Player.Instance.camX;
        PlayerController.camY = Player.Instance.camY;
        Player.Instance.transform.position = transform.position + Player.Instance.levelLoadPosition;

        Player.Instance.LoadMagnetState();
    }
    public void StartSetVolume(bool raiseVolume)
    {
        StartCoroutine(SetVolume(raiseVolume));
    }
    IEnumerator SetVolume(bool load)
    {
        if (load)
        {
            while(vol < 1)
            {
                vol += Time.deltaTime;
                vol = Mathf.Clamp01(vol);
                float adjustedValue = Mathf.Log10(vol) * 20;
                audioMixer.SetFloat("Volume", adjustedValue);
                yield return null;
            }
            vol = 1;
        }
        else
        {
            while(vol > 0)
            {
                vol -= Time.deltaTime;
                vol = Mathf.Clamp01(vol);
                float adjustedValue = Mathf.Log10(vol) * 20;
                audioMixer.SetFloat("Volume", adjustedValue);
                yield return null;
            }
            vol = 0;
        }
    }
    public void StartLevelLoad(string levelName)
    {
        SaveState();
        StartCoroutine(LoadNewLevel(levelName));
    }
    public IEnumerator LoadNewLevel(string levelName)
    {
        AsyncOperation levelLoad = SceneManager.LoadSceneAsync(levelName);

        while (!levelLoad.isDone)
        {
            yield return null;
        }
    }
}