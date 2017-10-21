using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStat))]
public class Enemy : MonoBehaviour{

	// Use this for initialization
	public EnemyStat Stats;
	protected virtual void Awake () {
		Stats = GetComponent<EnemyStat>();
	}
	protected virtual void Start () {
		InitStart();
	}
	public GameObject healthBar;
	private Image healthBarImage;
	public void InitStart () {
		this.GetComponent<NavMeshAgent>().speed = Stats.Speed.Value;
		healthBar = Instantiate(LevelManager.Instance.healthBar);
		healthBarImage = healthBar.transform.GetChild(0).GetComponent<Image>();

		healthBar.transform.SetParent(LevelManager.Instance.GameUI.transform, false);
		healthBar.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
		Stats.Health.BaseValue = Stats.MaxHealth.Value;
	}
	
	// Update is called once per frame
	public void OnUpdate () {
		healthBar.transform.position = LevelManager.mainCamera.WorldToScreenPoint(this.transform.position+Vector3.up);
	}
	public void ReceiveDamage(float dmg){
		Stats.Health.AddBase(-dmg);
		if(Stats.Health.Value <= 0){
			Die();
		}
		healthBarImage.fillAmount = Stats.Health.Value/Stats.MaxHealth.Value;
		//damage animation
	}
	public void Heal(float amount){
		ReceiveDamage(-amount);
	}
	public void Die(){
		Destroy(this.gameObject);
		Destroy(healthBar);
		LevelManager.Instance.Money += Stats.Reward.Value;
		//die animation
	}
	public void Destinated(){
		Destroy(this.gameObject);
		Destroy(healthBar);
		LevelManager.Instance.Lifes -= (int)Stats.Damage.Value;
	}
}
