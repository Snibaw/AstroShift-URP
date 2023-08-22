using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using TMPro;

public class GPGSManager : MonoBehaviour
{
    private GameManager gameManager;
    private void Start()
    {
    #if UNITY_ANDROID
        PlayGamesPlatform.Instance.Authenticate(delegate(SignInStatus status) { });
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate((bool succes) => { });

        // Update Leaderboard
        // Social.ReportScore(PlayerPrefs.GetInt("Play",0), GPGSIds.leaderboard_nombre_de_parties_joues, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});
        // Social.ReportScore(PlayerPrefs.GetInt("HighScore",0), GPGSIds.leaderboard_meilleur_score_mode_infini, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});
        // Social.ReportScore(PlayerPrefs.GetInt("Level",1), GPGSIds.leaderboard_plus_haut_niveau_mode_campagne, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});
        // Social.ReportScore(long.Parse(PlayerPrefs.GetString("Money","0")), GPGSIds.leaderboard_argent_maximum, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});
        // Social.ReportScore(PlayerPrefs.GetInt("QuetesTerminees",0), GPGSIds.leaderboard_qutes_termines, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    #endif
    }
    public void ShowLeaderboardUI()
    {
        gameManager.PlayButtonSound();
        Social.ShowLeaderboardUI();
    }
    public void ReportHighScore(int score)
    {
        Social.ReportScore(score, GPGSIds.leaderboard_highscore, success => {Debug.Log(success ? "Reported score successfully" : "Failed to report score");});
    }
}
