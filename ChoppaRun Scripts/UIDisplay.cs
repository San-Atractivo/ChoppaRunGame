using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplay : MonoBehaviour {

    [Range(0f, 100f)]
    public float x;

    [Range(0f, 100f)]
    public float y;

    [Range(-1f,100f)]
    public float width;

    [Range(-1f, 100f)]
    public float height;

    void Awake()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();

        if (rectTrans == null) return;

        rectTrans.localPosition = new Vector3(percentToDisplaySize(x, Screen.width), percentToDisplaySize(y,Screen.height),0 );
        rectTrans.sizeDelta = DisplayUISize();
    }

    Vector2 DisplayUISize()
    {
        return new Vector2(percentToLength(width,Screen.width), percentToLength(height, Screen.height));
    }

    float percentToDisplaySize(float percent, int length)
    {
        return (length / 100) * (percent-50);
    }

    float percentToLength(float percent, int length)
    {
        return percent >= 0 ? (length / 100) * percent : width < 0 ? percentToLength(height, Screen.height) : percentToLength(width, Screen.width);
    }
}
