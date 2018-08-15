using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ObjectPool {

    Animator ani;
    AudioSource audio;
    private float speed = 6.5f;

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
        if (col.tag.Equals("Player"))
        {
            bool death;
            
            audio.Play();
            ani.SetTrigger("Explosion");
            death = col.gameObject.transform.parent.GetComponent<Player>().PlayerDead();
            if (death) audio.Play();
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public void removeObject()
    {
        puspAction(this.gameObject);
    }

    private void OnDisable()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
