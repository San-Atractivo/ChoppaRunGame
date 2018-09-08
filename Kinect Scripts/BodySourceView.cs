using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class BodySourceView : MonoBehaviour
{

    public GameObject BodySourceManager;    // BodySourceManager 클래스를 가진 오브젝트

	private Dictionary<GameObject, bool> _OverLab = new Dictionary<GameObject, bool>();     // 뼈 사용여부 저장 변수
    private Dictionary<ulong, GameObject> _Bodies = new Dictionary<ulong, GameObject>();    // 뼈 오브젝트 저장 변수
    private BodySourceManager _BodyManager; // 키넥트센서 연동 및 뼈 인식 클래스

    private bool esc = false;   // 설정창 UI 

	void Update()
    {
        //  Cancel 버튼 클릭시 UI창 Active
        if (Input.GetButtonDown("Cancel"))
        {
            if (!esc)
                esc = true;
            else
                esc = false;
        }

        //  오브젝트가 없을 경우
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null) // 뼈 인식 스크립트가 없을경우
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)   // 뼈 데이터를 가져올 수 없을 경우
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)  // 뼈마디 데이터 서치
        {
            if (body == null)       // 뼈가 없을 경우
            {
                continue;
            }

            if (body.IsTracked)     // 뼈를 리스트에 추가
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // 인식되지 않은 뼈마디를 리스트에서 삭제
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                Destroy(_Bodies[trackingId]);
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)  // 뼈마디 데이터 서치
        {   
            if (body == null)       // 뼈마디 데이터가 존재하지 않을 경우
            {
                continue;
            }

            if (body.IsTracked)     
            {
                if (!_Bodies.ContainsKey(body.TrackingId))  // 리스트에 추가된 뼈가 있는지 확인
                {
                    _Bodies[body.TrackingId] = CreateBodyObject(body.TrackingId);   // 생성된 뼈 오브젝트를 리스트에 추가
                }
				RefreshBodyObject(body, _Bodies[body.TrackingId]);
            }
        }
    }

    /**
     * 뼈 오브젝트를 생성 및 기본 세팅
     * @param 뼈마디 id값
     */
    private GameObject CreateBodyObject(ulong id)
    {
        GameObject body = new GameObject("Body:" + id); // 부모 오브젝트 세팅

        // 테스트 용 : 눈에 보일 수 있게 캡슐오브젝트 생성
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        _OverLab [body] = true;
        cube.transform.name = "SpineBase";
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.AddComponent<CapsuleCollider>();
        cube.GetComponent<CapsuleCollider>().height = 5;
		cube.GetComponent<CapsuleCollider> ().radius = SetRadius ();
		cube.AddComponent<Rigidbody> ().useGravity = false;
        cube.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        cube.transform.parent = body.transform;
        return body;
    }

    /**
     * 뼈의 위치를 가져와 설정하는 함수
     * @param 키넥트 바디, 뼈 부모 오브젝트
     */
	private void RefreshBodyObject(Kinect.Body body, GameObject bodyObject)
    {
        // 테스트용 : 필요한 부분인 SpineBase 뼈 데이터만 가져온다.
        Kinect.Joint sourceJoint = body.Joints[Kinect.JointType.SpineBase];

        // 사용되는 SpineBase를 가져온다
        Transform jointObj = bodyObject.transform.FindChild("SpineBase");

        Vector3 v = GetVector3FromJoint(sourceJoint);

		if (_OverLab [bodyObject]) 
		{
            // 뼈의 위치를 확인. 인식 범위빡으로 나갔을 경우 사용여부 설정
			float over = float.Parse(PlayerPrefs.GetString("MaxRange"))-float.Parse(PlayerPrefs.GetString("MinRange"));

			if (v.z > over - (over / 10))
				return;
			_OverLab [bodyObject] = false;
		}
		
        if( Mathf.Abs(v.z - jointObj.position.z) > 0.5 || Mathf.Abs(v.x - jointObj.position.x) > 0.5 )
            jointObj.localPosition = GetVector3FromJoint(sourceJoint);
    }

    /**
     * 유니티 위치와 뼈데이터의 위치를 조정하는 함수
     * @param 뼈마디 데이터
     */
    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, 0, joint.Position.Z * 10);
    }


	private float SetRadius(){
		float left = PlayerPrefs.GetFloat("SideLeft"); 
		float right = PlayerPrefs.GetFloat("SideRight");

		float center = (right - left);	// 폭 가운데
		float height = right + left;	// 폭의 넓이

		float a = (center + height - 1) - (center - height + 1); 

		return (a / 22) * 2;
	}

    // 설정창 GUI
    void OnGUI()
    { 

        if (esc)
        {
            GUI.Box(new Rect(Screen.width *0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.5f), "\n\n뒤로 \n\n나가시겠습니까?");
            if (GUI.Button(new Rect(Screen.width *0.25f + 10, Screen.height * 0.5f, Screen.width * 0.25f-10, Screen.height *0.25f-10), "네"))
                Application.LoadLevel(0);
            else if (GUI.Button(new Rect(Screen.width * 0.5f + 10, Screen.height * 0.5f, Screen.width * 0.25f - 20, Screen.height * 0.25f-10), "아니요"))
                esc = false;

        }
    }
}