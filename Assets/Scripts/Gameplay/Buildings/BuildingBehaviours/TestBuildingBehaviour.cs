using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingBehaviour))]
public class TestBuildingBehaviour : MonoBehaviour
{
    private BuildingBehaviour buildingBehaviour;

    // Start is called before the first frame update
    private void Start()
    {
        buildingBehaviour = GetComponent<BuildingBehaviour>();
    }

    void Update()
    {

    }

}