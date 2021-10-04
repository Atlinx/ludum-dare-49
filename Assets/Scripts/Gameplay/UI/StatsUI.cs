using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{

    public TextMeshProUGUI populationValueText;
    public TextMeshProUGUI foodValueText;
    public TextMeshProUGUI industryValueText;

    //private int populationAmount;
    //private int populationDifference;
    //private int populationTotal;

    //private int foodAmount;
    //private int foodDifference;
    //private int foodTotal;

    //private int industryAmount;
    //private int industryDifference;
    private int industryTotal;

    public void SetPopulationText(int amount, int difference, int total)
    {
        populationValueText
            .text = $"{amount.ToString()}/{total.ToString()}  {GetNumberSign(difference)}{Mathf.Abs(difference).ToString()} ";
    }

    public void SetFoodText(int amount, int difference, int total)
    {
        
        foodValueText.text = $"{amount.ToString()}/{total.ToString()}  {GetNumberSign(difference)}{Mathf.Abs(difference).ToString()} ";
    }
    
    public void SetIndustryText(int amount, int difference, int total)
    {
        industryTotal = total;
        industryValueText.text = $"{amount.ToString()}/{total.ToString()}  {GetNumberSign(difference)}{Mathf.Abs(difference).ToString()} ";
    }

    public void SetIndustryText(int amount, int difference)
    {
        industryValueText.text = $"{amount.ToString()}/{industryTotal.ToString()}  {GetNumberSign(difference)}{Mathf.Abs(difference).ToString()} ";
    }

    public string GetNumberSign(int value)
    {
        return value > 0 ? "+" : (value == 0 ? "" : "-");
    }
}
