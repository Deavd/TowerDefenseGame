using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMissile : Missiles {

	// Use this for initialization
	public override void OnEnemyHit(bool hasTarget){
		PlayParticleEffect(hasTarget);
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float rangeSqr = 6f;
        foreach (GameObject enemy in enemies){
			float distanceSqr = (enemy.transform.position - this.transform.position).sqrMagnitude;
			if(distanceSqr <= rangeSqr){
            	enemy.GetComponent<Enemy>().ReceiveDamage(damage, origin);
			}
        }
	}
}
