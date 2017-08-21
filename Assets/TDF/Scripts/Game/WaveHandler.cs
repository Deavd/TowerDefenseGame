using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class WaveHandler : MonoBehaviour
{
    #region Instance
    public static WaveHandler Instance
    {
        get
        {
            return (WaveHandler)FindObjectOfType(typeof(WaveHandler));
        }
    }
    #endregion
    //enemy objects
    public GameObject homeBase;
    public Text WaveUIWave;
    public Text WaveUITime;
    public GameObject[] enemies;
    public GameObject[] spawnpoints;
    int difficulty = 1;
    int wave = 0;

      bool canSpawn = true;
     private float nextSpawn = 0.0f;
    public float period = 1.1f;
    
    public void Start(){
    }
    public void startSpawningWaves()
    {
        
        wave++;
        spawnRandomWave(wave, difficulty);

    }
    private void spawnRandomWave(int wave, int difficulty)
    {
        time = 0;
        String s = "0:60:0.3";

        StartCoroutine("SpawnWave", s);
    }
    private int calculateAmount(double wave, double difficulty)
    {
        double x = (wave * 2 - (4 - difficulty)) * (difficulty) / 10;
        double y = 1;

        int z = Convert.ToInt32((x / y + 1) * wave * 1.8);
        return z;
    }
    public bool isRunning = false;    
    void Awake(){
        time+=timeToStart;
    }
    void Update(){
        Debug.Log(time);
        time -= Time.deltaTime;
        if(!isRunning && time <= 0){
            startSpawningWaves();
            isRunning = true;
        }
    }
    float timeToStart = 2;
    float delayToNextPartWave = 0.5f;
    float timeToNextPartWave = 0;
    float _time;
    public float time {get{
        return _time;
    }set{
        _time = value;
        this.WaveUITime.text = value+"";       
    }}
    IEnumerator SpawnWave(String s){
        String[] partWaves = s.Split();
        WaveUIWave.text = wave + "/" + "-";
        time = 0f;
        foreach(String partWave in partWaves){
            float amount;
            float period;
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            float.TryParse(parts[1], out amount);
            float.TryParse(parts[2], out period);
            time += amount * period;
        }
        
        WaveUITime.text = (time /60).ToString();
        //String format: id:amount:period id:amount:period
        //yield return new WaitForSeconds(timeToStart);
        Debug.Log("Spawning Wave");
        foreach(String partWave in partWaves){
            Debug.Log("Spawning PartWave");
            String[] parts = partWave.Split(":".ToCharArray()[0]);
            int id;
            int.TryParse(parts[0], out id);
            int amount;
            int.TryParse(parts[1], out amount);
            float period;
            float.TryParse(parts[2], out period);
            for(int i = 0; i < amount; i++){
                GameObject spawnedEnemy = Instantiate(enemies[id], spawnpoints[0].transform.position, Quaternion.identity);
                spawnedEnemy.GetComponent<NavMeshAgent>().SetDestination(homeBase.transform.position);
                //Debug.Log("Spawning Enemy with "+period + "delay");
                yield return new WaitForSeconds(period);
            }
            yield return new WaitForSeconds(delayToNextPartWave);
        }
        isRunning = false;
    }

}
public class Wave : MonoBehaviour
{

}
