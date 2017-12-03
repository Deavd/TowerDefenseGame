using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class ClickHandler : MonoBehaviour
{
/*    Ray ray;
    RaycastHit hit;
    Collider lastHit = null;
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())    // is the touch on the GUI
        {
            /*PointerEventData data = EventSystem.current.gameObject.GetComponent<ExtendedEventSystem>().GetPointerEventData();
            Debug.Log(data.pointerEnter.name);*/
           /* return;
        }
        //Resulting ray is in world space, starting on the near plane of the camera and going through position's (x,y) pixel coordinates on the screen (position.z is ignored).
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);


        if (Physics.Raycast(ray, out hit, 105f, LayerMask.GetMask("Map")))
        {
            if (lastHit != null)
            {
                lastHit.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
            }
            lastHit = hit.collider;
            if(lastHit.GetComponent<MapObject>().isBuildable){
                lastHit.GetComponent<Renderer>().material.color = new Color(0.1f, 1, 0.2f, 0.8f);
            }else{
                 lastHit.GetComponent<Renderer>().material.color = new Color(0.8f, 1, 0.2f, 0.1f);
            }
            

        }
}*/
}
