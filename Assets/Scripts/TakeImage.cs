using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeImage : MonoBehaviour
{
    public CameraController cam;
    public RectTransform rt;
    public GameObject scan;

    bool shrunk = false;
    bool moving = false;

    public void CaptureImage()
    {
        LeanTween.color(scan.transform as RectTransform, Color.white, 0.5f);
        LeanTween.moveLocalY(scan, -800, 1f).setLoopCount(4).setLoopType(LeanTweenType.pingPong);
        StartCoroutine(SendRequest(""));
    }

    void UpdateCamera(float val)
    {
        Vector2 size = rt.sizeDelta;
        size.y = val;
        rt.sizeDelta = size;
    }

    IEnumerator SendRequest(string url)
    {
        WWWForm form = new WWWForm();
        //form.AddBinaryData("image", data);
        WWW www = new WWW(url, form);
        yield return www;
        LeanTween.color(scan.transform as RectTransform, new Color(1, 1, 1, 0), 0.5f);
        Debug.Log(www.text);

        Debug.Log(rt.sizeDelta.y);
        if (!moving)
        {
            moving = true;
            LeanTween.value(gameObject, rt.sizeDelta.y, shrunk ? 0 : -500, 1f).setOnUpdate(UpdateCamera).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
            {
                moving = false;
                if (shrunk)
                    cam.Play();
                else
                    cam.Pause();
                shrunk = !shrunk;
            });
        }
    }
}
