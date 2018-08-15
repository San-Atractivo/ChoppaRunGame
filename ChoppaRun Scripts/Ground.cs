using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

    public float mapLength;
    private float speed = 5f;

    void Start()
    {
        StartCoroutine(VisibleViewPointCheck());
    }

    // Update is called once per frame
    void Update () {
        if (transform.position.x < -(mapLength+5))
            GroundDestroyOperation();
        transform.position = new Vector3(transform.position.x-(speed*Time.deltaTime*GameManager.SingleTon.GetGameSpeed()), transform.position.y, transform.position.z);
	}

    IEnumerator VisibleViewPointCheck()
    {
        do
        {
            yield return new WaitForSeconds(0.05f);
            Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
            if (view.x <= 0) break;
        } while (true);

        GameManager.SingleTon.InstaniateGround(mapLength, transform.position.x);
    }

    Transform GetChildObject(string name)
    {
        Transform[] childs = this.GetComponentsInChildren<Transform>();
        foreach(Transform child in childs)
        {
            if (child.name == name) return child;
        }
        return null;
    }

    void GroundDestroyOperation()
    {
        Transform coinGroup = GetChildObject("item");
        if (coinGroup != null)
        {
            int groupCount = coinGroup.childCount;
            for (int i = 0; i < groupCount; i++)
            {
                CoinGeneratorBox cgb = coinGroup.GetChild(i).GetComponent<CoinGeneratorBox>();
                cgb.returnToCoin();
            }
        }
        Destroy(this.gameObject);
    }
    
}
