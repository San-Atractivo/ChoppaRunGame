using UnityEngine;
using System.Collections;

public class Flower : MonoBehaviour {

    private GameObject _flower; // 활성화된 꽃 오브젝트 저장변수
    public GameObject[] flower; // 꽃오브젝트 Prefab


    /**
     * 해당 스크립트의 오브젝트 위치에 꽃오브젝트 Prefab을 생성
     */
    void Start() {
		float left = PlayerPrefs.GetFloat("SideLeft"); 
		float right = PlayerPrefs.GetFloat("SideRight");

		float center = (right - left);	// 폭 가운데
		float height = right + left;	// 폭의 넓이

		float a = (center + height - 1) - (center - height + 1); 

        float x = transform.position.x;
        float z = transform.position.z;

        GameObject obj;

        for (int i =0; i< flower.Length; i++)
        {
            obj = Instantiate(flower[i]) as GameObject;
            flower[i] = obj;
            obj.transform.position = new Vector3(x + Random.Range(-0.5f, 0.5f), -1, z + Random.Range(-0.5f, 0.5f));
            obj.transform.parent = this.transform;
			switch (i) {
			case 0:
				obj.transform.localScale = new Vector3 (a / 22, a / 22, a / 22);
				break;
			case 1:
				obj.transform.localScale = new Vector3 ( (a / 22) *3, (a / 22) *3, (a / 22) *3);
				break;
			default:
				obj.transform.localScale = new Vector3 ( (a / 22) *12, (a / 22) *12, (a / 22) *12);
				break;
			}
            obj.SetActive(false);
        }

		if (Camera.main.transform.position.y > 0)
			transform.localEulerAngles = new Vector3 (0, 0, 180);
		else
			transform.localEulerAngles = new Vector3 (0, 0, 0);
    }

    /**
     * 키넥트에 인식된 오브젝트와 부딪힐경우
     * 해당 지점에 꽃을 피운다.
     */
	void OnTriggerEnter(Collider col){

        _flower = flower [Random.Range (0, 5)];
	    _flower.SetActive (true);
		StartCoroutine(DeleteFlower(_flower));
	}

    /**
     * 활성화된 꽃이 지는 타이머 코루틴
     */
	IEnumerator DeleteFlower(GameObject obj){
		
	    yield return new WaitForSeconds (3);
		obj.SetActive (false);
	}
}
