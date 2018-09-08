using UnityEngine;
using System.Collections;

public class ViewManager : MonoBehaviour { // 씬이 넘어올 경우 바닥과 카메라를 설정하고, 꽃관리오브젝트를 생성한다. 이후 모든 오브젝트가 생성되면 사라짐. 

    GameObject ground;
    // Use this for initialization
    void Start () {

        if (!PlayerPrefs.HasKey ("MaxRange")) {
			PlayerPrefs.SetString ("MaxRange", "45");
		}if (!PlayerPrefs.HasKey ("MinRange")) {
			PlayerPrefs.SetString ("MinRange", "5");
		}if (!PlayerPrefs.HasKey ("SideLeft")) {
			PlayerPrefs.SetFloat ("SideLeft", 2);
		}if (!PlayerPrefs.HasKey ("SideRight")) {
			PlayerPrefs.SetFloat ("SideRight", 2);
		}

		float max = float.Parse(PlayerPrefs.GetString("MaxRange"));
        float min = float.Parse(PlayerPrefs.GetString("MinRange"));
		float left = PlayerPrefs.GetFloat("SideLeft"); 
		float right = PlayerPrefs.GetFloat("SideRight");

		float range = max - min;

        float center = (right - left);	// 폭 가운데
        float height = right + left;	// 폭의 넓이

		int upsidedown = PlayerPrefs.GetInt ("UpSideDown");
		int mirror = PlayerPrefs.GetInt ("Mirror");

        float y = (max - min) / 1.5f;	// 높이
        float z = (max + min) / 2;	// 길이


        // 카메라 설정
		Camera camera = Camera.main;

		camera.transform.position = new Vector3 (0, (y * mirror * upsidedown), z);
		camera.transform.localEulerAngles = new Vector3 (90 * mirror * upsidedown, 0, -90 * upsidedown);


        // 잔디 생성 및 설정
		ground = Instantiate(Resources.Load("Prefeb/ground0")) as GameObject;
		ground.transform.position = new Vector3((left - right)/3 * (range*0.0077f)*(left + right)/2, 0, z);
		ground.transform.localScale = new Vector3((range*0.0077f)*(left + right)/2, 1, (max-min)*0.1f + 0.1f);

		if(camera.transform.position.y > 0)
			ground.transform.localEulerAngles = new Vector3 (0, 0, 0);
		else
			ground.transform.localEulerAngles = new Vector3 (0, 0, 180);


		// 꽃 관리 오브젝트
		for (float i = min; i < max; i += (max-min)/40) {
			for (float j = center - height + 1 ; j < center + height - 1  ; j += ((center + height - 1) - (center - height + 1))/22 ) {
				GameObject obj = Instantiate (Resources.Load ("Prefeb/Flower")) as GameObject;
				obj.transform.position = new Vector3 (j, 0, i);
			}
		}

		//위 아래 벽 
		GameObject down = GameObject.CreatePrimitive(PrimitiveType.Plane);
		down.name = "down";
		down.transform.position = new Vector3((range*0.75f)/2, 0, z);
		down.transform.localScale = new Vector3((range*0.0077f)*(20 - (left + right))/2, 1, (((y * 1.5f) / 9) * 16) / 10);
		down.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
		down.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1f);

		GameObject up = GameObject.CreatePrimitive(PrimitiveType.Plane);
		up.name = "up";
		up.transform.position = new Vector3(-((range*0.75f)/2), 0, z);
		up.transform.localScale = new Vector3((range*0.0077f)*(20 - (left + right))/2, 1, (((y * 1.5f) / 9) * 16) / 10);
		up.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
		up.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1f);
	}

}
