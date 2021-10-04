using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using UnityEngine;

public class BuildingRegistry : MonoBehaviour
{
    
    [SerializeField]
    private List<BuildingData> buildingRegistry = new List<BuildingData>();


    public BuildingData Get(int index)
    {
        return buildingRegistry[index];
    }

    public int GetIndex(BuildingData data)
    {
        return buildingRegistry.IndexOf(data);
    }
}
