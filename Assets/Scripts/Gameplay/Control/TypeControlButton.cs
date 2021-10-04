using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeControlButton : MonoBehaviour
{
    private Button button;
    public int type;
    public TypeControl typeController;
    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnClicked()
    {
        typeController.OnButtonClick(button, type);
    }
}
