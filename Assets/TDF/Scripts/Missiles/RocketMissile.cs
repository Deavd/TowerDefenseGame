using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMissile : Missiles {

	// Use this for initialization
	public override void OnEnemyHit(bool hasTarget){
		Vector3 impactPos = hasTarget ? target.transform.position : lastPos;
		GameObject impacteffect =Instantiate(effect, impactPos, Quaternion.Euler(90, 0, 0));
		Destroy(impacteffect,0.4f);
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float rangeSqr = 7f;
        foreach (GameObject enemy in enemies){
			float distanceSqr = (enemy.transform.position - this.transform.position).sqrMagnitude;
			if(distanceSqr <= rangeSqr){
            	enemy.GetComponent<Enemy>().ReceiveDamage(damage);
			}
        }
	}
}
