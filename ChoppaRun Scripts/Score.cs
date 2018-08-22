using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Utills;

public class Score : MonoBehaviour {

    int score;

    public Text scoretext;

	public void ScoreCalculation()
    {
        // 플레이어 스코어값 가져오기
        score = GameManager.SingleTon.GetScore();

        // 게임매니저 최대이동거리 값 가져오기
        float distance = GameManager.SingleTon.GetDistance();

        // 저장된 거리값과 비교 - 크면 새로운 값 저장
        if (DATA.getData().MAXDISTANCE < distance)
        {
            DATA.getData().MAXDISTANCE = (int)distance;
            SocialManager.AchievementsCheck(distance);
        }

        // 저장된 스코어 비교 - 크면 새로운 값 저장
        if (DATA.getData().BESTSCORE < score)
        {
            DATA.getData().BESTSCORE = score;
        }

        // 스코어의 10% 돈으로 저장
        DATA.getData().MONEY += (score / 10);

        StartCoroutine(ScoreAnimation());
    }
	
    // 스코어 점수 올라가는 룰렛 실행
    IEnumerator ScoreAnimation()
    {
        int scoreview = 0;
        yield return new WaitForSeconds(1.3f);

        for (int i = 0; i< 20; i++)
        {
            scoreview += score / 20;
            scoretext.text = TextUtills.NumberToCommaText(scoreview);
            yield return new WaitForSeconds(0.05f);
        }
        scoretext.text = TextUtills.NumberToCommaText(score);
    }

    public void homeButton()
    {
        this.gameObject.SetActive(false);
        GameManager.SingleTon.LoddingViewActive(MainLoadScene);
        GameManager.SingleTon.LoddingComplete();
    }

    void MainLoadScene()
    {
        SceneManager.LoadScene("Main");
    }


}
