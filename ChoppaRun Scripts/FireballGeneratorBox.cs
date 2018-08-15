using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballGeneratorBox : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneratorBox"))
        {
            FireballObjectPooling ojp = collision.GetComponent<FireballObjectPooling>();
            GameObject fireball = ojp.PopObject();
            fireball.transform.position = new Vector3(transform.position.x + 4f, transform.position.y, transform.position.z);
            fireball.SetActive(true);
            ojp.OnDangerEvent(transform.position.y);
        }
    }
}
