using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballObjectPooling : MonoBehaviour {

    public GameObject[] fireDangerView; // 파이어볼 나오는 위치의 위험 UI

    public GameObject prefab;           // 파이어볼 Prefab
    public List<GameObject> objectList; // 파이어볼 Prefab List

    /**
     * 오브젝트 생성 함수
     */
    void CreateObject()
    {
        if (prefab == null) return;

        GameObject objectPool = Instantiate(prefab) as GameObject;
        objectPool.transform.SetParent(this.transform);

        // 오브젝트풀의 오브젝트는 ObjectPool 클래스를 가지고 있으며 해당 클래스는 Push함수를 가지고 자발적 리턴을 할 수 있도록 함.
        ObjectPool oPool = objectPool.GetComponent<ObjectPool>();
        oPool.puspAction = PushObject;
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
     * 오브젝트를 다시 반환받는 함수
     */
    public void PushObject(GameObject objectPool)
    {
        objectPool.transform.SetParent(this.transform);
        objectPool.SetActive(false);
        objectList.Add(objectPool);
    }

    /**
     * 파이어볼의 위험 UI를 처리하는 함수 ( 파이어볼의 y값)
     */ 
    public void OnDangerEvent(float y)
    {
        if(y < 1.5f)
            StartCoroutine(DangerEventCoroutine(fireDangerView[0]));
        else if (y < 2f)
            StartCoroutine(DangerEventCoroutine(fireDangerView[1]));
        else 
            StartCoroutine(DangerEventCoroutine(fireDangerView[2]));
    }

    /**
     * 위험 UI의 애니메이션 코루틴
     */
    IEnumerator DangerEventCoroutine(GameObject gameObject)
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        if (audio != null) audio.Play();
        gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        gameObject.SetActive(false);
    }
}
