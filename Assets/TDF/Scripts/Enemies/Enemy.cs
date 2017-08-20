using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitStart();
	}
	public GameObject healthBar;
	public Image healthBarImage;
	public void InitStart () {
		this.GetComponent<NavMeshAgent>().speed = speed;
		healthBar = Instantiate(LevelHandler.healthBar);
		healthBarImage = healthBar.transform.GetChild(0).GetComponent<Image>();

		healthBar.transform.SetParent(LevelHandler.GameUI.transform, false);
		healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
		health = maxHealth;
	}
	
	// Update is called once per frame
	public void OnUpdate () {
		healthBar.transform.position = LevelHandler.mainCamera.WorldToScreenPoint(this.transform.position+Vector3.up);
	}
	public float maxHealth = 100;
	public float health;
	public float speed = 10;
	public float reward = 100;

	public int damage = 1;
	public void ReceiveDamage(float dmg){
		health -= dmg;
		if(health <= 0){
			Die();
		}
		healthBarImage.fillAmount = health/maxHealth;
		//damage animation
	}
	public void Heal(float amount){
		ReceiveDamage(-amount);
	}
	public void Die(){
		Destroy(this.gameObject);
		Destroy(healthBar);
		LevelHandler.Instance.Money += reward;
		//die animation
	}
	public void Destinated(){
		Destroy(this.gameObject);
		Destroy(healthBar);
		LevelHandler.Instance.Lifes -= damage;
	}
}
