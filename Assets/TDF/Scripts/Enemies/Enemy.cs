using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStat)), RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, StatHolder{

	private EnemyStat _stats;
	public EnemyStat Stats {
		get{
			return _stats == null ? Stats = GetComponent<EnemyStat>() : _stats;
		}
		set{
			_stats = value;
		}
	}
	private NavMeshAgent __agent;
	private NavMeshAgent _agent{
		get{
			return __agent == null ? __agent = this.GetComponent<NavMeshAgent>(): __agent;
		}
	}
	protected virtual void Awake () {
		this.tag = "Enemy";
		
		_HealthBarImage.transform.parent.transform.SetParent(LevelManager.Instance.GameUI.transform, false);
		_HealthBarImage.transform.parent.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
		_agent.speed = Stats.Speed.Value;
	}
	public GameObject DeathEffect;
	private Image _healthBarImage;
	private Image _HealthBarImage{
		set{
			_healthBarImage = value;
		}
		get{
			return _healthBarImage == null ? _healthBarImage = Instantiate(GameObject.Find("Healthbar")).transform.GetChild(0).GetComponent<Image>() : _healthBarImage;
		}
	}

	// Update is called once per frame
	protected virtual void Update () {
		_healthBarImage.transform.parent.position = Camera.main.WorldToScreenPoint(this.transform.position+Vector3.up);
	}
	public Origin Origin;
	public float getOriginDamage(Origin damageOrigin){
		if(damageOrigin == Origin.NORMAL || Origin == Origin.NORMAL){
			return 1;
		}
		if(damageOrigin == Origin){
			return 0.5f;
		}
		return 2f;
	}
	public void ReceiveDamage(float dmg, Origin origin = Origin.NORMAL){
		dmg = getOriginDamage(origin)*dmg;
		Stats.getStat(StatType.Health).AddBase(-dmg);
		//damage animation
	}
	public void Heal(float amount){
		ReceiveDamage(-amount);
	}
	public void Die(){
		SoundManager.Instance.PlaySound("deathSound");
		GameObject effect  = Instantiate(DeathEffect, transform.position, Quaternion.identity);
		WaveManager.EnemiesAlive--;
		Destroy(this.gameObject);
		Destroy(effect, 2f);
		Destroy(_healthBarImage.transform.parent.gameObject);
		LevelManager.Instance.Money += Stats.getStat(StatType.Reward).Value;
	}
	public void Destinated(){
		WaveManager.EnemiesAlive--;
		Destroy(this.gameObject);
		Destroy(_healthBarImage.transform.parent.gameObject);
		LevelManager.Instance.Lifes -= (int)Stats.getStat(StatType.Damage).Value;
	}

	void StatHolder.OnStatChanged(Stat t)
	{
		switch(t.Type){
			case StatType.MaxHealth:
				Stats.Health.BaseValue = t.Value;
				break;
			case StatType.Health:
				_HealthBarImage.fillAmount = t.Value/Stats.getStat(StatType.MaxHealth).Value;
				if(Stats.getStat(StatType.Health).Value <= 0){
					Die();
				}
				break;
			case StatType.Speed:
				_agent.speed = t.Value;
				break;
		}
	}
}
public enum Origin{
	NORMAL,
	FIRE_LAND,
	ICE_LAND

}