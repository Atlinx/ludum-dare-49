using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using UnityEngine;

public interface IClickable
{

    void OnClicked(GameControl control);
    void ClickUpdate();
    void OnExit();

    

}
