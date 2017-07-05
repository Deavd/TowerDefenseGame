using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelHandler : MonoBehaviour
{
    //text for timer
    public Text gameTimerText;
    //text for money
    public TextMeshProUGUI gameMoneyText;
    public bool isPaused { get; set; }
    //the time the level started
    private float startTime;

    //the players money
    private double money = 600;

    //TOWER PANEL
    public List<GameObject> towers;
    public GameObject towerButton;
    public GameObject towerButtonPanelContainer;
    public int selectedTower = -1;
    // END TOWER PANEL
    public double Money
    {
        get
        {
            return this.money;
        }
        set
        {
            //MoneyAnimation(value);
            this.money = value;
            //Update gui
            gameMoneyText.text = value.ToString() + "$";

        }
    }
    public List<TowerInformation> aviableTowers = new List<TowerInformation>();
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
        Money = money;
        //load all aviable towers
        LoadTowers();
        //add them to the bar
        LoadTowerBar();
        //create the map
        MapCreator.Instance.createMap(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);
        //start wave
        WaveHandler.Instance.spawnWave();
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
                    drawingTower.transform.position =  MapCreator.Instance.getPosition(hit.collider.GetComponent<MapObject>().posX,hit.collider.GetComponent<MapObject>().posZ);
                }
            }
            else
            {
                //create the prevTower
                drawingTower = Instantiate(towers[selectedTower], Input.mousePosition, Quaternion.identity);
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
        if (!isPaused)
        {
            time = Time.time - startTime;
            string min = Mathf.Floor(time / 60).ToString("00");
            string sec = (time % 60).ToString("00");
            gameTimerText.text = min + ":" + sec;
        }
    }
    public bool PlaceTower(int x, int z)
    {
        //check if player has enough money
        if (Money >= aviableTowers[selectedTower].BuyPrice)
        {
            //handle economy
            Money -= aviableTowers[selectedTower].BuyPrice;
            //place the tower
            GameObject towerObj = Instantiate(towers[selectedTower], MapCreator.Instance.getPosition(x, z), Quaternion.identity);
            Tower testTower = towerObj.GetComponent<Tower>();
            testTower.LoadInformations(aviableTowers[selectedTower]);
            testTower.Build();
            return true;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Money + " PRICE: " + aviableTowers[selectedTower].BuyPrice);
        }
        return false;
    }
    
    //triggered from listener;; select a tower with button in panel
    public void SelectTower(int id)
    {
        //check if player has enough money
        if (Money >= aviableTowers[id].BuyPrice)
        {
            //change selected tower
            selectedTower = id;
        }
        else
        {
            Debug.LogError("Not enough money; MONEY:" + Money + " PRICE: " + aviableTowers[id].BuyPrice);
        }
    }
    //loads the aviable towers into the panelbar
    public void LoadTowerBar()
    {
        foreach (TowerInformation towerInfo in aviableTowers)
        {
            //create button
            towerInfo.selectButton = Instantiate(towerButton).GetComponent<Button>();
            //add a listener
            int id = towerInfo.id;
            towerInfo.selectButton.onClick.AddListener(() => SelectTower(id));
            //put it into the panel
            towerInfo.selectButton.transform.SetParent(towerButtonPanelContainer.transform);
            //change button text
            towerInfo.selectButton.GetComponentsInChildren<Text>()[0].text = towerInfo.name;
        }
    }
    public void LoadTowers()
    {
        TowerInformation RocketLauncher = new TowerInformation(
            "Rocket Launcher",                          //Display Name
            1,                                          //Level
            3,                                          //Max Level
            new float[] { 4, 5, 6 },                    //Range
            new float[] { 100, 200, 300 },              //Damage
            new float[] { 0.25f, 0.2f, 0.15f },         //Attackspeed
            new float[] { 0.1f, 2f, 5f },               //Build Time
            new double[] { 150, 100, 200 },             //Buy Price
            new double[] { 1, 2, 3 },                   //Sell Price
            new MissleTypes[] { MissleTypes.ROCKET },   //Missle Type
            0                                           //Tower ID
        );
        aviableTowers.Add(RocketLauncher);
    }
}
