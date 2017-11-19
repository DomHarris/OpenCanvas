using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{

    public string googleSearch;

    public void Expand()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void Retract()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }

    public void Search()
    {
        Retract();
        string url = "http://www.google.com/search?q=" + WWW.EscapeURL(googleSearch + " art");
        Debug.Log(url);
        Application.OpenURL(url);
    }
}
