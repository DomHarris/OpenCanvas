using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Content : MonoBehaviour
{

    public Text _name;
    public Text _author;
    public Text _content;

    public void UpdateText(string name, string author, string content)
    {
        _name.text = "Name: " + name;
        _author.text = "Author: " + author;
        _author.text = "Content: " + content;

        LeanTween.scaleY(gameObject, 1, 0.5f).setEase(LeanTweenType.easeOutQuint);
    }
}
