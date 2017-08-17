using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

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
    public GameObject[] enemies;
    public GameObject[] spawnpoints;
    int difficulty = 1;
    int wave = 0;

      bool canSpawn = true;
     private float nextSpawn = 0.0f;
    public float period = 1.1f;
    void Update(){
        if (Time.time > nextSpawn ) {
            nextSpawn = Time.time + period;
        }

    }
    public void startSpawningWaves()
    {
        wave++;
        spawnRandomWave(wave, difficulty);

    }
    private void spawnRandomWave(int wave, int difficulty)
    {
        String s = "0:10:1 0:10:2 0:100:0.2";

        StartCoroutine("SpawnWave", s);
    }
    private int calculateAmount(double wave, double difficulty)
    {
        double x = (wave * 2 - (4 - difficulty)) * (difficulty) / 10;
        double y = 1;

        int z = Convert.ToInt32((x / y + 1) * wave * 1.8);
        return z;
    }
        bool isRunning = false;    
    void Upadte(){

    }
    float timeToStart = 2;
    float delayToNextPartWave = 0.5f;
    float timeToNextPartWave = 0;
    IEnumerator SpawnWave(String s){
        yield return new WaitForSeconds(timeToStart);
        Debug.Log("Spawning Wave");
        isRunning = true;
        //String format: id:amount:period id:amount:period
        String[] partWaves = s.Split();
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
                Debug.Log("Spawning Enemy with "+period + "delay");
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
