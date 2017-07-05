using UnityEngine;
public class GameHandler : MonoBehaviour
{

    public GameObject mapGroundObject;
    public int mapSizeX, mapSizeZ, levelDifficulty;
    void Start()
    {
        initInstances();
    }
    private void initInstances()
    {
        //maybe goes into the level part
        LevelHandler.Instance.StartLevel(mapSizeX, mapSizeZ, levelDifficulty, mapGroundObject);

    }

}
