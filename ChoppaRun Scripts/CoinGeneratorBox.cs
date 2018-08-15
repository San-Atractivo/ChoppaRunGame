using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGeneratorBox : MonoBehaviour {

    GameObject coin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 생성 박스를 만날 경우 코인 오브젝트를 가져와 보여줌
        if (collision.tag.Equals("GeneratorBox"))
        {
            coin = CoinObjectPooling.Bulid().PopObject();
            coin.SetActive(true);
            coin.transform.SetParent(this.transform);
            coin.transform.position = this.transform.position;
        }
    }

    // 코인 오브젝트를 다시 돌려줌
    public void returnToCoin()
    {
        if (coin == null) return;
        CoinObjectPooling.Bulid().PushObject(coin);
    }
}
