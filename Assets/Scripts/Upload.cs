﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Upload : MonoBehaviour
{

    public string url = "https://vision.googleapis.com/v1/images:annotate?key=";
    public string apiKey = "AIzaSyBoLcUoReUdZpUkhnnWVlnKjWQhnqmFAd8";
    public float captureIntervalSeconds = 5.0f;
    public int requestedWidth = 640;
    public int requestedHeight = 480;
    public FeatureType featureType = FeatureType.LOGO_DETECTION;
    public int maxResults = 10;

    WebCamTexture webcamTexture;
    Texture2D texture2D;
    Dictionary<string, string> headers;

    [System.Serializable]
    public class AnnotateImageRequests
    {
        public List<AnnotateImageRequest> requests;
    }

    [System.Serializable]
    public class AnnotateImageRequest
    {
        public Image image;
        public List<Feature> features;
    }

    [System.Serializable]
    public class Image
    {
        public string content;
    }

    [System.Serializable]
    public class Feature
    {
        public string type;
        public int maxResults;
    }

    [System.Serializable]
    public class ImageContext
    {
        public LatLongRect latLongRect;
        public List<string> languageHints;
    }

    [System.Serializable]
    public class LatLongRect
    {
        public LatLng minLatLng;
        public LatLng maxLatLng;
    }

    [System.Serializable]
    public class AnnotateImageResponses
    {
        public List<AnnotateImageResponse> responses;
    }

    [System.Serializable]
    public class AnnotateImageResponse
    {
        public List<FaceAnnotation> faceAnnotations;
        public List<EntityAnnotation> landmarkAnnotations;
        public List<EntityAnnotation> logoAnnotations;
        public List<EntityAnnotation> labelAnnotations;
        public List<EntityAnnotation> textAnnotations;
        public WebDetection webDetection;
    }

    [System.Serializable]
    public class FaceAnnotation
    {
        public BoundingPoly boundingPoly;
        public BoundingPoly fdBoundingPoly;
        public List<Landmark> landmarks;
        public float rollAngle;
        public float panAngle;
        public float tiltAngle;
        public float detectionConfidence;
        public float landmarkingConfidence;
        public string joyLikelihood;
        public string sorrowLikelihood;
        public string angerLikelihood;
        public string surpriseLikelihood;
        public string underExposedLikelihood;
        public string blurredLikelihood;
        public string headwearLikelihood;
    }

    [System.Serializable]
    public class EntityAnnotation
    {
        public string mid;
        public string locale;
        public string description;
        public float score;
        public float confidence;
        public float topicality;
        public BoundingPoly boundingPoly;
        public List<LocationInfo> locations;
        public List<Property> properties;
    }

    [System.Serializable]
    public class WebDetection
    {
        public List<WebEntity> webEntities;
    }

    [System.Serializable]
    public class WebEntity
    {
        public string entityId;
        public string score;
        public string description;
    }

    [System.Serializable]
    public class BoundingPoly
    {
        public List<Vertex> vertices;
    }

    [System.Serializable]
    public class Landmark
    {
        public string type;
        public Position position;
    }

    [System.Serializable]
    public class Position
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class Vertex
    {
        public float x;
        public float y;
    }

    [System.Serializable]
    public class LocationInfo
    {
        LatLng latLng;
    }

    [System.Serializable]
    public class LatLng
    {
        float latitude;
        float longitude;
    }

    [System.Serializable]
    public class Property
    {
        string name;
        string value;
    }

    public enum FeatureType
    {
        TYPE_UNSPECIFIED,
        FACE_DETECTION,
        LANDMARK_DETECTION,
        LOGO_DETECTION,
        LABEL_DETECTION,
        TEXT_DETECTION,
        SAFE_SEARCH_DETECTION,
        IMAGE_PROPERTIES,
        WEB_DETECTION,
    }

    public enum LandmarkType
    {
        UNKNOWN_LANDMARK,
        LEFT_EYE,
        RIGHT_EYE,
        LEFT_OF_LEFT_EYEBROW,
        RIGHT_OF_LEFT_EYEBROW,
        LEFT_OF_RIGHT_EYEBROW,
        RIGHT_OF_RIGHT_EYEBROW,
        MIDPOINT_BETWEEN_EYES,
        NOSE_TIP,
        UPPER_LIP,
        LOWER_LIP,
        MOUTH_LEFT,
        MOUTH_RIGHT,
        MOUTH_CENTER,
        NOSE_BOTTOM_RIGHT,
        NOSE_BOTTOM_LEFT,
        NOSE_BOTTOM_CENTER,
        LEFT_EYE_TOP_BOUNDARY,
        LEFT_EYE_RIGHT_CORNER,
        LEFT_EYE_BOTTOM_BOUNDARY,
        LEFT_EYE_LEFT_CORNER,
        RIGHT_EYE_TOP_BOUNDARY,
        RIGHT_EYE_RIGHT_CORNER,
        RIGHT_EYE_BOTTOM_BOUNDARY,
        RIGHT_EYE_LEFT_CORNER,
        LEFT_EYEBROW_UPPER_MIDPOINT,
        RIGHT_EYEBROW_UPPER_MIDPOINT,
        LEFT_EAR_TRAGION,
        RIGHT_EAR_TRAGION,
        LEFT_EYE_PUPIL,
        RIGHT_EYE_PUPIL,
        FOREHEAD_GLABELLA,
        CHIN_GNATHION,
        CHIN_LEFT_GONION,
        CHIN_RIGHT_GONION
    };

    public enum Likelihood
    {
        UNKNOWN,
        VERY_UNLIKELY,
        UNLIKELY,
        POSSIBLE,
        LIKELY,
        VERY_LIKELY
    }

    // Use this for initialization
    void Start()
    {
        headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json; charset=UTF-8");

        if (apiKey == null || apiKey == "")
            Debug.LogError("No API key. Please set your API key into the \"Web Cam Texture To Cloud Vision(Script)\" component.");
    }

    public IEnumerator Capture(FeatureType type, byte[] data, System.Action<AnnotateImageResponses> onComplete)
    {
        string base64 = System.Convert.ToBase64String(data);

        AnnotateImageRequests requests = new AnnotateImageRequests();
        requests.requests = new List<AnnotateImageRequest>();

        AnnotateImageRequest request = new AnnotateImageRequest();
        request.image = new Image();
        request.image.content = base64;
        request.features = new List<Feature>();

        Feature feature = new Feature();
        feature.type = type.ToString();
        feature.maxResults = this.maxResults;

        request.features.Add(feature);

        requests.requests.Add(request);

        string jsonData = JsonUtility.ToJson(requests, false);
        if (jsonData != string.Empty)
        {
            string url = this.url + this.apiKey;
            byte[] postData = System.Text.Encoding.Default.GetBytes(jsonData);
            using (WWW www = new WWW(url, postData, headers))
            {
                yield return www;
                if (string.IsNullOrEmpty(www.error))
                {
                    Debug.Log(www.text.Replace("\n", "").Replace(" ", ""));
                    AnnotateImageResponses responses = JsonUtility.FromJson<AnnotateImageResponses>(www.text);
                    if (onComplete != null)
                        onComplete(responses);
                    // SendMessage, BroadcastMessage or someting like that.

                }
                else
                {
                    Debug.Log("Error: " + www.error);
                }
            }
        }
    }
}