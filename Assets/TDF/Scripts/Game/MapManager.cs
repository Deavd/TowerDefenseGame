using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [System.Serializable]
public class MapManager : MonoBehaviour {
	private static MapManager _instance;
	public static MapManager Instance
	{
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<MapManager>() : _instance) == null ? new GameObject().AddComponent<MapManager>(): _instance;
        }
	}
	//spawnpoints[]
	//size x,y
	public int x,z,difficulty;
	public Vector3 groundBoundSize;

	public float offset = 0.0f;
	private GameObject groundObject;
	public MapObject[,] Map;
	public void createMap(int x, int z, int difficulty, GameObject ground){
		this.x = x;
		this.z = z;
		this.difficulty = difficulty;
		this.groundObject = ground;
		this.groundBoundSize = ground.GetComponent<Renderer>().bounds.size;
		Map = new MapObject[x,z];
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
				MapObject mapObj = ground.GetComponent<MapObject>();
				mapObj.posX = i;
				mapObj.posZ = i2;
				Map[i,i2] = mapObj;
			}
		}
	}

	bool[,] goneThrough;
	bool top = false;
	bool bottom = false;
	int cycles = 0;
	public bool checkObstruction(int x, int z){
		top = false;
		bottom = false;
		goneThrough = new bool[this.x,this.z]; 
		return Obstruct(x,z);
	}
	private bool Obstruct(int x, int z){
		if(bottom&&top){
			return true;
		}
			//reached top of the map
			if(z == this.z-1){
				top = true;
			}else{
				//get the row on z+1
				int i = z+1;
				//go through the 3 fields on top that could block the way
				for(int n = -1; n <= 1; n++){
					//if its out of the map continue
					if(x+n < 0 || x+n >= this.x){
						continue;
					}
					//otherwise check if its buildable and not goneThrough already
					if(!Map[x+n,i].isBuildable && !goneThrough[x+n,i]){
						//if its buildable mark with goneTHrough
						goneThrough[x+n,i] = true;
						//repeat for the next block
						if(Obstruct(x+n,i)){
							return true;
						}
					}
				}
			}
			//reached bottom of the map
			if(z == 0){
				bottom = true;
			}else{
				int i = z-1;
				//same as above, just checking the 3 blocks under
				for(int n = -1; n <= 1; n++){
					if(x+n < 0 || x+n >= this.x){
						continue;
					}
					if(!Map[x+n,i].isBuildable && !goneThrough[x+n,i]){
						goneThrough[x+n,i] = true;
						if(Obstruct(x+n,i)){
							return true;
						}
					}
				}
			}
			//and also the block on the side of the building field
			if(x+1 < this.x){
				if(!Map[x+1,z].isBuildable && !goneThrough[x+1,z]){
					goneThrough[x+1,z] = true;
					if(Obstruct(x+1,z)){
							return true;
					}
				}
			}
			if(x-1 > 0){
				if(!Map[x-1,z].isBuildable && !goneThrough[x-1,z]){
					goneThrough[x-1,z] = true;
					if(Obstruct(x-1,z)){
						return true;
					}
				}
			}
		return bottom&&top;
	}

}
