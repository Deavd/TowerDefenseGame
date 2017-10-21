using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    public GameObject mapGroundObject; //prefab
    public int mapSizeX, mapSizeZ, levelDifficulty;  //predefinded

    private int selectedLevel;
    void Start()
    {
        startLevel(selectedLevel);
    }

    private void startLevel(int selectedLevel)
    {
        LevelManager.Instance.StartLevel(mapSizeX,mapSizeZ,levelDifficulty,mapGroundObject);
    }
}
