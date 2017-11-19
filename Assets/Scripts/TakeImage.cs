using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TakeImage : MonoBehaviour
{
    public CameraController cam;
    public RectTransform rt;
    public GameObject scan;
    public Upload up;
    public Content content;
    public Popup popup;

    public ImageFind imgid;
    public ImageInfo imginfo;

    bool shrunk = false;
    bool moving = false;

    public void CaptureImage()
    {
        LeanTween.color(scan.transform as RectTransform, Color.white, 0.5f);
        LeanTween.moveY(scan, -5, 1f).setLoopCount(20).setLoopType(LeanTweenType.pingPong);

        cam.Pause();
        byte[] captured = cam.Capture().EncodeToJPG();

        StartCoroutine(up.Capture(Upload.FeatureType.LOGO_DETECTION, captured, (Upload.AnnotateImageResponses res) =>
        {
            //foreach (var response in res.responses)
            //{
            //    foreach (var annotation in response.logoAnnotations)
            //    {
            //        Debug.LogFormat("{0}: {1}%", annotation.description, annotation.score);
            //    }
            //}

            imginfo = imgid.UpdateInfo(res);
            if (imginfo != null)
            {
                content.UpdateText(imginfo.painting_name, imginfo.author_name, imginfo.description);
                Move();
            }
            else
            {
                StartCoroutine(up.Capture(Upload.FeatureType.WEB_DETECTION, captured, (Upload.AnnotateImageResponses webRes) =>
                {
                    imginfo = imgid.UpdateInfo(webRes, true);
                    if (imginfo != null)
                        content.UpdateText(imginfo.painting_name, imginfo.author_name, imginfo.description);
                    else
                    {
                        if (res.responses != null && res.responses.Count > 0 && res.responses[0].logoAnnotations != null && res.responses[0].logoAnnotations.Count > 0)
                            popup.googleSearch = res.responses[0].logoAnnotations[0].description;
                        else
                            popup.googleSearch = webRes.responses[0].webDetection.webEntities[0].description;

                        popup.Expand();
                    }
                    Move();
                }));
            }
        }));
    }

    void Move()
    {
        LeanTween.color(scan.transform as RectTransform, new Color(1, 1, 1, 0), 0.5f);
        LeanTween.moveY(scan, 5, 0.5f).setOnComplete(LeanTween.cancelAll);
        //if (!moving)
        //{
        //    moving = true;
        //    LeanTween.value(gameObject, rt.sizeDelta.y, shrunk ? 0 : -1300, 1f).setOnUpdate(UpdateCamera).setEase(LeanTweenType.easeOutQuint).setOnComplete(() =>
        //    {
        //        moving = false;
        //        shrunk = !shrunk;
        //    });
        //}
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

    }
}
