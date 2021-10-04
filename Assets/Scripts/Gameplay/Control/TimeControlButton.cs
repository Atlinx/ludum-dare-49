using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using UnityEngine;
using UnityEngine.UI;

public class TimeControlButton : MonoBehaviour
{
    private Button button;
    public float TimeScale;
    public TimeControl timeController;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnClicked()
    {
        timeController.OnButtonClick(button, TimeScale);
    }
}
