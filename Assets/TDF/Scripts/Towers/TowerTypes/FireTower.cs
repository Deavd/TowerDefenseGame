using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : AttackTower {
    public ParticleSystem system;
	public override bool AttackEnemy()
    {
        if(Target == null){return false;}
        GameObject missile = (GameObject) Instantiate(Missile, transform.GetChild(0).GetChild(0).position, this.transform.rotation);
        
        if(HasEffect){
            Effect.Value = Stats.getStat(StatType.Effect).Value;
            missile.GetComponent<Missiles>().addEffect(Effect);
        }
        missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value, Origin);
        return true;
    }
}
