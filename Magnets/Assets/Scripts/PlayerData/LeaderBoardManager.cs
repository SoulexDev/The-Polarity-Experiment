using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private LevelLeaderboard[] levelLeaderboards;
    [SerializeField] private GameObject leaderboardUI;
    private void Start()
    {
        if(GameProgression.Instance != null)
            GameProgression.Instance.levelLeaderboards = levelLeaderboards;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            leaderboardUI.SetActive(!leaderboardUI.activeSelf);
        }
    }
}