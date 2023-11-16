using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static Leaderboard instance;
    void Awake(){ instance = this; }

    public void OnLoggedIn()
    {
        leaderboardCanvas.SetActive(true);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        //Debug.Log("DisplayLeaderboard was called");

        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "FastestTime",
            MaxResultsCount = 10
        };

        //Debug.Log("test 1");
        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
            );
        //Debug.Log("test 2");
    }

    void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        //Debug.Log("UpdateLeaderboardUI was called");

        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);

            if (x >= leaderboard.Count)
                continue;

            leaderboardEntries[x].transform.Find("PlayerName").GetComponent<TextMeshProUGUI>().text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardEntries[x].transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = (-(float)leaderboard[x].StatValue * 0.001f).ToString("F2");
        }
    }

    public void SetLeaderboardEntry(int newScore)
    {
        //Debug.Log("SetLeaderboardEntry was called");

        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateHighscore",
            FunctionParameter = new { score = newScore }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result =>
            {
                Debug.Log(result);
                DisplayLeaderboard();
                Debug.Log(result.ToJson());
            },
            error =>
            {
                Debug.Log(error.ErrorMessage);
                Debug.Log("ERROR");
            }
            );

    } 

    
}
