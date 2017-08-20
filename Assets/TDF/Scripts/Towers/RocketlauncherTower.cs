using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketlauncherTower : Tower {
    public override void AttackEnemy(){
        if(target == null){return;}
        Transform spawnPoints = this.transform.GetChild(0).GetChild(0);
        GameObject missile = (GameObject) Instantiate(Missile, spawnPoints.GetChild(0).position, this.transform.GetChild(0).rotation);
		GameObject missile2 = (GameObject) Instantiate(Missile, spawnPoints.GetChild(1).position, this.transform.GetChild(0).rotation);
        missile.GetComponent<Missiles>().Shoot(target, Damage/2);    
		missile2.GetComponent<Missiles>().Shoot(target, Damage/2);   

    }
}
