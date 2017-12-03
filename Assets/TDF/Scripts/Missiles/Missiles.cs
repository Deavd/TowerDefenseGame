using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour {
	public  GameObject target;
	public GameObject effect;
	public float damage;
	public float accuracy;
	public float speed;
	private bool _hasEffect = false;
	private Stat _effectStat;
	private StatModifier _statModifier;

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
		if(_hasEffect){
			e.Stats.addTimeModifierToStat( _statModifier);
			/*switch(_effectType){
				case EffectTypes.SLOW:
					//Debug.Log("ADDING SLOW EFFECT!");
					//Debug.Log("CURRENT HEALTH: "+ e.Stats.getStat(StatType.Health).Value);
					e.Stats.addModifierToStat(_effectType new StatModifier(StatModifierAddType.PERCENTAGE_MODIFIED, _effectStat.Value).withTime(1f));
					break;
				case EffectTypes.DAMAGE_OVER_TIME:
					//Debug.Log("ADDING DAMAGE OVER TIME EFFECT!");
					e.Stats.addModifierToStat(StatType.Speed, new StatModifier(StatModifierAddType.VALUE_MODIFIED, _effectStat.Value).withTime(2f).withPeriod(0.1f));
					break;
			}*/
		}

        e.ReceiveDamage(damage);
	}
	public void addEffect(StatModifier mod){
		_hasEffect = true;
		_statModifier = mod;
	}
}
public enum EffectTypes{
	SLOW,
	DAMAGE_OVER_TIME
}