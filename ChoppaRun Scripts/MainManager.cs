using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainManager : MonoBehaviour {

    //상태 변수
	private bool store = false;     // 상점 UI Flag 
    private int plus;               // 추가 버튼 Flag

    //계수 변수
    private int money;              // 플레이어 돈
    private int energy;             // 플레이어 에너지

    //캔버스
	public GameObject Store;        // 상점 UI
    public GameObject Plus;         // 확인창 UI
    public GameObject Menu;         // 메뉴 UI
    public GameObject SettingPanel; // 설정창 UI

    //메인 텍스트
    public Text Money;              // 플레이어 돈 TextUI
    public Text Energy;             // 플레이어 에너지 TextUI
    public Text BestScore;          // 베스트 스코어 TextUI
    public Text BestDistance;           // 베스트 거리 TextUI
    public Text EnergyChargeTime;   // 에너지 추가 시간 딜레이 TextUI
    public Text PlusText;

    //스토어 텍스트
    public Text Fast;               // 스킬 Fast 확률 TextUI
    public Text Shilde;             // 스킬 Shilde 확률 TextUI
    public Text Magnet;             // 스킬 Magnet 확률 TextUI

    //설정 메뉴
    public Toggle FPSview;          // 설정창 프레임표시 Toggle UI
        
    //스크립트
    AudioSource AudioSource;        // BGM 관리 오디오

	void Start() {
        // 변수 초기화 및 데이터 불러오기
        AudioSource = GetComponent<AudioSource>();
        money = DATA.getData().MONEY;
        energy = DATA.getData().ENERGY;
        int bestScore = DATA.getData().BESTSCORE;
        int bestDistance = DATA.getData().MAXDISTANCE;

        // 에너지 시간 체크
        EnergyTimeCheck();

        // TextUI text 추가
        ReflashEnergyText();
        ReflashCommaText(Money, money);
        ReflashCommaText(BestScore, bestScore);
        ReflashCommaText(BestDistance, bestDistance, " M");

        // 설정창 프레임표시 확인 및 적용
        initSettingMenu();
    }

    /**
     * 상점에서 스킬확률 변경시 확률변경 및 UI 변경 함수
     */
    private void SetStat()
    {
        float persent = (1.0f / DATA.getData().ALLSTAT) * 100;

        float fPersent = (Mathf.Round(((DATA.getData().FASTSTAT * persent) / 0.01f)) * 0.01f);
        float sPersent = (Mathf.Round(((DATA.getData().SHILDESTAT * persent) / 0.01f)) * 0.01f);
        float mPersent = (Mathf.Round(((DATA.getData().MAGNETSTAT * persent) / 0.01f)) * 0.01f);

        ReflashPersentText(Fast, fPersent, "%");
        ReflashPersentText(Shilde, sPersent, "%");
        ReflashPersentText(Magnet, mPersent, "%");
    }


    /**
     * 에너지 추가되는 시간 체크하는 함수
     */
    void EnergyTimeCheck()
    {
        DATA data = DATA.getData(); // PlayerPrefs 데이터를 관리하는 함수를 가져옴
        string nowTimeString = data.ENERGYTIME; // 에너지가 사용되는 마지막 시간을 가져옴

        // 에너지사용마지막 시간이 비어있거나, 에너지가 가득 자있을 경우 반환한다.
        if (nowTimeString.Equals("") || energy >= 7) return;

        
        System.DateTime saveTime = System.Convert.ToDateTime(nowTimeString); // 마지막 시간의 문자열을 시간형태로 변환
        System.DateTime nowTime = System.DateTime.Now;                       // 현재시간을 가져옴

        System.TimeSpan timeCal = nowTime - saveTime;                        // 두 시간 차를 구함

        int timeCalDay = timeCal.Days;      // 날짜 차이
        int timeCalHour = timeCal.Hours;    // 시간 차이
        int timeCalMin = timeCal.Minutes;   // 분 차이

        // 하루 이상 또는 1시간 이상 일 경우 풀에너지
        if(timeCalDay > 0 || timeCalHour > 0)
        {
            data.ENERGY = DATA.MAXENERGY;
            data.ENERGYTIME = "";
        }else // 분단위 일 경우 확인작업
        {
            int addEnergy = timeCalMin / 3; // 지나간 시간의 에너지 개수

            if( (energy + addEnergy) >= 7)  // 에너지 개수가 맥스일 경우
            {
                energy = DATA.MAXENERGY;
                data.ENERGY = DATA.MAXENERGY;
                data.ENERGYTIME = "";
            }
            else                            // 아닐 경우 시간 확인 및 타이머 
            {
                // 에너지 추가 및 데이터 저장
                energy += addEnergy;
                data.ENERGY = energy;

                // 추가된 에너지만큼의 시간 추가 및 마지막 시간 저장
                saveTime = saveTime.AddMinutes(addEnergy * 3);
                data.ENERGYTIME = Utills.DateTimeToString(saveTime);

                // 이후 남은 시간 카운팅코르틴 시작
                StartCoroutine(EnergyChargeText());
            }
        }
    }

    /**
     * 설정창의 프레임 표시 값을 확인 후 불러오며 데이터 저장 함수
     */
    void initSettingMenu()
    {
        bool fpsValue = DATA.getData().FPSView;
        FPSview.isOn = fpsValue;
    }

    /**
     * 플레이어 돈이 변경되는 애니메이션 코르틴함수
     */
    IEnumerator MoneyAnimation()
    {
        int saveMoney = DATA.getData().MONEY;

        while (money <= saveMoney)
        {
            yield return new WaitForSeconds(0.016f);

            int addMoney = money - saveMoney;
            for (int i = 1 ; addMoney != 0; i+=10)
            {
                money += 1 * i;
                addMoney /= 10;
            }
            ReflashCommaText(Money, money);
        }
    }

    /**
     * 에너지 추가 남은 시간을 확인 및 UI 변경하는 코르틴함수
     */
    IEnumerator EnergyChargeText()
    {
        DATA data = DATA.getData();                                             // 저장 데이터 클래스 가져오기
        System.DateTime saveTime = System.Convert.ToDateTime(data.ENERGYTIME);  // 저장된 마지막 시간 가져오기
        System.DateTime dateTime = System.DateTime.Now;                         // 현재시간 가져오기
        System.TimeSpan timeCal = dateTime - saveTime;                          // 현재시간과 저장된 시간 차이 구하기

        Debug.Log(timeCal.Seconds);
        int seconds = 180 - timeCal.Seconds;                                     // 초 구하기

        do
        {
            yield return new WaitForSeconds(1);                     
            seconds -= 1;
            ReflashEnergyTimeText(seconds);

            if (seconds == 0)
            {
                energy += 1;
                DATA.getData().ENERGY = energy;
                if (energy < 7) seconds = 180;
                ReflashEnergyText();
            }

        } while(energy < 7);

        ReflashEnergyTimeText(0);
    }

    /**
     * 확인창의 버튼 클릭시에 따른 이벤트처리함수
     */
    public void PlusButton(int value) // 0번이 행동력 1번이 돈
    {
        AudioSource.Play();
        switch (value)
        {
            case 0: // 에너지 추가 버튼 클릭시
                Plus.gameObject.SetActive(true);
                ReflashText(PlusText, "행동력을 충전 : Money -100");
                plus = 1;
                break;
            case 1: // 골드 추가 버튼 클릭시
                Plus.gameObject.SetActive(true);
                ReflashText(PlusText, "골드를 충전하시겠습니까 ? ");
                plus = 2;
                break;
            case 2: // 확인창의 확인버튼 클릭시
                if (plus == 1)
                {
                    if (DATA.getData().MONEY >= 100)
                    {
                        energy += DATA.MAXENERGY;
                        money -= 100;

                        DATA.getData().MONEY = money;
                        DATA.getData().ENERGY = energy;

                        ReflashEnergyText();
                    }
                }
                else if (plus == 2)
                {
                    DATA.getData().MONEY += 9999;
                }

                StartCoroutine(MoneyAnimation());
                Plus.gameObject.SetActive(false);
                plus = 0;
                break;
            case 3: // 확인창의 취소버튼 클릭시
                Plus.gameObject.SetActive(false);
                plus = 0;
                break;
        }
    }

    /**
     * 상점 버튼 클릭 이벤트처리 함수
     */
	public void StoreButton(int value) {
        AudioSource.Play();

        switch (value){
		case 0: // 상점 버튼 클릭시 
			if (!store) {store = true;Store.gameObject.SetActive (true);}
			else {store = false;Store.gameObject.SetActive (false);	}
			break;
		case 1: // fast 증가
			if (DATA.getData().MONEY >= Mathf.Abs(DATA.getData().FASTSTAT * -100) && DATA.getData().ALLSTAT < 10) {
				//DATA.MONEY += DATA.FASTSTAT * -100;
				DATA.getData().FASTSTAT++;
			}
			break;
		case 2: // fast 감소
			if (DATA.getData().MONEY >= 100 && DATA.getData().FASTSTAT > 1) {
				DATA.getData().FASTSTAT--;
			}
			break;
		case 3: // shilde 증가
			if (DATA.getData().MONEY >= Mathf.Abs(DATA.getData().SHILDESTAT * -100) && DATA.getData().ALLSTAT < 10) {
                    //DATA.MONEY += DATA.SHILDESTAT * -100;
                DATA.getData().SHILDESTAT++;
			}
			break;
		case 4: // shilde 감소
			if (DATA.getData().MONEY >= 100 && DATA.getData().SHILDESTAT > 1) {
				DATA.getData().SHILDESTAT--;
			}
			break;
		case 5: // magnet 증가
			if (DATA.getData().MONEY >= Mathf.Abs(DATA.getData().MAGNETSTAT * -100) && DATA.getData().ALLSTAT < 10) {
				//DATA.MONEY += DATA.MAGNETSTAT * -100;
                DATA.getData().MAGNETSTAT++;
			}
			break;
		case 6: // magnet 감소
			if (DATA.getData().MONEY >= 100 && DATA.getData().MAGNETSTAT > 1) {
				DATA.getData().MAGNETSTAT--;
			}
			break;
		case 7: // 아이템 1 
			break;
		case 8: // 아이템 2
			break;
		case 9: // 아이템 3
			break;
		}

		SetStat ();
		StartCoroutine (MoneyAnimation ());
	}

    /**
     * 리더보드 버튼 클릭시 이벤트함수
     */
    public void LeadboardButton()
    {
        AudioSource.Play();
        SocialManager.SetLeaderboardScore(DATA.getData().BESTSCORE);
        //SocialManager.AchievementsViewUI();
    }

    /**
     * 메뉴버튼 클릭시 이벤트 함수
     */
	public void MenuButton(){
        AudioSource.Play();
        Menu.SetActive(!Menu.active);
	}
		
    /**
     * 게임 플레이 버튼 이벤트함수
     */
	public void StartButton() {
        int energy = DATA.getData().ENERGY;

        if (energy > 0) {
            AudioSource.Play();
            energy -= 1;
            DATA.getData().ENERGY = energy;
            if (energy < 7)
            {
                if (DATA.getData().ENERGYTIME.Equals(""))
                {
                    DATA.getData().ENERGYTIME = Utills.GetNowDateTime();
                }
            }
            SceneManager.LoadScene("Game");
		}
	}

    /**
     * 설정창 버튼 이벤트함수
     */
    public void SettingButton()
    {
        AudioSource.Play();
        SettingPanel.SetActive(!SettingPanel.active);
    }

    /**
     * 프레임표시 값변경 이벤트함수
     */
    public void SettingFPSViewButton()
    {
        DATA.getData().FPSView = FPSview.isOn;
    }

    /**
     * 게임 종료 이벤트함수
     */
    public void GameExit()
    {
        AudioSource.Play();
        Application.Quit();
    }

    /**
     * 에너지 시간 Text 변경함수
     */
    private void ReflashEnergyTimeText(int seconds)
    {
        string sec = (seconds % 60) > 9 ? (seconds%60).ToString() : "0" + (seconds % 60).ToString();
        EnergyChargeTime.text = "0" + (seconds / 60) + " : " + sec;
    }

    /**
     * 에너지 Text 변경함수
     */
    private void ReflashEnergyText()
    {
        Energy.text = "" + energy + " / " + DATA.MAXENERGY;
    }

    /**
     * Text 변경함수
     */

    private void ReflashText(Text UI, string str)
    {
        UI.text = str;
    }
    private void ReflashCommaText(Text UI, int value)
    {
        UI.text = Utills.NumberToCommaText(value);
    }

    private void ReflashCommaText(Text UI, int value, string nextStr)
    {
        UI.text = Utills.NumberToCommaText(value) + nextStr;
    }

    private void ReflashPersentText(Text UI, float value, string nextStr)
    {
        UI.text = value + nextStr;
    }
}
