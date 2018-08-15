using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGeneratorBox : MonoBehaviour {

    GameObject coin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("GeneratorBox"))
        {
            coin = CoinObjectPooling.Bulid().PopObject();
            coin.SetActive(true);
            coin.transform.SetParent(this.transform);
            coin.transform.position = this.transform.position;
        }
    }

    public void returnToCoin()
    {
        if (coin == null) return;
        CoinObjectPooling.Bulid().PushObject(coin);
    }
}
