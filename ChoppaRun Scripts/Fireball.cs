using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ObjectPool {

    Animator ani;
    AudioSource audio;
    private float speed = 6.5f; // 파이어볼 오브젝트 스피드

    bool ui;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.x < -5)
            removeObject();
        transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime * GameManager.SingleTon.GetGameSpeed()), transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // 플레이어와 부딪힐 경우
        if (col.tag.Equals("Player"))
        {            
            audio.Play();
            ani.SetTrigger("Explosion");
            col.gameObject.transform.parent.GetComponent<Player>().PlayerDead();
            GetComponent<CircleCollider2D>().enabled = false;   // 콜라이더 스크립트 비활성화
        }
    }

    public void removeObject()
    {
        puspAction(this.gameObject);
    }

    private void OnDisable()
    {
        GetComponent<CircleCollider2D>().enabled = true; // 콜라이더 스크립트 활성화
    }
}
