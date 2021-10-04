using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public AudioSource buttonClickSfx;

    public void Click()
    {
        buttonClickSfx.Play();
    }
}
