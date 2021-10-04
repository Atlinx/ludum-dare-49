using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    public List<GameObject> currentTexts = new List<GameObject>();
    
    public GameObject textPrefab;
    
    public void SetObjectivesText(string[] objectives)
    {
        foreach (var gameobj in currentTexts)
        {
            Destroy(gameobj);
        }
        
        foreach(var strObj in objectives)
        {
            var newText = Instantiate(textPrefab);
            newText.GetComponentInChildren<TextMeshProUGUI>().text = strObj;
            currentTexts.Add(newText);
        }

    }
}
