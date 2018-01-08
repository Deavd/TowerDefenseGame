using System;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public void EndGame(){
		#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
         #else
         Application.Quit();
         #endif
    }
    public void RestartGame(){
        SceneManager.LoadScene(0);	
    }
}
