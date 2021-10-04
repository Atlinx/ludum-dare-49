using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeControl : MonoBehaviour
{
    public List<Button> timeObjects = new List<Button>();


    public Color defaultColor, selectedColor;

    public int SelectedType = 0;
    
    private void Start()
    {
        foreach (var button in timeObjects)
        {
            var typeControlButton = button.gameObject.GetComponent<TypeControlButton>();

            if (typeControlButton.type == SelectedType)
            {
                button.image.color = selectedColor;
            }
            else
            {
                button.image.color = defaultColor;
            }
        }
    }

    public void OnButtonClick(Button button, int type)
    {

        SelectedType = type;
        
        foreach (var buttonObj in timeObjects)
        {
            buttonObj.image.color = defaultColor;
        }
        button.image.color = selectedColor;
    }

}
