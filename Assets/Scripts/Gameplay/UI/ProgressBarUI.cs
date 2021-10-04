using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarUI : MonoBehaviour
{

    public RectTransform ProgressBarTopPanel;
    public bool IsVertical = false;

    public float currentValue, maxValue = 10;
        
    public void UpdateBar() {


        Vector2 anchor = ProgressBarTopPanel.anchorMax;
        if (IsVertical) {
            anchor.y = Mathf.Clamp((float)currentValue/ maxValue, 0, 1);
        } else {
            anchor.x = Mathf.Clamp((float)currentValue/ maxValue, 0, 1);
        }
        ProgressBarTopPanel.anchorMax = anchor;

        ProgressBarTopPanel.sizeDelta = Vector2.zero;
    }
}

