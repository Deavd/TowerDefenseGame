using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<LevelManager>() : _instance) == null ? new GameObject().AddComponent<LevelManager>(): _instance;
        }
    }
    public GameObject RangeUI;              //addfromscene
    public Text gameTimerText;              //addfromscene
    public TextMeshProUGUI gameMoneyText;   //addFromScene
    public TextMeshProUGUI gameLifesText;   //addFromScene
    public GameObject healthBar;            //addFromScene
    public Canvas GameUI;                   //addFromScene

    public static Camera mainCamera;
    //the time the level started
    private float startTime;                //timeTheLevelStarted
    private float disTime;

    public static bool IsPaused { get; set; }

    private double _money = 600;
    public double Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            gameMoneyText.text = value.ToString() + "$";
        }
    }
    private int _lifes = 20;
    public int Lifes
    {
        get
        {
            return _lifes;
        }
        set
        {
            _lifes = value;
            if(_lifes <= 0){
                Defeat();
                value = 0;
            }
            gameLifesText.text = value.ToString();
        }
    }
    void Awake()
    {
        mainCamera =  Camera.main;
        healthBar =  GameObject.Find("Healthbar");
        GameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        Debug.Log("init health");
        //gameLifesText.text = lifes.ToString();
    }
    public void Defeat()
    {
        IsPaused = true;
    }
    public void Win()
    {
        IsPaused = true;
    }
    
    public void StartLevel(int mapSizeX, int mapSizeZ, int levelDifficulty, GameObject mapGroundObject)
    {
        Money = _money;        
        MapManager.Instance.createMap(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);
        startTime = Time.time;
        BuildManager.Instance.initGUI();
    }

    void Update()
    {
        UpdateTimer();
    }
    private void UpdateTimer()
    {
        if (!IsPaused)
        {
            disTime = Time.time - startTime;
            string min = Mathf.Floor(disTime / 60).ToString("00");
            string sec = (disTime % 60).ToString("00");
            gameTimerText.text = min + ":" + sec;
        }
    }
}
