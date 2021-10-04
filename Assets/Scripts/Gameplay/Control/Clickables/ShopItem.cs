using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Buildings;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    public BuildingData buildingData;

    public int Cost;
    
    public GameValues gameValues;

    public Image NotEnoughCurrencyOverlay;

    public GameObject gridCursor;
    
    public bool RemoveAfterPlace;

    public TextMeshProUGUI CostText;
    
    private RectTransform rectTransform;
    
    
    private GameControl control;
    private GameObject prefabClone;

    private Vector3 clickOffset;

    private Vector2Int gridPos;
    private bool CanPlace;

    private bool dragging = false;

    private Vector3 lastPosition;

    public UnityEvent PlacedValidEvent;
    public UnityEvent PlacedInvalidEvent;
    public UnityEvent StartDragEvent;

    private bool placedValid = false;
    
    private void Update()
    {
        if (rectTransform != null &&lastPosition != rectTransform.position)
        {
            lastPosition = rectTransform.position;
            if(!dragging) ResetPrefabClonePosition();
        }
    }

    public void Init()
    {
        this.rectTransform = GetComponent<RectTransform>();
        lastPosition = rectTransform.position;
        if(buildingData != null) SetItem(buildingData);
        CostText.text = Cost.ToString();
    }
    

    public void SetItem(BuildingData data)
    {
        this.buildingData = data;
        prefabClone = Instantiate(data.building);
        prefabClone.GetComponent<SpriteRenderer>().sortingOrder = 3;
        ResetPrefabClonePosition();
    }
    
    
    
    public void OnClicked(GameControl control)
    {
        dragging = true;
        this.control = control;
        
        if (this.buildingData == null || !CanBuy())
        {
            control.StopCurrentClickAction();
            return;
        }
    
        
        clickOffset =  prefabClone.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickOffset.z = 0;
        StartDragEvent.Invoke();
        UpdateMousePosition();
    }

    public void ClickUpdate()
    {
        UpdateMousePosition();

        if (Input.GetMouseButtonUp(0) || !Input.GetMouseButton(0))
        {
            if (CanPlace)
            {
                control.buildingManager.AddBuilding(this.buildingData, gridPos.x, gridPos.y);
                
                SubtractIndustry();
                control.statsUI.SetIndustryText(gameValues.IndustryTotal,-Cost);
                
                if (RemoveAfterPlace)
                {
                    buildingData = null;
                    Destroy(prefabClone);
                }

                placedValid = true;
            }
            
            control.StopCurrentClickAction();
        }
    }

    public void OnExit()
    {
        dragging = false;
        if (buildingData != null)
        {
            ResetPrefabClonePosition();
        }

        if (placedValid)
        {
            PlacedValidEvent.Invoke();
        }
        else
        {
            PlacedInvalidEvent.Invoke();
        }
    }

    

    public void OnPointerEnter(PointerEventData eventData)
    {
        NotEnoughCurrencyOverlay.enabled = !CanBuy();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        NotEnoughCurrencyOverlay.enabled = false;
    }

    public bool CanBuy()
    {
        return gameValues.IndustryTotal >= Cost;
    }

    public void SubtractIndustry()
    {
        
        gameValues.IndustryTotal -= Cost;
    }

    public void ResetPrefabClonePosition()
    {
        prefabClone.transform.position = rectTransform.position;
    }

    public void UpdateMousePosition()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        mousePosition += clickOffset;



        CanPlace = false;
        gridPos = BuildingUtility.GetGridFromWorld(mousePosition);
        
        if (control.buildingManager.WithinGrid(gridPos) && !control.buildingManager.HasBuilding(gridPos))
        {
            prefabClone.transform.position = BuildingUtility.GridToWorld(gridPos);
            CanPlace = true;
        }
        else
        {
            prefabClone.transform.position = mousePosition;

        }

    }
}
