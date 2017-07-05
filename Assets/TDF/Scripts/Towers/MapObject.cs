using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapObjectType
{
    GROUND, SPAWNPOINT, BASE, TOWER
}
public class MapObject : MonoBehaviour
{
    public int posX, posZ;
    public bool isBuildable = true;
    public bool isWalkable = true;
    MapObjectType type = MapObjectType.GROUND;
    GameObject tower;
    void OnMouseDown()
    {
        if (isBuildable)
        {
            if (LevelHandler.Instance.selectedTower != -1)
            {
                if (LevelHandler.Instance.PlaceTower(posX, posZ))
                {
                    this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
					isBuildable = false;
                }
            }
        }
    }
    void OnMouseExit()
    {
		this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }
    void OnMouseEnter()
    {
        if (LevelHandler.Instance.selectedTower != -1)
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
}
