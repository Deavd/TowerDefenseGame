using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MachineGunTower : Tower
{

    public override void AttackEnemy(){
        if(target == null){return;}
        Transform spawnPoints = this.transform.GetChild(0).GetChild(1);
        int i = spawnPoints.childCount;
        GameObject missile = (GameObject) Instantiate(Missile, spawnPoints.GetChild(Random.Range(0,i)).position, this.transform.GetChild(0).rotation);
        missile.GetComponent<Missiles>().Shoot(target, Damage);    

    }
}