using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class Login : MonoBehaviour {

    public Text LoddingText;

    void Start()
    {
        SocialManager.LoginInit();
    }

    //void LoginInit()
    //{

    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //       .EnableSavedGames()
    //       .Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.DebugLogEnabled = true;
    //    PlayGamesPlatform.Activate();
    //}

    public void UserLoginCheck()
    {
        Debug.LogError("로그인 체크 시작");
        if (!SocialManager.UserLoginCheck()) SocialManager.UserLogin(UserGameStart);
    }

    //void UserLogin()
    //{
    //    Debug.Log("로그인 시작");
    //    //PlayGamesPlatform.Instance.Authenticate((bool success) =>
    //    Social.localUser.Authenticate((bool success) =>
    //    {
    //        Debug.LogError(success + " / 로그인 콜백 함수 실행");
    //        if (success)
    //        {
    //            StartCoroutine(GameStart());
    //        }
    //        Debug.LogError("로그인 콜백 함수 끝");
    //    });
    //}

    void UserGameStart()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        Debug.Log("로그인 완료 게임 시작");
      
        int height = Screen.height / 14;
        LoddingText.fontSize = height;
        LoddingText.gameObject.SetActive(true);

        float loddingTime = 0;
        do
        {
            if ((loddingTime % 1) == 0) LoddingText.text = "Lodding";
            else LoddingText.text += ".";
            yield return new WaitForSeconds(0.25f);
            loddingTime += 0.25f;
        } while (loddingTime < 4);
        SceneManager.LoadScene("Main");
    }

    public static void UserLogout()
    {
    }
}
