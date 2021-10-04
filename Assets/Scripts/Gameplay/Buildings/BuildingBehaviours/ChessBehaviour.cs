using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChessBehaviour : MonoBehaviour
{
    public UnityEvent OnMoveFailed;
    public UnityEvent OnMoveSuccessful;

    [Header("Settings")]
    [SerializeField]
    private MoveBuildingAction queuedMoveAction;

    [Header("Dependencies")]
    [SerializeField]
    private BuildingBehaviour buildingBehaviour;
    [SerializeField]
    private InteractableBehaviour interactableBehaviour;
    
    private BuildingManager buildingManager;

    private void Awake()
    {
        buildingManager = ServiceLocator.Instance.GetService<BuildingManager>();

        interactableBehaviour.OnInteract.AddListener(OnInteract);
    }

    private void OnMoveResult(bool isSuccess)
    {
        if (isSuccess)
            OnMoveFailed.Invoke();
        else
            OnMoveSuccessful.Invoke();
    }

    private void OnInteract(Vector2Int interactPosition)
    {
        if (queuedMoveAction != null)
        {
            buildingManager.DequeueMoveAction(queuedMoveAction);
            if (interactPosition == queuedMoveAction.EndPosition)
            {
                // Don't add any new position if the player clicks the same position.
                // Essentially deletes a queued move for a position if the player
                // clicks on that position.
                queuedMoveAction = null;
                return;
            }
        }

        queuedMoveAction = new MoveBuildingAction(buildingBehaviour.Position, interactPosition, OnMoveResult);
        buildingManager.QueueMoveAction(queuedMoveAction);
    }
}