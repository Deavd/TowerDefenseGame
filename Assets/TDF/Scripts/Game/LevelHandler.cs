using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class LevelHandler : MonoBehaviour
{
    //text for timer
    public Text gameTimerText;
    //text for money
    public TextMeshProUGUI gameMoneyText;
    public TextMeshProUGUI gameLifesText;
    public bool IsPaused { get; set; }
    //the time the level started
    private float startTime;



    //TOWER PANEL
    public List<GameObject> towerObjs;
    public GameObject towerButton;
    public GameObject towerButtonPanelContainer;
    public int selectedTower = -1;
    // END TOWER PANEL
    //the players money
    public static double money = 600;
    public double Money
    {
        get
        {
            return money;
        }
        set
        {
            //MoneyAnimation(value);
            money = value;
            //Update gui
           gameMoneyText.text = value.ToString() + "$";

        }
    }
    //the players lifes
    private static int lifes = 20;
    public int Lifes
    {
        get
        {
            return lifes;
        }
        set
        {
            //MoneyAnimation(value);
            lifes = value;
            //Update gui
            gameLifesText.text = value.ToString();

        }
    }
    public static LevelHandler Instance
    {
        get
        {
            return (LevelHandler)FindObjectOfType(typeof(LevelHandler));
        }
    }
    //function to start thge level
    public void StartLevel(int mapSizeX, int mapSizeZ, int levelDifficulty, GameObject mapGroundObject)
    {
        Money = Money;
        //add them to the bar
        LoadTowerBar();
        //create the map
        MapCreator.Instance.createMap(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);
        //start wave
        WaveHandler.Instance.startSpawningWaves();
        //start the timer
        startTime = Time.time;
    }

    void Update()
    {
        UpdateTimer();
        TowerPreview();
        if (Input.GetMouseButtonDown(1))
        {
            selectedTower = -1;
        }
    }
    GameObject drawingTower;
    Ray ray;
    RaycastHit hit;
    float towerPreviewUpdateTime = 0.1f;
    float towerPreviewLastUpdateTime;
    //creates a Tower preview
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
    float time;
    //display timer
    private void UpdateTimer()
    {
        if (!IsPaused)
        {
            time = Time.time - startTime;
            string min = Mathf.Floor(time / 60).ToString("00");
            string sec = (time % 60).ToString("00");
            gameTimerText.text = min + ":" + sec;
        }
    }
    public Tower loadTower(){
        return towerObjs[selectedTower].GetComponent<Tower>();
    }
    public GameObject PlaceTower(int x, int z)
    {
        Tower tower = loadTower();
        //check if player has enough money
        double buyPrice = tower.BuyPrice;
        
        if (Money >= buyPrice)
        {
            //handle economy
            Money -= buyPrice;
            //place the tower$
            Vector3 placePosition = MapCreator.Instance.getPosition(x, z);
            //placePosition.y = tower.transform.localScale.y/2;
            GameObject towerObj = Instantiate(towerObjs[selectedTower], placePosition, Quaternion.identity);
            Tower testTower = towerObj.GetComponent<Tower>();
            NavMeshObstacle navObstacle = towerObj.GetComponent<NavMeshObstacle>();
            navObstacle.enabled = true;
            testTower.Build();
            Destroy(drawingTower);
            this.selectedTower = -1;
            return towerObj;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Money + " PRICE: " + buyPrice);
        }
        return null;
    }
    
    //triggered from listener;; select a tower with button in panel
    public void SelectTower(int id)
    {
        //check if player has enough money
        if (Money >= towerObjs[id].GetComponent<Tower>().BuyPrice)
        {
            //change selected tower
            selectedTower = id;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Money + " PRICE: " + loadTower().BuyPrice);
        }
    }
    //loads the aviable towers into the panelbar
    public void LoadTowerBar()
    {
        foreach (GameObject towerObj in towerObjs)
        {
            Tower tower = towerObj.GetComponent<Tower>();
            //create button
            tower.selectButton = Instantiate(towerButton).GetComponent<Button>();
            //add a listener
            int id = tower.id;
            tower.selectButton.onClick.AddListener(() => SelectTower(id));
            //put it into the panel
            tower.selectButton.transform.SetParent(towerButtonPanelContainer.transform);
            //change button text
            tower.selectButton.GetComponentsInChildren<Text>()[0].text = tower.name;
        }
    }
    public GameObject missle;
}
