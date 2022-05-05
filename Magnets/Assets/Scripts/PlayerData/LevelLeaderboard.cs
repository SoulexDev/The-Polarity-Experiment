using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLeaderboard : MonoBehaviour
{
    public TextMeshProUGUI[] boardInfo;
    [SerializeField] private Transform board;
    private void Awake()
    {
        boardInfo = board.GetComponentsInChildren<TextMeshProUGUI>();
    }
}