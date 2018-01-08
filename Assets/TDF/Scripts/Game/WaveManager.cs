using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
#region Instance
    private static WaveManager _instance;
	public static WaveManager Instance
	{
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<WaveManager>() : _instance) == null ? new GameObject().AddComponent<WaveManager>(): _instance;
        }
	}
#endregion
    //enemy objects
    public GameObject homeBase;
    public Text WaveUI;
    public Text WaveUITime;
    public GameObject[] enemies;
    public GameObject[] spawnpoints;
    public string[] Waves;
    public static int Wave = 0;
    public bool isRunning = true;  
    public bool isBuildingPhase = true;  
    public void startSpawningWaves()
    {
        Debug.Log("- Starting wave: "+Wave);
        isRunning = true;
        isBuildingPhase = false;
        //wave: "id:amount:period :::"
        if(hasNextWave()){
            spawnWave(Waves[Wave], Wave);
            Wave++;
        }else{
            if(EnemiesAlive == 0){
                Win();
            }
        }
    }
    public bool hasNextWave(){
        return Waves.Length > Wave;
    }
    private void spawnWave(string wave, int waveID)
    {
        StartCoroutine(SpawnWave(wave, waveID));
    }

  
    void Awake()
    {
        WaveStartTime=0;
        this.WaveUITime.text = "Building...";
    }
    void Update()
    {
        if(isRunning)
            WaveStartTime -= Time.deltaTime;
    }
    private float _delayToNextPartWave = 5.5f;
    private float _time = 0f;
    private string _wavePrefix;
    public float WaveStartTime 
    {
        get
        {
            return _time;
        }
        set
        {
            _time = value;
            if(isRunning){
                this.WaveUITime.text = _wavePrefix+_time.ToString("0.0");
            }
        }
    }
    public static int EnemiesAlive = 0;
    IEnumerator SpawnWave(String s, int waveID)
    {
        _wavePrefix = "Wave ends in: ";
        String[] partWaves = s.Split();
        WaveUI.text = Wave + "/" + "-";
        float timeNeeded = 0f;

        int id;
        float next = 0f;
        int amount;
        float period;
        foreach(String partWave in partWaves)
        {
            if(partWave == ""){
                continue;
            }
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            int.TryParse(parts[1], out amount);
            float.TryParse(parts[2], out period);
            float.TryParse(parts[3], out next);
            timeNeeded += amount * period + next;
        }
        WaveStartTime = timeNeeded;
        for(int j = 0; j < partWaves.Length; j++)
        {
            String partWave = partWaves[j];
            if(partWave == ""){
                continue;
            }
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            int.TryParse(parts[0], out id);
            int.TryParse(parts[1], out amount);
            float.TryParse(parts[2], out period);
            float.TryParse(parts[3], out next);
            for(int i = 0; i < amount; i++)
            {
                EnemiesAlive++;
                SoundManager.Instance.PlaySound("spawnSound");
                GameObject spawnedEnemy = Instantiate(enemies[id], spawnpoints[0].transform.position, Quaternion.identity);
                spawnedEnemy.GetComponent<NavMeshAgent>().SetDestination(homeBase.transform.position);
                Enemy enemy = spawnedEnemy.GetComponent<Enemy>();
                for(int level = 0; level < waveID; level++){
                    Debug.Log("LEVELUP");
                    enemy.Stats.LevelUpAll();
                }
                if(i == amount-1){
                    yield return null;
                } 
                yield return new WaitForSeconds(period);
            }
            if(j == partWaves.Length-1){
                yield return null;
            }else{
                yield return new WaitForSeconds(next);
            }
        }
        if(isRunning){
            if(hasNextWave()){
                    _wavePrefix = "Next Wave incoming: ";
                    isBuildingPhase = true;
                    WaveStartTime = next;
                    yield return new WaitForSeconds(next);

                if(Wave == waveID+1){
                    startSpawningWaves();
                }
            }else{
                if(EnemiesAlive == 0){
                    Win();
                }
            }
        }
    }
    public void Win(){
        isBuildingPhase = true;
        isRunning = false;
        this.WaveUITime.text = "All waves cleared!";
        LevelManager.Instance.Win();
    }
}