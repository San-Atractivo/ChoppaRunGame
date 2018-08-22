using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utills;

public class GameManager : MonoBehaviour {

    public static GameManager SingleTon;    // 자주 사용되기 때문에 싱글톤 클래스

    int backgroundColorFlag;                // 백그라운드 컬러 플래그 변수
    int gameSpeed = 1;                      // 게임 스피드 변수
    int score;
    float distance;                         // 플레이어 이동 거리 변수
    bool menuFlag = false;                  // 메뉴 플래그 변수

    AudioSource audio;                      // 오디오 스크립트
    Color backgroundColor;                  // 백그라운드 컬러 변수

    public Lodding Lodding;                 // 로딩화면 스크립트

    public Text ScoreText;
    public Text distanceText;               // 플레이어 이동 거리 TextUI

    GameObject ScoreUI;
    public GameObject Menu;                 // 메뉴 UI
    public GameObject Background;           // 백그라운드 오브젝트
    public GameObject[] Grounds;            // 스테이지 오브젝트
    public GameObject Player;               // 플레이어 오브젝트
    

    // Use this for initialization
    void Start()
    {
        LoddingViewActive(startGame);       // 로딩화면

        // 기본 초기화
        Player.SetActive(false);            
        audio = GetComponent<AudioSource>();

        SetGameSpeed(0);                   
        backgroundColor = new Color(1, 1, 1);
        distanceText.fontSize = (int)((25f * (Screen.height * 0.2f)) / 72);
        distanceText.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height * 0.2f);

        if (SingleTon == null) SingleTon = this;

        Player.GetComponent<Player>().SetGameManager(SingleTon);

        // 설정창 프레임 표시 확인 후 스크립트 추가 
        if (DATA.getData().FPSView) this.gameObject.AddComponent<FPSDisplay>();

        ScoreUI = Instantiate(Resources.Load("Prefab/DeadCutScene")) as GameObject;
        ScoreUI.SetActive(false);

        // 초기화 준비 완료 로딩화면 지워도 된다는 신호
        Lodding.SetCoroutineFlag(true);
    }

    /**
     * 게임 시작 시 호출함수
     */
    void startGame()
    {
        Player.SetActive(true);     // 플레이어 캐릭터 보이기
        SetGameSpeed(1);            // 게임 스피드 설정
        BGMActive(true);            // BGM Play
        backgroundColorFlag = 0;
        StartCoroutine(PlayerTravelDistance());     // 플레이어 이동 거리 UI 코루틴
    }

    /**
     * 스테이지 생성 함수 (현재 진행중인 맵의 총 길이, 현재 position의 x값)
     */
    public void InstaniateGround(float mapLength, float x)
    {
        // 스테이지 중 랜덤의 스테이지를 생성
        GameObject obj = Instantiate(Grounds[UnityEngine.Random.Range(1, Grounds.Length)]) as GameObject;

        // 현재 깔려있는 맵의 길이에 + 현제x 값을 더한 후 생성시간과 게임 스피드를 곱해 차이만큼을 다음 스테이지 위치에서 빼서 공백을 없앤다.
        obj.transform.position = new Vector3((mapLength + x) - (Time.deltaTime * gameSpeed), 0, 0.1f);
    }

    /**
     * 게임 스피드 Get, Set 함수
     */
    public int GetGameSpeed()
    {
        return gameSpeed;
    }
    public void SetGameSpeed(int value)
    {
        gameSpeed = value;
        // 게임이 정지한다면 백그라운드 이동속도도 정지하도록 함
        if (value == 0)
            Background.GetComponent<BackGround>().setSpeed(0);
        else if(value == 2)
            Background.GetComponent<BackGround>().setSpeed(BackGround.MAXSCROOLSPEED);
        else
            Background.GetComponent<BackGround>().setSpeed(BackGround.NOMALSCROLLSPEED);
    }

    /**
     * 플레이어 이동 거리 확인 및 TextUI변경 코루틴
     */
    IEnumerator PlayerTravelDistance()
    {
        while (true)
        {
            distance += GetGameSpeed() * (Time.deltaTime * 10);     // 시간 경과에 따라 플레이어 이동거리 추가
            TextUtills.ReflashCommaText(distanceText, (int)distance, " M");    // 이동거리 변경에 따라 UI Text 변경

            if ( backgroundColorFlag != (int)(distance * 0.01)%3)   // 현재 컬러와 이동거리가 다를경우 백그라운드 색 변경
                BackgroundColorChange();

            yield return new WaitForEndOfFrame();
        }
    }

    /**
     * Background 이미지의 쉐이더 컬러를 이동 거리에 따라 변경하는 함수
     */
    void BackgroundColorChange()
    {
        Renderer renderer = Background.GetComponent<Renderer>();

        int bgcNumber = (int)(distance * 0.01) % 3;
        backgroundColorFlag = bgcNumber;

        switch (bgcNumber)
        {
            case 0:
                renderer.material.SetColor("_Color", new Color(1,1,1));
                distanceText.color = new Color(0, 0, 0);
                break;
            case 1:
                renderer.material.SetColor("_Color", new Color(186f / 255f, 80f / 255f, 80f / 255f));
                distanceText.color = new Color(1, 1, 1);
                break;
            case 2:
                renderer.material.SetColor("_Color", new Color(68f / 255f, 68f / 255f, 68f / 255f));
                distanceText.color = new Color(1, 1, 1);
                break;
        }
    }

    /**
     * 메뉴 클릭 이벤트 함수
     */
    public void OnClickSetting()
    {
        menuFlag = !menuFlag;
        Time.timeScale = menuFlag ? 0 : 1;
        Menu.SetActive(menuFlag);
    }

    /**
     * 로비로 가기 버튼 이벤트 함수
     */
    public void GoToLobby()
    {
        OnClickSetting();
        Player.GetComponent<Player>().ForcedDead();
    }

    /**
     * 플레이어가 죽어서 게임이 끝날 경우 처리 함수
     */
    public void TheEnd()
    {
        ScoreUI.SetActive(true);
        ScoreUI.GetComponent<Score>().ScoreCalculation();
    }

    /**
     * BGM On/Off 함수
     */
    public void BGMActive(bool active)
    {
        if (active) audio.Play();
        else audio.Stop();
    }

    /**
     * 로딩화면 이벤트 등록 및 로딩화면 보여주는 함수
     */
    public void LoddingViewActive(Lodding.LoddingEvent action)
    {
        Lodding.OnAddEvent(action);
        Lodding.LoddingView(true);
    }

    /**
     * 로딩화면을 지워도 된다는 플래그 함수
     */
    public void LoddingComplete()
    {
        Lodding.SetCoroutineFlag(true);
    }

    /**
     * 플레이어 이동거리 Get 함수
     */
    public float GetDistance()
    {
        return distance;
    }

    /**
     * 스코어 Get 함수
     */
    public int GetScore()
    {
        return score;
    }

    /** 
     * 스코어 추가 함수
     */
    public void AddScore()
    {
        score += 10;
        TextUtills.ReflashCommaText(ScoreText, score);
    }
}
