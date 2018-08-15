using UnityEngine;
using UnityEngine.UI;

public class SkillEvent : MonoBehaviour {

    public Sprite onImage;
    public Sprite offImage;

    Image image;
    bool onoff; // true = on, false = off

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public bool GetOnoff()
    {
        return onoff;
    }

    public void OnSkillImage()
    {
        onoff = true;
        SetImage(onImage);
    }

    public void OffSkillImage()
    {
        onoff = false;
        SetImage(offImage);
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }


}
