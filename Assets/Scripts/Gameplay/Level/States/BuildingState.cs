using System.Collections;
using System.Collections.Generic;
using Gameplay.Level.States;
using UnityEngine;

public class BuildingState : MonoBehaviour
{
    public StateData data;
    public Color timerBarColor;
    
    public void OnEnter()
    {
        data.timerProgressBarImage.color = timerBarColor;
    }
}
