using UnityEngine;
using System.Collections;

public class BackGround : MonoBehaviour {
    public const float MAXSCROOLSPEED = 1.0f;
    public const float NOMALSCROLLSPEED = 0.3f;
    private float scrollSpeed = 0.3f;
    float Offset;

    private Material backgroundMaterial;

    void Start()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (scrollSpeed != 0) {
            Offset += Time.deltaTime * scrollSpeed;
            backgroundMaterial.mainTextureOffset = new Vector2(Offset, 0);
        }
    }

    public void setSpeed(float speed)
    {
        scrollSpeed = speed;
    }
}
