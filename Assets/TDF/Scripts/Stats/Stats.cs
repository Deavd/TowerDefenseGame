using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour{
	public Color StatColor {set;get;}
	public string Name {set;get;}
	public string Description {set;get;}
	float Value {set;get;}

	public enum StatType { Health, Damage, AttackSpeed, Speed, Range, Level, Cost, Value }
	// Use this for initialization
	public Stat (string Name, float Value, string Description) {
		
	}
	public Stat (string Name, float Value, string Description, Color c) {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
