using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]

public class Player : MonoBehaviour {

    public SkillEvent[] skills; // Skill 이벤트 스크립트

    GameManager gm;
    Animator ani;                  // 캐릭터 애니메이션
    Rigidbody2D rigid;             // Rigidbody 

	public int[] gauge;            // 스킬 게이지 변수 - 0 fast, 1 shile, 2 magnet

    int jumpStack = 0;              // 2단점프 횟수 변수
    int jumpPower = 5;              // 점프 파워 변수

    bool activeSkillFast = false;   // 스킬Fast 사용 Flag 변수
    bool activeSkillShilde = false; // 스킬Shilde 사용 Flag 변수
    bool activeSkillMagnet = false; // 스킬Magnet 사용 Flag 변수

    bool dead = false;              // 플레이어 죽음 Flag 변수

    float fastpersent;              // 스킬 Fast 퍼센트 변수
    float shildepersent;            // 스킬 Shilde 퍼센트 변수

    Touch tempTouchs;               // 화면 터치 스크립트


    // Use this for initialization
    void Start () {
        // 기본 초기화
		gauge = new int[3];

        rigid = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();

        fastpersent = DATA.getData().FASTSTAT;
        shildepersent = fastpersent + DATA.getData().SHILDESTAT;
    }
	
	// Update is called once per frame
	void Update () {

        // 캐릭터가 지정된 위치보다 뒤에 있을 경우 앞으로 조금씩 이동하도록
        if (transform.position.x < 0 && transform.position.y > 0)   
            transform.position += new Vector3(1*Time.deltaTime, 0, 0);
        
        // 화면 터치시 , 점프 스택이 2미만일 경우, 스킬 Fast 사용중이 아닐 경우, 죽음상태가 아닐경우
        if(Input.touchCount > 0 && jumpStack < 2 && !activeSkillFast && !dead)
        {
            tempTouchs = Input.GetTouch(0);

            if (tempTouchs.phase == TouchPhase.Began)
            {
                jumpStack++;
                rigid.velocity = new Vector3(0, jumpPower, 0);      // 점프 파워만큼 위로 힘을 준다.
                ani.SetBool("Jump", true);                           // 애니메이션 플래그
                
                // 스킬 시전중이 아닐경우
                if (!activeSkillMagnet && !activeSkillShilde)
                    { ani.Play("Jump", -1, 0); }  // 점프 애니메이션 실행
            }
        }

        // PC 테스트용 점프키
        if (Input.GetKeyDown(KeyCode.Space) && jumpStack++ < 2 && !activeSkillFast && !dead)
        {
            rigid.velocity = new Vector3(0, jumpPower, 0);
            ani.SetBool("Jump", true);
            if (!activeSkillMagnet && !activeSkillShilde) { ani.Play("Jump", -1, 0); }
        }

        // 빠른 이동 스킬 발동중일 경우
        if (activeSkillFast)
        {
            if (2 - transform.position.y > 0.5f ) {
                transform.position += new Vector3(0, 0.5f, 0);
            }else if( transform.position.y < 2)
                transform.position += new Vector3(0, 2-transform.position.y, 0);
        }

    }

    /**
     * GameManager 변수 gm의 Set 함수
     */
    public void SetGameManager(GameManager gameManager)
    {
        gm = gameManager;
    }

    /**
     * 플레이어 죽음Flag 변수 Get 함수
     */
    public bool getDead()
    {
        return dead;
    }

    /**
     * 플레이어가 점프 후 바닥에 닿을 경우
     */
    void OnCollisionEnter2D(Collision2D col)
    {
        if (jumpStack != 0){
            jumpStack = 0;
            ani.SetBool("Jump", false);
        }
    }

