using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeControl : MonoBehaviour
{

    public List<Button> timeObjects = new List<Button>();


    public Color defaultColor, selectedColor;

    private void Start()
    {
        foreach (var button in timeObjects)
        {
            var time = button.gameObject.GetComponent<TimeControlButton>();

            if (time.TimeScale == 1)
            {
                button.image.color = selectedColor;
            }
            else
            {
                button.image.color = defaultColor;
            }
        }
    }

    public void OnButtonClick(Button button, float timeMultiplier)
    {
        Time.timeScale = timeMultiplier;

        foreach (var buttonObj in timeObjects)
        {
            buttonObj.image.color = defaultColor;
        }
        button.image.color = selectedColor;
    }
    
}
