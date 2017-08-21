using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStats : MonoBehaviour {
	List<Stat> towerStats = new List<Stat>();
	// Use this for initialization
	void Start () {
		towerStats.Add(new Stat("Health", 100, "", Color.green));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
