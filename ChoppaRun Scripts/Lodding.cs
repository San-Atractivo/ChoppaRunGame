using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lodding : MonoBehaviour {
    bool coroutineFlag;
    public Text LoddingText;

    public delegate void LoddingEvent();
    LoddingEvent OnEvent;

    public void SetCoroutineFlag(bool flag) { coroutineFlag = flag; }

    public void OnAddEvent(LoddingEvent _event)
    {
        OnEvent = _event;
    }

    public void LoddingView(bool active)
    {
        this.gameObject.SetActive(active);
        if (active) StartCoroutine(LoddingCoroutine());
    }

    IEnumerator LoddingCoroutine()
    {
        for (; !coroutineFlag;)
        {
            for (int count = 0; count < 4; count++)
            {

                yield return new WaitForSeconds(0.25f);

                if (count % 4 == 0) LoddingText.text = "Lodding";
                else LoddingText.text += ".";

            }
        }

        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
        OnEvent();
    }


}
