using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class InteractableBehaviour : MonoBehaviour
{
    public UnityEvent<Vector2Int> OnInteract;
    /// <summary>
    /// Requests for interactable positions. Methods listening to this
    /// should add interactable positions to the HashSet. Note that the 
    /// interactable positions are a hash set since there cannot be two 
    /// or more of the same positions.
    /// </summary>
    public UnityEvent<HashSet<Vector2Int>> OnGetInteractablePositions;

    /// <summary>
    /// Is this building currently executing it's interaction?
    /// </summary>
    public bool IsInteracting { get; set; } = false;
    /// <summary>
    /// Can this building start an interaction?
    /// </summary>
    public bool CanInteract { get; set; } = true;

    public ICollection<Vector2Int> GetInteractablePositions()
    {
        HashSet<Vector2Int> interactablePositions = new HashSet<Vector2Int>();
        OnGetInteractablePositions.Invoke(interactablePositions);
        return interactablePositions;
    }

    public void Interact(Vector2Int gridPos)
    {
        if (!IsInteracting && CanInteract)
            OnInteract.Invoke(gridPos);
    }
}