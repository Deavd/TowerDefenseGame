using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    public GameObject mapGroundObject; //prefab
    public int mapSizeX, mapSizeZ, levelDifficulty;  //predefinded

    void Start()
    {
        startLevel();
    }

    private void startLevel()
    {
        LevelManager.Instance.StartLevel(mapSizeX,mapSizeZ,levelDifficulty,mapGroundObject);
    }
}
