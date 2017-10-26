using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PostProcessing;
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

    public static bool IsGameOver { get; set; }
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
                value = 0;
            }
            if(value == 0 && !IsGameOver){
                Defeat();
            }
            gameLifesText.text = value.ToString();
        }
    }
    void Awake()
    {
        Lifes = _lifes;

        mainCamera =  Camera.main;
        healthBar =  GameObject.Find("Healthbar");
        GameUI = GameObject.Find("GameUI").GetComponent<Canvas>();
        
        _postProcess =  Camera.main.GetComponent<PostProcessingBehaviour>();
        _postColorGrading = _postProcess.profile.colorGrading;
        _postColorGradingSettings = _postColorGrading.settings;
        _postColorGradingSettings.basic.saturation = 1f;
        _postColorGrading.settings = _postColorGradingSettings;
        _rect = GameOverGui.GetComponent<RectTransform>();
        //gameLifesText.text = lifes.ToString();
        IsPaused = false;
        IsGameOver = false;
        initScreens();
    }
    public void Defeat()
    {
        IsGameOver = true;
        _defeatEffect = true;
        SwitchToScreen(TDScreen.GAME_OVER);
    }
    public GameObject GameOverGui;

    bool _defeatEffect = false;

    PostProcessingBehaviour _postProcess;
    ColorGradingModel _postColorGrading;
    ColorGradingModel.Settings _postColorGradingSettings;
    float _fadeSaturation = 1.0f;
    float timescale = 1;
    RectTransform _rect;
    void Update()
    {
        UpdateTimer();
        if(_defeatEffect){
            _rect.localScale = new Vector3(1-timescale,1-timescale,1-timescale);
            if(Time.timeScale > 0f){
                timescale =Time.timeScale = Mathf.Clamp(Time.timeScale-0.01f, 0,1);
            }else if(Time.timeScale == 0){
                _defeatEffect = false;
            }
            if(_fadeSaturation > 0){
                _fadeSaturation -= 0.01f;
                _postColorGradingSettings.basic.saturation = _fadeSaturation;
                _postColorGrading.settings = _postColorGradingSettings;
            }else if(_fadeSaturation != 0){
                _fadeSaturation = 0;
                _postColorGradingSettings.basic.saturation = _fadeSaturation;
                _postColorGrading.settings = _postColorGradingSettings;
            }
        }
    }
    public void Win()
    {
        IsPaused = true;
    }
    public Dictionary<TDScreen, CanvasGroup> Screens = new Dictionary<TDScreen, CanvasGroup>();
    public CanvasGroup GameOverScreen;
    public CanvasGroup PauseScreen;
    public CanvasGroup HUD;

    void initScreens(){
        Screens.Add(TDScreen.GAME_OVER, GameOverScreen);
        Screens.Add(TDScreen.HUD, HUD);
        Screens.Add(TDScreen.PAUSE, PauseScreen);
    }
    public void SwitchToScreen(TDScreen screen){
        foreach(CanvasGroup canvas in Screens.Values){
            if(canvas == Screens[screen]){
                continue;
            }
            canvas.interactable = false;
            canvas.gameObject.SetActive(false);
        }
        Screens[screen].interactable = true;
        Screens[screen].gameObject.SetActive(true);
    }
    public void PauseGame(){
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f:1f;
        PauseScreen.alpha = IsPaused ? 1f:0f;
        if(IsPaused){
            SwitchToScreen(TDScreen.PAUSE);
        }else{
            SwitchToScreen(TDScreen.HUD);
        }
    }
    public void StartLevel(int mapSizeX, int mapSizeZ, int levelDifficulty, GameObject mapGroundObject)
    {
        Money = _money;        
        MapManager.Instance.createMap(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);
        startTime = Time.time;
        SwitchToScreen(TDScreen.HUD);
    }


    private void UpdateTimer()
    {
        if (!IsPaused && !IsGameOver)
        {
            disTime = Time.time - startTime;
            string min = Mathf.Floor(disTime / 60).ToString("00");
            string sec = (disTime % 60).ToString("00");
            gameTimerText.text = min + ":" + sec;
        }
    }
    void OnApplicationQuit()
    {
        _postColorGradingSettings.basic.saturation = 1;
        _postColorGrading.settings = _postColorGradingSettings;
    }
}
public enum TDScreen{
    HUD,
    GAME_OVER,
    PAUSE
}