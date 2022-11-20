using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonVisuals : MonoBehaviour
{
    public TextMeshPro text;
    public Color highlightColor;
    public Color baseColor;

    public void OnHover()
    {
        text.color = highlightColor;
    }
    public void OnUnHover()
    {
        text.color = baseColor;
    }
}
