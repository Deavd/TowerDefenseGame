using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BuildManager : MonoBehaviour {
	//TOWER PANEL
    public List<GameObject> towerObjs; //prefab
    public GameObject towerButton;  //prefab
    public GameObject towerButtonPanelContainer;  //prefab on scene
    public static int selectedTower = -1;
    // END TOWER PANEL
    static GameObject drawingTower;
    Ray ray;
    RaycastHit hit;
    float towerPreviewUpdateTime = 0.1f;
    float towerPreviewLastUpdateTime;

	private static BuildManager _instance;
    public static BuildManager Instance
    {
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<BuildManager>() : _instance) == null ? new GameObject().AddComponent<BuildManager>(): _instance;
        }
    }
	public void initGUI (){
        LoadTowerBar();
        enabled = true;
	}
    void Start()
    {
        enabled = false;
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
                    Vector3 placePosition = MapManager.Instance.getPosition(hit.collider.GetComponent<MapObject>().posX,hit.collider.GetComponent<MapObject>().posZ);
                    //placePosition.y = drawingTower.transform.localScale.y/2;
                    drawingTower.transform.position = placePosition;
                    TowerMenuUI.Instance.ShowRangePreview(loadTower().Stats.Range.Value, placePosition); 
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
    public Tower PlaceTower(int x, int z)
    {
        Tower tower = loadTower();
        //check if player has enough money
        //double buyPrice = tower.BuyPrice;
        double buyPrice = tower.Stats.BuyPrice.Value;
        
        if (LevelManager.Instance.Money >= buyPrice)
        {
            //handle economy
            LevelManager.Instance.Money -= buyPrice;
            //place the tower$
            Vector3 placePosition = MapManager.Instance.getPosition(x, z);
            //placePosition.y = tower.transform.localScale.y/2;
            GameObject towerObj = Instantiate(towerObjs[selectedTower], placePosition, Quaternion.identity);
            Tower testTower = towerObj.GetComponent<Tower>();
            
            NavMeshObstacle navObstacle = towerObj.GetComponent<NavMeshObstacle>();
            navObstacle.enabled = true;
            testTower.Build();
            Destroy(drawingTower);
            Debug.Log("REMOVING SELECTED TOWER");
            selectedTower = -1;
            return testTower;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + LevelManager.Instance.Money + " PRICE: " + buyPrice);
        }
        return null;
    }
    
    //triggered from listener;; select a tower with button in panel
    public void SelectTower(int id)
    {
        Debug.Log( LevelManager.Instance.Money);
        Debug.Log( id);
        Debug.Log( towerObjs[id]);
        //check if player has enough money
        //if (LevelManager.Instance.Money >= towerObjs[id].GetComponent<Tower>().BuyPrice)
        if (LevelManager.Instance.Money >= towerObjs[id].GetComponent<Tower>().Stats.BuyPrice.Value)
        {
            Debug.Log("SETTING ID TO "+id);
            //change selected tower
            selectedTower = id;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + LevelManager.Instance.Money + " PRICE: " + towerObjs[id].GetComponent<Tower>().Stats.BuyPrice.Value);
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
            Button button = Instantiate(towerButton).GetComponent<Button>();
            //add a listener
            tower.id = id;
            button.onClick.AddListener(() => SelectTower(tower.id));
            //put it into the panel
            button.transform.SetParent(towerButtonPanelContainer.transform);
            //change button text
            button.GetComponentsInChildren<Text>()[0].text = tower.displayName;
            id++;
        }
    }
}
