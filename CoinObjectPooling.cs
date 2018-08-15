using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObjectPooling : MonoBehaviour {

    static CoinObjectPooling SingleTon;


    public GameObject prefab;
    [SerializeField]
    public List<GameObject> objectList;

    private void Start()
    {
        if(SingleTon == null) SingleTon = this;
        Bulid();
    }

    public static CoinObjectPooling Bulid()
    {
        return SingleTon;
    }

    void CreateObject()
    {
        if (prefab == null) return;

        GameObject objectPool = Instantiate(prefab) as GameObject;
        objectPool.transform.SetParent(this.transform);
        objectList.Add(objectPool);
    }

    public GameObject PopObject()
    {
        if (objectList.Count == 0) CreateObject();
        GameObject objectPool = objectList[0];
        objectList.RemoveAt(0);
        return objectPool;
    }

    public void PushObject(GameObject objectPool)
    {
        objectPool.transform.SetParent(this.transform);
        objectPool.SetActive(false);
        objectList.Add(objectPool);
    }

    

    

    
}
