using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.PostProcessing;
public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<LevelManager>() : _instance) == null ? _instance = new GameObject().AddComponent<LevelManager>(): _instance;
        }
    }
    public GameObject RangeUI;              //addfromscene
    public Text gameTimerText;              //addfromscene
    public Text gameMoneyText;   //addFromScene
    public Text gameLifesText;   //addFromScene
    public Canvas GameUI;                   //addFromScene

    public static Camera mainCamera;
    //the time the level started
    private float _startTime;                //timeTheLevelStarted
    private float _disTime;

    public static bool IsGameOver { get; set; }
    public static bool IsPaused { get; set; }

    private double _money = 250;
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
        //definiere Startvariabeln
        Lifes = _lifes;

        mainCamera =  Camera.main;
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
        //wird beim Verlieren aufgerufen
        IsGameOver = true;
        _defeatEffect = true;
        //wechsle Fenster
        SwitchToScreen(TDScreen.GAME_OVER);
    }
    public GameObject GameOverGui;

    private bool _defeatEffect = false;

    private PostProcessingBehaviour _postProcess;
    private ColorGradingModel _postColorGrading;
    private ColorGradingModel.Settings _postColorGradingSettings;
    private float _fadeSaturation = 1.0f;
    private float _scale = 1;
    private RectTransform _rect;
    void Update()
    {
        UpdateTimer();
        if(_defeatEffect){
            Time.timeScale = 1;
            _rect.localScale = new Vector3(1-_scale,1-_scale,1-_scale);
            _scale -= 0.1f;
            if(Time.timeScale != 1){
                Time.timeScale = 1;
            }
            if(_fadeSaturation > 0){
                _fadeSaturation -= 0.1f;
                _postColorGradingSettings.basic.saturation = _fadeSaturation;
                _postColorGrading.settings = _postColorGradingSettings;
            }else if(_fadeSaturation != 0){
                _fadeSaturation = 0;
                _postColorGradingSettings.basic.saturation = _fadeSaturation;
                _postColorGrading.settings = _postColorGradingSettings;

                _defeatEffect = false;
                Time.timeScale = 0;
            }
        }
    }
    public void Win()
    {
        //wird beim Gewinnen des Spiels aufgerufen
        IsGameOver = true;
        _defeatEffect = true;
        SwitchToScreen(TDScreen.WIN);
    }
    public Dictionary<TDScreen, CanvasGroup> Screens = new Dictionary<TDScreen, CanvasGroup>();
    public CanvasGroup GameOverScreen;
    public CanvasGroup PauseScreen;
    public CanvasGroup HUD;
    public CanvasGroup WinScreen;

    void initScreens(){
        //lädt alle Screens
        Screens.Add(TDScreen.GAME_OVER, GameOverScreen);
        Screens.Add(TDScreen.HUD, HUD);
        Screens.Add(TDScreen.PAUSE, PauseScreen);
        Screens.Add(TDScreen.WIN, WinScreen);
    }
    public void SwitchToScreen(TDScreen screen){
        //Funktion um zu einem Screen zu wechseln
        foreach(CanvasGroup canvas in Screens.Values){
            if(canvas == Screens[screen]){
                continue;
            }
            //setzte deaktiviere alle anderen Screens
            canvas.interactable = false;
            canvas.gameObject.SetActive(false);
        }
        //aktiviere diesen Screen
        Screens[screen].interactable = true;
        Screens[screen].gameObject.SetActive(true);
    }
    public void PauseGame(){
        ///wird über Button geöffnet
        IsPaused = !IsPaused;
        Time.timeScale = IsPaused ? 0f:1f;
        PauseScreen.alpha = IsPaused ? 1f:0f;
        if(IsPaused){
            SwitchToScreen(TDScreen.PAUSE);
        }else{
            SwitchToScreen(TDScreen.HUD);
        }
    }
    int fastForwardState = 0;
    public void ToggleFastForward(){
        switch(fastForwardState){
            case 0:
                Time.timeScale = 2;
                fastForwardState++;
                break;
            case 1:
                Time.timeScale = 4;
                fastForwardState++;
            break;
            case 2:
                Time.timeScale = 1;
                fastForwardState  = 0;
            break;
        }
    }
    public void StartLevel(int mapSizeX, int mapSizeZ, int levelDifficulty, GameObject mapGroundObject)
    {
        Money = _money;        
        MapManager.Instance.createMap(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);
        _startTime = Time.time;
        SwitchToScreen(TDScreen.HUD);
    }


    private void UpdateTimer()
    {
        if (!IsPaused && !IsGameOver)
        {
            _disTime = Time.time - _startTime;
            string min = Mathf.Floor(_disTime / 60).ToString("00");
            string sec = (_disTime % 60).ToString("00");
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
    WIN,
    PAUSE
}