using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyStat)), RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour, StatHolder{

	// Use this for initialization
	private EnemyStat _stats;
	public EnemyStat Stats {
		get{
			return _stats == null ? Stats = GetComponent<EnemyStat>() : _stats;
		}
		set{
			_stats = value;
		}
	}
	private NavMeshAgent _agent;
	protected virtual void Awake () {		
		this.tag = "Enemy";
		
		_agent = this.GetComponent<NavMeshAgent>();
		_HealthBarImage.transform.parent.transform.SetParent(LevelManager.Instance.GameUI.transform, false);
		_HealthBarImage.transform.parent.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
	
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
	public void ReceiveDamage(float dmg){
		Stats.getStat(StatType.Health).AddBase(-dmg);
		if(Stats.getStat(StatType.Health).Value <= 0){
			Die();
		}
		
		//damage animation
	}
	public void Heal(float amount){
		ReceiveDamage(-amount);
	}
	public void Die(){
		WaveManager.EnemiesAlive--;
		Destroy(this.gameObject);
		Destroy(_healthBarImage.transform.parent.gameObject);
		LevelManager.Instance.Money += Stats.getStat(StatType.Reward).Value;
		//die animation
	}
	public void Destinated(){
		WaveManager.EnemiesAlive--;
		Destroy(this.gameObject);
		Destroy(_healthBarImage.transform.parent.gameObject);
		LevelManager.Instance.Lifes -= (int)Stats.getStat(StatType.Damage).Value;
	}

    void StatHolder.OnStatChanged(Stat t)
    {
		//Debug.Log("Handling event!");
		//Debug.Log("Stattype is: " + t.Type + ";" +" Statvalue is: "+t.Value);
        switch(t.Type){
			case StatType.Health:
				//Debug.Log(_HealthBarImage.fillAmount);
				//Debug.Log(Stats.getStat(StatType.Health).Value);
				_HealthBarImage.fillAmount = t.Value/Stats.getStat(StatType.MaxHealth).Value;
				break;
			case StatType.Speed:
				_agent.speed = t.Value;
				break;
		}
    }
}
