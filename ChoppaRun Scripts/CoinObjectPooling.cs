using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObjectPooling : MonoBehaviour {

    static CoinObjectPooling SingleTon; // 싱글톤 클래스

    public GameObject prefab;           // 오브젝트 풀링 프리팹
    public List<GameObject> objectList; // 오브젝트 리스트

    private void Start()
    {
        if(SingleTon == null) SingleTon = this;
    }

    public static CoinObjectPooling Bulid()
    {
        return SingleTon;
    }

    /**
     * 오브젝트 생성 함수
     */
    void CreateObject()
    {
        if (prefab == null) return;

        GameObject objectPool = Instantiate(prefab) as GameObject;
        objectPool.transform.SetParent(this.transform);
        objectList.Add(objectPool);
    }

    /**
     * 오브젝트를 주는 함수
     */
    public GameObject PopObject()
    {
        if (objectList.Count == 0) CreateObject();
        GameObject objectPool = objectList[0];
        objectList.RemoveAt(0);
        return objectPool;
    }

    /**
     * 오브젝트 반환받는 함수
     */
    public void PushObject(GameObject objectPool)
    {
        objectPool.transform.SetParent(this.transform);
        objectPool.SetActive(false);
        objectList.Add(objectPool);
    }

    

    

    
}
