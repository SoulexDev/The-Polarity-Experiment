using LootLocker.Requests;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.Assertions;

public class GameProgression : MonoBehaviour
{
    public static GameProgression Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        profanityBlockList = textAssetBlockList.text.Split(new string[] { "", "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries);
    }

    [SerializeField] private TMP_InputField playerID;
    [SerializeField] private GameObject invalidNotif;
    [SerializeField] private TextAsset textAssetBlockList;
    static string[] profanityBlockList;
    private string memberID;
    [SerializeField] private List<int> levelLeaderboardKeys = new List<int>();
    public LevelLeaderboard[] levelLeaderboards;
    public Vector3 checkPointPos = Vector3.zero;
    public int level = -1;

    public int minutes = 0;
    public float seconds = 0;
    string strTime => minutes.ToString() + ":" + (seconds < 10 ? "0" + Mathf.Floor(seconds).ToString(): Mathf.Floor(seconds).ToString("f0"));
    int scoreTime => minutes * 60 + Mathf.RoundToInt(seconds);

    int leaderboardID => levelLeaderboardKeys[level];
    [SerializeField] private List<Vector3> checks = new List<Vector3>();

    public void StartGame()
    {
        if (playerID.text == "" || playerID.text.Length > 15 || playerID.text.Length < 6)
        {
            invalidNotif.SetActive(true);
            return;
        }
        memberID = ProfanityCheck(playerID.text);
        Login();
        minutes = 0;
        seconds = 0;
        SceneManager.LoadSceneAsync("GameScene");
    }
    private void Update()
    {
        seconds += Time.deltaTime;
        if(seconds >= 60)
        {
            minutes++;
            seconds -= 60;
        }
    }
    public void EnterNewLevel(Vector3 check)
    {
        if (checks.Contains(check))
            return;
        else
            checks.Add(check);

        level++;
        checkPointPos = check;
        LootLockerSDKManager.SubmitScore(memberID, scoreTime, leaderboardID.ToString(), memberID, (response) =>
        {
            if (response.statusCode == 200)
            {
                Debug.Log("Successful");
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
        minutes = 0;
        seconds = 0;
        UpdateLeaderBoards();
    }
    void Login()
    {
        LootLockerSDKManager.StartGuestSession(memberID, (response) =>
        {
            if (response.success)
            {
                Debug.Log("successfully started LootLocker session");
                UpdateLeaderBoards();
            }
            else
            {
                Debug.Log(response.Error);
            }
        });
    }
    public void UpdateLeaderBoards()
    {
        for (int i = 0; i < levelLeaderboards.Length; i++)
        {
            Display(i);
        }
    }
    void Display(int i)
    {
        LootLockerSDKManager.GetScoreList(levelLeaderboardKeys[i], 10, (response) =>
        {
            if (response.statusCode == 200)
            {
                LootLockerLeaderboardMember[] scores = response.items;
                for (int w = 0; w < scores.Length; w++)
                {
                    levelLeaderboards[i].boardInfo[w].text = scores[w].rank + ". " + ProfanityCheck(scores[w].metadata, "***") + ": " + scores[w].score;
                }
                if (scores.Length < 10)
                {
                    for (int a = scores.Length; a < 10; a++)
                    {
                        levelLeaderboards[i].boardInfo[a].text = (a + 1).ToString() + " none";
                    }
                }
            }
            else
            {
                Debug.Log("failed: " + response.Error);
            }
        });
    }
    static string ProfanityCheck(string inputStr, string replacement = "")
    {
        Assert.IsTrue(profanityBlockList.Length > 0, "ProfanityList is empty!");
        inputStr = Regex.Replace(inputStr, @"[^a-zA-Z0-9\-_]", "");
        inputStr = Regex.Replace(inputStr, $"({string.Join("|", profanityBlockList)})", replacement, RegexOptions.IgnoreCase);
        return inputStr;
    }
}