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
    MapObjectType type = MapObjectType.GROUND;
    public Tower Tower;
    private Color color;
    void Start(){
        color = this.GetComponent<Renderer>().material.color;
    }

    void OnMouseExit()
    {
		this.GetComponent<Renderer>().material.color = color;
    }
    void OnMouseEnter()
    {
        if (BuildManager.selectedTower != -1)
        {
			if(isBuildable){
            	this.GetComponent<Renderer>().material.color = new Color(0, 1, 0, 1);
			}else{
				this.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
			}
        }
        else
        {
            this.GetComponent<Renderer>().material.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BuildManager.selectedTower != -1)
        {
            TowerMenuUI.Instance.UnloadTowerGui();
            if (isBuildable)
            {
                Tower towerObj = BuildManager.Instance.PlaceTower(posX, posZ);
                if (towerObj != null)
                {
                    this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
					isBuildable = false;
                    this.Tower = towerObj;
                    TowerMenuUI.Instance.LoadTowerGui(this);
                }
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
