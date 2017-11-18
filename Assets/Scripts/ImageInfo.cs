using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageInfo : MonoBehaviour
{
    public string author_name;
    public string painting_name;
    public string description;
    public string interpretation;
    public string style;
    public int year;
    public Dictionary<string, string> symbolism;

    public ImageInfo()
    {
        author_name = "";
        painting_name = "";
        description = "";
        interpretation = "";
        style = "";
    }
}
