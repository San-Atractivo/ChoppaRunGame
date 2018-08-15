using UnityEngine;
using UnityEngine.SocialPlatforms;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class SocialManager : MonoBehaviour {

    public static void LoginInit()
    {
#if UNITY_ANDROID
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
           .EnableSavedGames()
           .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
#endif
    }

    // 구글 플레이 유저 로그인 함수 ( 콜백 함수)
    public static void UserLogin( Action funtion)
    {
#if UNITY_ANDROID
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.LogError("로그인 성공");
                funtion();
            }
        });
#endif
    }

    // 구글 플레이 유저 로그인 확인 함수
    public static bool UserLoginCheck()
    {
        return Social.localUser.authenticated;
    }

    // 구글 플레이 리더보드 스코어 넣기 함수
    public static void SetLeaderboardScore(int score)
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard_best_score, (bool success) =>
        {
            if (success)
            {
                ViewLeaderboardUI();
            }
            else
            {

            }
        });
#endif
    }

    // 구글 플레이 리더보드 스코어 가져오기 함수
    // 실행 확인이 불명 
    // 이유 : 유니티가 업그레이드 되면서 구글플레이 함수가 작동이 안되는 것
    public static void GetLeaderboardScore()
    {
#if UNITY_ANDROID
        PlayGamesPlatform.Instance.LoadScores(GPGSIds.leaderboard_best_score, scores =>
        {
            if (scores.Length > 0)
            {
                Debug.Log("Got " + scores.Length + " scores");
                string myScores = "Leaderboard:\n";
                foreach (IScore score in scores)
                    myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
                Debug.Log(myScores);
            }
            else
                Debug.Log("No scores loaded");
        });
#endif
    }

    // 구글 플레이 리더보드 리스트 UI 호출 함수
    public static void ViewLeaderboardUI()
    {
        //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(GPGSIds.leaderboard_best_score);
        //if (!UserLoginCheck())
        //{
        //    Debug.LogError("Leaderboard Login Plase");
        //    UserLogin(PlayGamesPlatform.Instance.ShowLeaderboardUI);
        //}
        //else
        //{
        //    Debug.LogError("Leaderboard Login Not Plase");
        //    //PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_best_score);
        //    ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
        //}
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success) PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_best_score);
                else return;
            });
        }

        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_best_score);
    }

    // 구글 플레이 업적 확인 함수
    public static void AchievementsCheck(float distance)
    {
        if (UserLoginCheck())
        {
#if UNITY_ANDROID
            if (distance > 100)
                PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_100, 100f, null);
            if (distance > 200)
                PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_200, 100f, null);
            if (distance > 300)
                PlayGamesPlatform.Instance.ReportProgress(GPGSIds.achievement_300, 100f, null);
#endif
        }
    }

    public static void AchievementsViewUI()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success) Social.ShowAchievementsUI();
                else return;
            });
        }

        Social.ShowAchievementsUI();
    }
}
