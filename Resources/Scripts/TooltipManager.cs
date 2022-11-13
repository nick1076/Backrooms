using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{

    public TextMeshProUGUI tooltipText;

    public void SetTooltip(string text)
    {
        tooltipText.text = text;
    }

    public void ShowTooltip()
    {
        tooltipText.gameObject.SetActive(true);
    }
    
    public void HideTooltip()
    {
        tooltipText.gameObject.SetActive(false);
    }
}
