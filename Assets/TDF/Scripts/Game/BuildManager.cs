using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BuildManager : MonoBehaviour {
	//TOWER PANEL
    public List<GameObject> towerObjs;
    public GameObject towerButton;
    public GameObject towerButtonPanelContainer;
    public int selectedTower = -1;
    // END TOWER PANEL
    GameObject drawingTower;
    Ray ray;
    RaycastHit hit;
    float towerPreviewUpdateTime = 0.1f;
    float towerPreviewLastUpdateTime;

	LevelHandler Level;
	public BuildManager(LevelHandler level){
		Level = level;
        LoadTowerBar();
	}
    void Update()
    {
        TowerPreview();
        if (Input.GetMouseButtonDown(1))
        {
            selectedTower = -1;
        }
    }
    public Tower loadTower(){
        Debug.Log("SELECTET TOWER: "+selectedTower);
        return towerObjs[selectedTower].GetComponent<Tower>();
    }
	private void TowerPreview()
    {
        if(Time.time - towerPreviewLastUpdateTime < towerPreviewUpdateTime){
            return;
        }
        //check if a tower is selected
        if (selectedTower != -1)
        {
            if (drawingTower != null)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //check if mouse is over a clickableObj -> may change to another Mask
                if (Physics.Raycast(ray, out hit, 105f, LayerMask.GetMask("clickableObj")))
                {
                    towerPreviewLastUpdateTime = Time.time;
                    //change prevTower position
                    Vector3 placePosition = MapCreator.Instance.getPosition(hit.collider.GetComponent<MapObject>().posX,hit.collider.GetComponent<MapObject>().posZ);
                    //placePosition.y = drawingTower.transform.localScale.y/2;
                    drawingTower.transform.position = placePosition;
                }
            }
            else
            {
                //create the prevTower
                drawingTower = Instantiate(towerObjs[selectedTower], Input.mousePosition, Quaternion.identity);
            }
        }
        else if (drawingTower != null)
        {
            Debug.Log("DE");
            Destroy(drawingTower);
        }
    }
    public GameObject PlaceTower(int x, int z)
    {
        Tower tower = loadTower();
        //check if player has enough money
        double buyPrice = tower.BuyPrice;
        
        if (Level.Money >= buyPrice)
        {
            //handle economy
            Level.Money -= buyPrice;
            //place the tower$
            Vector3 placePosition = MapCreator.Instance.getPosition(x, z);
            //placePosition.y = tower.transform.localScale.y/2;
            GameObject towerObj = Instantiate(towerObjs[selectedTower], placePosition, Quaternion.identity);
            Tower testTower = towerObj.GetComponent<Tower>();
            
            NavMeshObstacle navObstacle = towerObj.GetComponent<NavMeshObstacle>();
            navObstacle.enabled = true;
            testTower.Build();
            Destroy(drawingTower);
            Debug.Log("REMOVING SELECTED TOWER");
            this.selectedTower = -1;
            return towerObj;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Level.Money + " PRICE: " + buyPrice);
        }
        return null;
    }
    
    //triggered from listener;; select a tower with button in panel
    public void SelectTower(int id)
    {
        Debug.Log(id);
        //check if player has enough money
        if (Level.Money >= towerObjs[id].GetComponent<Tower>().BuyPrice)
        {
            Debug.Log("SETTING ID TO "+id);
            //change selected tower
            selectedTower = id;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Level.Money + " PRICE: " + towerObjs[id].GetComponent<Tower>().BuyPrice);
        }
    }
    //loads the aviable towers into the panelbar
    public void LoadTowerBar()
    {
        int id = 0;
        foreach (GameObject towerObj in towerObjs)
        {
            Tower tower = towerObj.GetComponent<Tower>();
            Debug.Log("TOWER INIT: "+tower.displayName+" id: "+id);
            //create button
            tower.selectButton = Instantiate(towerButton).GetComponent<Button>();
            //add a listener
            tower.id = id;
            tower.selectButton.onClick.AddListener(() => SelectTower(tower.id));
            //put it into the panel
            tower.selectButton.transform.SetParent(towerButtonPanelContainer.transform);
            //change button text
            tower.selectButton.GetComponentsInChildren<Text>()[0].text = tower.name;
            id++;
        }
    }
}
