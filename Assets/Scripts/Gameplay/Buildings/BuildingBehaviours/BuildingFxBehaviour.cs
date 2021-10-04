using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingFxBehaviour : MonoBehaviour
{
    [SerializeField]
    private BuildingBehaviour buildingBehaviour;
    public UnityEvent OnRemove;
    public UnityEvent OnPlaced;

    public float destroyLifetime = 5f;

    private void Awake()
    {
        buildingBehaviour.OnRemove += Remove;
        buildingBehaviour.OnPlaced += Placed;
    }

    private void Remove()
    {
        OnRemove.Invoke();
        transform.parent = null;
        Destroy(gameObject, destroyLifetime);
    }

    private void Placed()
    {
        OnPlaced.Invoke();
    }
}
