using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour {
	public static MapCreator Instance
	{
		get
		{
			return (MapCreator) FindObjectOfType(typeof(MapCreator));
		}
	}
	//spawnpoints[]
	//size x,y
	public int x,z,difficulty;
	public Vector3 groundBoundSize;

	public float offset = 0.0f;
	List<MapObject> mapObjects = new List<MapObject>();
	private GameObject groundObject;
	public void createMap(int x, int z, int difficulty, GameObject ground){
		this.x = x;
		this.z = z;
		this.difficulty = difficulty;
		this.groundObject = ground;
		this.groundBoundSize = ground.GetComponent<Renderer>().bounds.size;
		buildMap();
	}

	public Vector3 getPosition(int x, int z){
		return new Vector3(x*(groundBoundSize.x+offset), 0, z*(groundBoundSize.z+offset));
	}
	private void buildMap(){
		for(int i = 0; i < x; i++){
			for(int i2 = 0; i2 < z; i2++){	
				GameObject ground = Instantiate(groundObject);
				ground.transform.position = getPosition(i,i2);
				ground.transform.parent = this.gameObject.transform;
				ground.GetComponent<MapObject>().posX = i;
				ground.GetComponent<MapObject>().posZ = i2;
				//MapObject obj = new MapObject(i,i2,ground,MapObjectType.GROUND);
				//mapObjects.Add(obj);
			}
		}
	}
	//difficulty[]
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
