using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTower : AttackTower {
	public StatModifier SlowEffect;
    public override bool AttackEnemy(){
        if(Target == null){return false;}
			Transform spawnPoints;
		if(transform.childCount == 0){
			spawnPoints = this.transform;
		}else{
			spawnPoints = this.transform.GetChild(0).GetChild(0);
		}
        SlowEffect.Value = Stats.getStat(StatType.Effect).Value;
        GameObject missile = (GameObject) Instantiate(Missile, spawnPoints.position, spawnPoints.rotation);
		missile.GetComponent<Missiles>().addEffect(SlowEffect);
		missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value);
		return true;
    }
}
