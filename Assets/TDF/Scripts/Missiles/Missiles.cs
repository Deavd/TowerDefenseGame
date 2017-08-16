using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour {
	private GameObject target;
	public float accuracy;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//wenn das Ziel nicht existiert, zerstöre das Objekt
		if(target == null){
			Destroy(this.gameObject);
			return;
		}
		float distance = (target.transform.position - this.transform.position).magnitude;
		//Wenn die Distanz Zwischen Objekt und Ziel kleiner als die Strecke ist, die das Objekt zurücklegt, dann zerstöre es
		if(distance <= speed * Time.deltaTime){
			HitEnemy();
			Destroy(this.gameObject);
			return;
		}
		//Bewege das Objekt in Richtung des Ziels
		this.transform.Translate(Time.deltaTime * speed * target.transform.position - this.transform.position);
	}
}
