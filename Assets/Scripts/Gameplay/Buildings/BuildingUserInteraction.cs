using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for getting user input and placing or removing a building
/// </summary>
public class BuildingUserInteraction : MonoBehaviour
{

    public enum Tool
    {
        Create, Remove, Info
    }


    public Tool CurrentTool;
    
    public int selectedBuilding = 0;

    public BuildingManager buildingManager;

    public GameObject gridInteractionCursor;
    public SpriteRenderer gridInteractionCursorSR;

    public Camera camera;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            selectedBuilding = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBuilding = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBuilding = 2;
        }
        
        var mousePosition = Input.mousePosition;
        mousePosition = camera.ScreenToWorldPoint(mousePosition);


        var gridPos = BuildingUtility.GetGridFromWorld(mousePosition);
        gridInteractionCursor.transform.position = new Vector3(gridPos.x, gridPos.y, 0);

        switch (CurrentTool)
        {
            case Tool.Create:
                
                bool canPlace = buildingManager.WithinGrid(gridPos.x, gridPos.y) &&
                                !buildingManager.HasBuilding(gridPos.x, gridPos.y);
        
                gridInteractionCursorSR.color= (canPlace ? Color.green : Color.red);
                
                
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    buildingManager.AddBuilding(selectedBuilding, gridPos.x, gridPos.y);
                }
                
                
                break;
            case Tool.Remove:
                
                bool canRemove = buildingManager.WithinGrid(gridPos.x, gridPos.y) &&
                                buildingManager.HasBuilding(gridPos.x, gridPos.y);
        
                gridInteractionCursorSR.color = (canRemove ? Color.blue : Color.red);
                
                
                if (Input.GetMouseButtonDown(0) && canRemove)
                {
                    buildingManager.RemoveBuilding(gridPos.x, gridPos.y);
                }
                
                
                break;
            case Tool.Info:
                
                
                
                
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }
}
