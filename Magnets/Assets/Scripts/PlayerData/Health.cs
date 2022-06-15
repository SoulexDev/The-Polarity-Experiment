using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour, IPlayer
{
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject deathScreen;
    private float health = 100;
    private float maxHealth = 100;
    private List<MagnetGuns> mg = new List<MagnetGuns>();
    private void Awake()
    {
        //AwakeGameProgression();
    }
    private void Start()
    {
        MagnetGuns[] temp = FindObjectsOfType<MagnetGuns>();
        foreach (MagnetGuns magGun in temp)
        {
            mg.Add(magGun);
        }
        if (mg.Count > 2)
        {
            mg.RemoveAt(3);
            mg.RemoveAt(2);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthBar.fillAmount = health / maxHealth;
        if(health <= 0)
        {
            Player.Instance.dead = true;
            DisplayDeathScreen();
        }
    }
    void DisplayDeathScreen()
    {
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AwakeGameProgression();
    }
    public void Respawn()
    {
        foreach (MagnetGuns magnetGun in mg)
        {
            magnetGun.ConnectMagnet();
        }
        mg.Clear();
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Player.Instance.dead = false;
        Player.Instance.canMove = true;
        health = maxHealth;
        healthBar.fillAmount = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void AwakeGameProgression()
    {
        if (GameProgression.Instance != null && GameProgression.Instance.checkPointPos != Vector3.zero)
        {
            GameProgression.Instance.minutes = 0;
            GameProgression.Instance.seconds = 0;
            transform.position = GameProgression.Instance.checkPointPos;
        }
        else
        {
            transform.position = new Vector3(0, 1, 0);
        }
        //if (GameProgression.Instance != null)
        //    GameProgression.Instance.UpdateLeaderBoards();
    }
}