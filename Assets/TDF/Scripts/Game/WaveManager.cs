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
    int Wave = 0;
    public bool isRunning = true;  
    public bool isBuildingPhase = true;  
    public void startSpawningWaves()
    {
        isRunning = true;
        isBuildingPhase = false;
        //wave: "id:amount:period :::"
        if(Waves.Length > Wave){
            spawnWave(Waves[Wave]);
        }else{
            isRunning = false;
            Debug.Log("you won");
        }
    }
    private void spawnWave(string wave)
    {
        StartCoroutine("SpawnWave", wave);
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
    float timeToStart = 2;
    float delayToNextPartWave = 5.5f;
    float _time = 0f;
    string WavePrefix;
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
                this.WaveUITime.text = WavePrefix+_time.ToString("0.0");
            }
        }
    }
    IEnumerator SpawnWave(String s)
    {
        WavePrefix = "Wave ends in: ";
        String[] partWaves = s.Split();
        WaveUI.text = Wave + "/" + "-";
        float timeNeeded = 0f;;

        int id;
        float next = 0f;
        int amount;
        float period;
        foreach(String partWave in partWaves)
        {
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            int.TryParse(parts[1], out amount);
            float.TryParse(parts[2], out period);
            float.TryParse(parts[3], out next);
            timeNeeded += amount * period + (partWaves.Length - 1) * delayToNextPartWave;
        }
        WaveStartTime = timeNeeded;
        //WaveUITime.text = (WaveStartTime /60).ToString();
        //String format: id:amount:period:next id:amount:period
        //yield return new WaitForSeconds(timeToStart);
        Debug.Log("Spawning Wave");
        for(int j = 0; j < partWaves.Length; j++)
        {
            String partWave = partWaves[j];
            Debug.Log("Spawning PartWave");
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            int.TryParse(parts[0], out id);
            int.TryParse(parts[1], out amount);
            float.TryParse(parts[2], out period);
            float.TryParse(parts[3], out next);
            for(int i = 0; i < amount; i++)
            {
                GameObject spawnedEnemy = Instantiate(enemies[id], spawnpoints[0].transform.position, Quaternion.identity);
                spawnedEnemy.GetComponent<NavMeshAgent>().SetDestination(homeBase.transform.position);
                if(i == amount-1){
                    yield return null;
                } 
                //Debug.Log("Spawning Enemy with "+period + "delay");
                yield return new WaitForSeconds(period);
            }
            if(j == partWaves.Length-1){
                Debug.Log("Last Wave.. start with Buildingtime");
                yield return null;
            }else{
                Debug.Log("Another Wave.. spawn that!");
                yield return new WaitForSeconds(next);
            }
        }
        WavePrefix = "Next Wave incoming: ";
        Debug.Log("Enter Buildingtime!");
        Wave++;
        isBuildingPhase = true;
        WaveStartTime = next;
        yield return new WaitForSeconds(next);
        startSpawningWaves();
    }

}