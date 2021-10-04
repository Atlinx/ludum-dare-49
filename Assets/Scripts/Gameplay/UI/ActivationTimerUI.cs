using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationTimerUI : MonoBehaviour
{
    
    
    public void Update()
    {
        float time = 0.5f;
        
        
        var intTime  = time;
        var seconds = intTime % 60;
        var fraction= 0.5f * 1000;
        fraction = fraction % 1000;
        var timeText = String.Format ("{0:00}:{1:00}", seconds ,fraction);
        
        
        
    }
}
