using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public RawImage image;
    public RectTransform imageParent;
    public AspectRatioFitter imageFitter;

    // Device cameras
    WebCamDevice device;

    WebCamTexture wtex;

    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Image uvRect
    Rect defaultRect = new Rect(0f, 0f, 1f, 1f);
    Rect fixedRect = new Rect(0f, 1f, 1f, -1f);

    // Image Parent's scale
    Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    Vector3 fixedScale = new Vector3(-1f, 1f, 1f);

    bool initialised = false;

    // Use this for initialization
    void Start()
    {
        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
            return;
        }

        // Get the device's cameras and create WebCamTextures with them
        device = WebCamTexture.devices.First();

        wtex = new WebCamTexture(device.name, 10000, 10000);

        // Set camera filter modes for a smoother looking image
        wtex.filterMode = FilterMode.Trilinear;

        image.texture = wtex;
        image.material.mainTexture = wtex;

        wtex.Play();
    }

    void Update()
    {
        if (!initialised)
        {
            // Skip making adjustment for incorrect camera data
            if (wtex.width < 100)
            {
                //Debug.Log ( "Still waiting another frame for correct info..." );
                return;
            }
            // Rotate image to show correct orientation 
            rotationVector.z = -wtex.videoRotationAngle;
            image.rectTransform.localEulerAngles = rotationVector;

            // Set AspectRatioFitter's ratio
            float videoRatio =
                (float)wtex.width / (float)wtex.height;
            imageFitter.aspectRatio = videoRatio;

            // Unflip if vertically flipped
            image.uvRect =
                     wtex.videoVerticallyMirrored ? fixedRect : defaultRect;

            // Mirror front-facing camera's image horizontally to look more natural
            imageParent.localScale =
                           device.isFrontFacing ? fixedScale : defaultScale;

            initialised = true;
        }
    }

    public void Pause()
    {
        wtex.Pause();
    }

    public void Play()
    {
        wtex.Play();
    }

    public void Stop()
    {
        wtex.Stop();
    }

    public Texture2D Capture()
    {
        Texture2D temp = new Texture2D(wtex.width, wtex.height);
        temp.SetPixels32(wtex.GetPixels32());
        return temp;
    }
}
