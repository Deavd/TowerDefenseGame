using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class BuildManager : MonoBehaviour {
	//TOWER PANEL
    public List<GameObject> TowerList; //prefab
    private List<Tower> _towers = new List<Tower>();
    public GameObject towerButton;  //prefab
    public GameObject towerButtonPanelContainer;  //prefab on scene
    public static int selectedTower = -1;
    // END TOWER PANEL
    private GameObject _drawingTower;

    private float _towerPreviewUpdateTime = 0.1f;
    private float _towerPreviewLastUpdateTime = 0f;

	private static BuildManager _instance;
    public static BuildManager Instance
    {
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<BuildManager>() : _instance) == null ? _instance = new GameObject().AddComponent<BuildManager>(): _instance;
        }
    }
    void Start()
    {
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
    public Tower LoadTower(int i){
        return i != -1 ?_towers[i] : null;
    }
    public Tower LoadTower(){
        return selectedTower != -1 ?_towers[selectedTower] : null;
    }
    private Ray _ray;
    private RaycastHit _hit;
	private void TowerPreview()
    {

        if(Time.time - _towerPreviewLastUpdateTime < _towerPreviewUpdateTime){
            return;
        }
        //check if a tower is selected
        if (selectedTower != -1)
        {
            if (_drawingTower != null)
            {
                _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //check if mouse is over a clickableObj -> may change to another Mask
                if (Physics.Raycast(_ray, out _hit, 105f, LayerMask.GetMask("Map")))
                {
                    _towerPreviewLastUpdateTime = Time.time;
                    //change prevTower position
                    MapObject mapObject = _hit.collider.GetComponent<MapObject>();
                    Vector3 placePosition = MapManager.Instance.getPosition(mapObject.posX,mapObject.posZ);
                    //placePosition.y = drawingTower.transform.localScale.y/2;
                    _drawingTower.transform.position = placePosition;
                    TowerMenuUI.Instance.ShowRangePreview(LoadTower().Stats.Range.Value, placePosition); 
                }
            }
            else
            {
                //create the prevTower
                _drawingTower = Instantiate(TowerList[selectedTower], Input.mousePosition, Quaternion.identity);
            }
        }
        else if (_drawingTower != null)
        {
            Destroy(_drawingTower);
        }
    }
    public Tower PlaceTower(int x, int z)
    {
        //check if player has enough money
        //double buyPrice = tower.BuyPrice;
        double buyPrice = LoadTower().Stats.BuyPrice.Value;
        Debug.Log(buyPrice);
        if (LevelManager.Instance.Money >= buyPrice)
        {
            //handle economy
            LevelManager.Instance.Money -= buyPrice;
            //place the tower$
            Vector3 placePosition = MapManager.Instance.getPosition(x, z);
            //placePosition.y = tower.transform.localScale.y/2;
            GameObject towerObj = Instantiate(TowerList[selectedTower], placePosition, Quaternion.identity);
            Tower testTower = towerObj.GetComponent<Tower>();
            
            NavMeshObstacle navObstacle = towerObj.GetComponent<NavMeshObstacle>();
            navObstacle.enabled = true;
            testTower.Build();
            Destroy(_drawingTower);
            selectedTower = -1;
            return testTower;
        }
        else
        {
            ShowMessage.Instance.WriteMessageAt("You dont have enough money!", Camera.main.WorldToScreenPoint(MapManager.Instance.getPosition(x, z)), MessageType.ERROR);
            Debug.LogError("Not enough money; MONEY:" + LevelManager.Instance.Money + " PRICE: " + buyPrice);
        }
        return null;
    }
    
    //triggered from listener;; select a tower with button in panel
    public void SelectTower(int id)
    {
        TowerMenuUI.Instance.UnloadTowerGui();
        //check if player has enough money
        //if (LevelManager.Instance.Money >= towerObjs[id].GetComponent<Tower>().BuyPrice)
        if (LevelManager.Instance.Money >= LoadTower(id).Stats.BuyPrice.Value)
        {
            //change selected tower
            selectedTower = id;
        }
        else
        {
           ShowMessage.Instance.WriteMessageAt("You dont have enough money!", Input.mousePosition, MessageType.ERROR, 12,0.3f);
        }
    }
    //loads the aviable towers into the panelbar
    public void LoadTowerBar()
    {
        int id = 0;
        foreach (GameObject towerObj in TowerList)
        {
            Tower tower = towerObj.GetComponent<Tower>();
            _towers.Add(tower);
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
