using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowingMissile : Missiles {
	public override void OnEnemyHit(bool hasTarget){
		if(!hasTarget){
			return;
		}
		Enemy e = target.GetComponent<Enemy>();
        e.ReceiveDamage(damage);
	}
}