    /**
     * 땅 밑으로 떨어져서 죽을 경우
     */
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dead && collision.tag == "Dead")
        {
            activeSkillShilde = false;
            PlayerDead();
        }
    }

    /**
     * 플레이어가 죽을 수 있는지 확인하는 함수
     */
    public bool PlayerDead()
    {
        if (activeSkillShilde || activeSkillFast) return false;

        ForcedDead();
        return true;
    }

    /**
     * 플레이어가 죽을 경우 처리 함수, 및 로비가기 시 플레이어 강제 죽음 함수
     */
    public void ForcedDead()
    {
        dead = true;
        rigid.velocity = new Vector3(0, 0, 0);
        gm.SetGameSpeed(0);
        ani.SetTrigger("Dead");
        gm.BGMActive(false);
        gm.TheEnd();
    }

    /**
     * 코인 획득시 호출 함수
     */
    public void AddCoin()
    {
        gm.AddScore();
        SetGauge();
    }

    /**
     * 코인 획득 후 게이지 계산 함수
     */
    public void SetGauge() {
        int value = Random.Range(0, DATA.getData().ALLSTAT);    // 랜덤 값 

        // 랜덤값이 퍼센트보다 값 사이 라면 발동
        // 해당 스킬의 게이지에 +1를 더한다.
        // 스킬 게이지가 100와 같을 경우 스킬 On
        if (value < fastpersent)
        {
            if (gauge[0] < 100)
            {
                gauge[0] += 1;
                if (gauge[0] == 100 && !skills[0].GetOnoff()) skills[0].OnSkillImage();
            }
        }
        else if (value >= fastpersent && value < shildepersent)
        {
            if (gauge[1] < 100)
            {
                gauge[1] += 1;
                if (gauge[1] == 100 && !skills[1].GetOnoff()) skills[1].OnSkillImage();
            }
        }
        else if (value >= shildepersent)
        {
            if (gauge[2] < 100)
            {
                gauge[2] += 1;
                if (gauge[2] == 100 && !skills[2].GetOnoff()) skills[2].OnSkillImage();
            }
        }
	}

    /**
     * 스킬 버튼 이벤트 함수 ( 0 = Fast , 1 = Shilde , 2 = Magenet)
     */
    public void BTNSkill(int skillBtnNumber)
    {
        if (activeSkillFast || activeSkillShilde || activeSkillMagnet) return;

        switch (skillBtnNumber)
        {
            case 0:
                if(gauge[skillBtnNumber] == 100) StartCoroutine(skillFast());
                break;
            case 1:
                if (gauge[skillBtnNumber] == 100) StartCoroutine(skillShilde());
                break;
            case 2:
                if (gauge[skillBtnNumber] == 100) StartCoroutine(skillMagnet());
                break;
        }
    }

    // 스킬 Fast 발동 코루틴
    IEnumerator skillFast()
    {
        // 스킬 발동을 위한 초기화
        gauge[0] = 0;
        rigid.velocity = new Vector3(0, 0, 0);
        rigid.gravityScale = 0;

        activeSkillFast = true;
        ani.SetBool("Jump", false);
        ani.SetBool("FastRun", true);
        gm.SetGameSpeed(2);
        
        // 스킬 사용 시간
        yield return new WaitForSeconds(3);

        // 스킬 발동 후 처리
        gm.SetGameSpeed(1);

        skills[0].OffSkillImage();
        rigid.gravityScale = 1;
        activeSkillFast = false;
        ani.SetBool("FastRun", false);
    }

    // 스킬 Fast 발동 코루틴
    IEnumerator skillShilde()
    {
        // 스킬 발동을 위한 초기화
        gauge[1] = 0;
        activeSkillShilde = true;
		ani.SetBool ("Shilde", true);

        // 스킬 사용 시간
        yield return new WaitForSeconds (10);

        // 스킬 발동 후 처리
        skills[1].OffSkillImage();
        activeSkillShilde = false;
		ani.SetBool ("Shilde", false);
	}

    // 스킬 Fast 발동 코루틴
    IEnumerator skillMagnet()
    {
        // 스킬 발동을 위한 초기화
        gauge[2] = 0;
        activeSkillMagnet = true;
		ani.SetBool ("Magnet", true);
        transform.Find("Magnet_Body").gameObject.SetActive(true);

        // 스킬 사용 시간
        yield return new WaitForSeconds (10);

        // 스킬 발동 후 처리
        skills[2].OffSkillImage();
        activeSkillMagnet = false;
		ani.SetBool ("Magnet", false);
        transform.Find("Magnet_Body").gameObject.SetActive(false);
    }
}
