using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitStart();
	}
	public void InitStart () {
		this.GetComponent<NavMeshAgent>().speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public float health = 100;
	public float speed = 10;
	public float reward = 100;

	public int damage = 1;
	public void ReceiveDamage(float dmg){
		health -= dmg;
		if(health <= 0){
			Die();
		}
		//damage animation
	}
	public void Heal(float amount){
		ReceiveDamage(-amount);
	}
	public void Die(){
		Destroy(this.gameObject);
		LevelHandler.Instance.Money += reward;
		//die animation
	}
	public void Destinated(){
		Destroy(this.gameObject);
		LevelHandler.Instance.Lifes -= damage;
	}
}
