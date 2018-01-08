﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Missiles : MonoBehaviour {
	public  GameObject target;
	public GameObject ParticleEffect;
	public float damage;
	public float accuracy;
	public float speed;
	private bool _hasEffect = false;
	private Stat _effectStat;
	private StatModifier _statModifier;
	private AudioSource _shootSound;
	public Origin origin;
	private bool hasHit = false;
	//Wird aufgerufen wenn die Missile erstellt wird
	public void Shoot(GameObject target, float damage, Origin origin){
		//_shootSound.Play();
		this.damage = damage;
		this.target = target;
		this.origin = origin;
	}
	public void Awake(){
		this._shootSound = GetComponent<AudioSource>();
		this._mesh = GetComponentInChildren<MeshRenderer>();
	}
	
	public Vector3 lastPos;
	MeshRenderer _mesh;
	void Update () {
		if(hasHit){
			return;
		}
		float distance;
		//wenn das Ziel nicht existiert, zerstöre das Objekt
		Vector3 targetPos = target == null ? lastPos : target.transform.position;
		if(targetPos == Vector3.zero){
			Destroy(this.gameObject);
			return;
		}
		distance = (targetPos - this.transform.position).sqrMagnitude;
		//wenn die Distanz Zwischen Objekt und Ziel kleiner als die Strecke ist, die das Objekt zurücklegt, dann zerstöre es
		if(distance <= speed * speed * Time.deltaTime * Time.deltaTime){
			hasHit = true;
			_shootSound.Play();
			if(_mesh != null){
				_mesh.enabled = false;
			}
			OnEnemyHit(target != null);
			Destroy(this.gameObject, 1);
			return;
		}
		this.transform.LookAt(targetPos);
		//bewege das Objekt in Richtung des Ziels
		this.transform.Translate(Time.deltaTime * speed * (targetPos - this.transform.position).normalized, Space.World);
		lastPos = targetPos;
	}
	//wenn der Gegner getroffen wurde, wird dieses Event aufgerufen
	public virtual void  OnEnemyHit(bool hasTarget){
		if(!hasTarget){
			return;
		}		
		PlayParticleEffect(hasTarget);
		Enemy e = target.GetComponent<Enemy>();
		if(_hasEffect){
			//falls ein Effekt vorhanden ist, übergebe diesen an den Gegner
			e.Stats.addTimeModifierToStat(_statModifier);
		}
        e.ReceiveDamage(damage, origin);
	}
	public  void PlayParticleEffect(bool hasTarget){
		//spielt ein Impact-Effekt ab
		//setzte Impact-Position
		Vector3 impactPos = hasTarget ? target.transform.position : lastPos;
		impactPos.y += 0.25f;
		GameObject impacteffect = Instantiate(ParticleEffect, impactPos, Quaternion.Euler(90, 0, 0));
		Destroy(impacteffect,0.4f);
	}
	public void addEffect(StatModifier mod){
		//Effekt der übergeben wird an das Geschoss, der später dem Gegner übergeben wird
		_hasEffect = true;
		_statModifier = mod;
	}
}
public enum EffectTypes{
	SLOW,
	DAMAGE_OVER_TIME
}