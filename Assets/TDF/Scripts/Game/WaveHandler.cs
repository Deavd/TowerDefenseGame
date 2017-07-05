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
    public void spawnWave()
    {
        wave++;
        calculateRandomWave(wave, difficulty);
        /*for(int x = 0; x < 100; x++){
            calculateRandomWave(x+1,1);
            calculateRandomWave(x+1,2);
            calculateRandomWave(x+1,3);
            Debug.Log("");
        }*/
    }
    private void calculateRandomWave(int wave, int difficulty)
    {
        int amount = calculateAmount(wave, difficulty);
        for(int i = 0; i < amount; i++){
            GameObject enemy = Instantiate(enemies[0], spawnpoints[0].transform.position, Quaternion.identity);
            enemy.GetComponent<NavMeshAgent>().SetDestination(homeBase.transform.position);
        }
        /*difficulty 
        1
        Wave 1
        difficulty 1    -> more loot
                        -> enemies have less health
                        -> enemies are dumb?*/
        Debug.Log(calculateAmount(wave, difficulty) + " [" + difficulty + ", " + wave + "]");

    }
    private int calculateAmount(double wave, double difficulty)
    {
        double x = (wave * 2 - (4 - difficulty)) * (difficulty) / 10;
        double y = 1;

        int z = Convert.ToInt32((x / y + 1) * wave * 1.8);
        return z;
    }

}
