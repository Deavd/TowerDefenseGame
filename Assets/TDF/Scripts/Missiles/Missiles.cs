using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour {
	public  GameObject target;
	public GameObject effect;
	public float damage;
	public float accuracy;
	public float speed;

	// Use this for initialization
	void Start () {
		
	}
	//Wird aufgerufen wenn die Missile erstellt wird
	public void Shoot(GameObject target, float damage){
		this.damage = damage;
		this.target = target;
	}
	
	public Vector3 lastPos;
	// Update is called once per frame
	void Update () {
		float distance;
		//wenn das Ziel nicht existiert, zerstöre das Objekt
		Vector3 targetPos = target == null ? lastPos : target.transform.position;
		if(targetPos == Vector3.zero){Destroy(this.gameObject);return;}
		distance = (targetPos - this.transform.position).sqrMagnitude;
		//Wenn die Distanz Zwischen Objekt und Ziel kleiner als die Strecke ist, die das Objekt zurücklegt, dann zerstöre es
		if(distance <= speed * speed * Time.deltaTime * Time.deltaTime){
			OnEnemyHit(target != null);
			Destroy(this.gameObject);
			return;
		}
		this.transform.LookAt(targetPos);
		//Bewege das Objekt in Richtung des Ziels
		lastPos = targetPos;
		this.transform.Translate(Time.deltaTime * speed * (targetPos - this.transform.position).normalized, Space.World);
	}
	public virtual void  OnEnemyHit(bool hasTarget){
		if(!hasTarget){
			return;
		}
		Enemy e = target.GetComponent<Enemy>();
        e.ReceiveDamage(damage);
	}
}
