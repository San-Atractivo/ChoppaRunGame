using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

    public ParticleSystem particle; // 파티클 시스템

    bool touchFlag = false; // 자석효과 확인 플래그
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D col){
        // Debug.Log("coin tri"+col.transform.name);

        // 플레이어와 부딪히면
        if (col.tag.Equals("Player"))
        {
            audioSource.Play();
            touchFlag = true;
            col.transform.parent.GetComponent<Player>().AddCoin(); ;
            this.GetComponent<SpriteRenderer>().enabled = false;
            particle.Play();
        }
        else if (col.tag.Equals("Magnet"))  // 자석과 부딪히면
        {
            StartCoroutine(targetInComming(col.gameObject));
        }
    }

    /**
     * 플레이어가 자석을 사용했을 경우 따라가는 코르틴
     */
    IEnumerator targetInComming(GameObject target)
    {
        Vector3 distance = target.transform.position - transform.position;

        do
        {
            yield return new WaitForSeconds(0.01f);
            if (transform.position.x < target.transform.position.x) transform.position = target.transform.position;
            else transform.position = Vector3.Lerp(transform.position, target.transform.position, 3 * Time.deltaTime);
            
        } while (!touchFlag);
    }

    /**
     * 화면에 보여질 경우 자석판정 초기화
     */
    private void OnEnable()
    {
        touchFlag = false;
        particle.Stop();
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
