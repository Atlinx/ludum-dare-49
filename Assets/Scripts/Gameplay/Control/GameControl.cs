using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{

    public GraphicRaycaster UIRaycast;
    public EventSystem eventSystem;
    public BuildingManager buildingManager;
    public StatsUI statsUI;
    public PrefabPool commandDisplayPool;
    public PrefabPool displayIsQueuedPool;
    private IClickable CurrentClicked;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        
        if (CurrentClicked == null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            var clickable = CheckForIClickable();

            if (clickable != null)
            {
                CurrentClicked = clickable;
                CurrentClicked.OnClicked(this);
                return;
            }
        }
        
        CurrentClicked?.ClickUpdate();
    }


    public void StopCurrentClickAction()
    {
        CurrentClicked.OnExit();
        CurrentClicked = null;
    }
    

    IClickable CheckForIClickable()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Check UI

            var m_PointerDataEvent = new PointerEventData(eventSystem);

            m_PointerDataEvent.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            
            UIRaycast.Raycast(m_PointerDataEvent, results);

            foreach (var result in results)
            {
                var clickable = result.gameObject.GetComponent<IClickable>();
                if ( clickable != null)
                {
                    return clickable;
                }
            }
            
            //Check In Game

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                return hit.collider.gameObject.GetComponent<IClickable>();
            }

        }

        return null;
    }
}
