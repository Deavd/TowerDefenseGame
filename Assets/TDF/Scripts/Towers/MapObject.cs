using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MapObjectType
{
    GROUND, SPAWNPOINT, BASE, TOWER
}
public class MapObject : MonoBehaviour, IPointerClickHandler
{
    public int posX, posZ;
    public bool isBuildable = true;
    public bool isWalkable = true;
    private MapObjectType _type = MapObjectType.GROUND;
    public Tower Tower;
    private Color _color;
    private Renderer _renderer;
    void Awake(){
        this.gameObject.layer = LayerMask.NameToLayer("Map");
        this.gameObject.tag = "Map";
        _renderer = this.GetComponent<Renderer>();
        _color = _renderer.material.color;
    }

    void OnMouseExit()
    {
		this.GetComponent<Renderer>().material.color = _color;
    }
    void OnMouseEnter()
    {
       
        if (BuildManager.selectedTower != -1)
        {
			if(isBuildable && !MapManager.Instance.checkObstruction(posX,posZ)){
            	_renderer.material.color = new Color(0, 1, 0, 1);
			}else{
				_renderer.material.color = new Color(1, 0, 0, 1);
			}
        }
        else
        {
            _renderer.material.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _renderer.material.color = new Color(0.1f, 0.7f, 0.7f, 1);
        if (BuildManager.selectedTower != -1)
        {
            TowerMenuUI.Instance.UnloadTowerGui();
            if (isBuildable)
            {
                if(!MapManager.Instance.checkObstruction(posX,posZ)){
                    Tower towerObj = BuildManager.Instance.PlaceTower(posX, posZ);
                    if (towerObj != null)
                    {
                        _renderer.material.color = new Color(1, 1, 1, 1);
                        isBuildable = false;
                        this.Tower = towerObj;
                        TowerMenuUI.Instance.LoadTowerGui(this);
                    }
                }else{
                    ShowMessage.Instance.WriteMessageAt("Cannot place a tower here!", eventData.position, MessageType.WARNING, 14, 0);
                    //Cannot play in the path of the enemies
                }
            }else{
                ShowMessage.Instance.WriteMessageAt("Cannot place a tower here!", eventData.position, MessageType.WARNING, 14, 0);
                //cannot build here
            }
        }else{
            if(Tower != null){
                TowerMenuUI.Instance.LoadTowerGui(this);
            }else{
                TowerMenuUI.Instance.UnloadTowerGui();
            }
        }
    }
}
