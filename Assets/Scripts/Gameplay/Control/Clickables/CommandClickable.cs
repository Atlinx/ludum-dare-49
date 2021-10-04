using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CommandClickable : MonoBehaviour, IClickable
{

    public Dictionary<Vector2Int, GameObject> CommandDisplayobjects = new Dictionary<Vector2Int, GameObject>();


    public Color tintForHover;

    public UnityEvent CannotMoveEvent;
    
    private InteractableBehaviour interactableBehaviour;
    private GameControl control;

    private GameObject hasQueue;

    private ChessBehaviour chessBehaviour;
    
    public void OnClicked(GameControl control)
    {
        Debug.Log("COMMAND CLICKED");
        this.control = control;
        interactableBehaviour = GetComponent<InteractableBehaviour>();

        chessBehaviour = GetComponent<ChessBehaviour>();
        
        if (!interactableBehaviour.CanInteract)
        {
            control.StopCurrentClickAction();
            return;
        }

        var commandGrids = interactableBehaviour.GetInteractablePositions();
        
        if (commandGrids == null || commandGrids.Count == 0)
        {
            CannotMoveEvent.Invoke();
            control.StopCurrentClickAction();
            return;
        }
        
        foreach (var grid in commandGrids)
        {
            //Debug.Log(grid);
            var displayObject = control.commandDisplayPool.Get();
            displayObject.transform.position = new Vector3(grid.x, grid.y);
            //displayObject.transform.localScale = Vector3.one * 0.6f;
            displayObject.GetComponent<SpriteRenderer>().color = Color.white;
            CommandDisplayobjects.Add(grid, displayObject);
        }
    }

    private GameObject lastGameObject;
    
    public void ClickUpdate()
    {
        
        
        var mouseGridPos = BuildingUtility.GetGridFromWorld(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        if (CommandDisplayobjects.TryGetValue(mouseGridPos, out var objd))
        {
            if (lastGameObject != objd)
            {
                //if(lastGameObject != null) lastGameObject.transform.localScale = Vector3.one * 0.6f;
                //objd.transform.localScale = Vector3.one * 0.8f;
                objd.GetComponent<SpriteRenderer>().color = tintForHover;
                if(lastGameObject != null) lastGameObject.GetComponent<SpriteRenderer>().color = Color.white;
                lastGameObject = objd;
                
                
            }

        }
        
        
        
        

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (CommandDisplayobjects.TryGetValue(mouseGridPos, out var obj))
            {
                interactableBehaviour.Interact(mouseGridPos);
                if(hasQueue == null) hasQueue = control.displayIsQueuedPool.Get();
                hasQueue.transform.position = this.transform.position;
                chessBehaviour.OnMoveFailed.AddListener(DisabledHasQueue);
                chessBehaviour.OnMoveSuccessful.AddListener(DisabledHasQueue);
                Debug.Log("Interacted at spot " + mouseGridPos);
                
            }
            
            control.StopCurrentClickAction();
        }
    }
    
    

    public void OnExit()
    {
        //Debug.Log("COMMMAND EXIT");
        foreach (var displayObj in CommandDisplayobjects.Values)
        {
            control.commandDisplayPool.Put(displayObj);
        }
        
        CommandDisplayobjects.Clear();
    }

    public void DisabledHasQueue()
    {
        Debug.Log("Disabled Has Queued");
        control.displayIsQueuedPool.Put(hasQueue);
        hasQueue = null;
        chessBehaviour.OnMoveFailed.RemoveListener(DisabledHasQueue);
        chessBehaviour.OnMoveSuccessful.RemoveListener(DisabledHasQueue);
    }
}
