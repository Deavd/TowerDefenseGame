using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MachineGunTower : AttackTower
{
    int current = 0;
    public override bool AttackEnemy(){
        if(Target == null){return false;}
        Transform spawnPoints = this.transform.GetChild(0).GetChild(0);
        int i = spawnPoints.childCount;
        if(current >= i){
            current = 0;
        }
        GameObject missile = (GameObject) Instantiate(Missile, spawnPoints.GetChild(current).position, this.transform.GetChild(0).rotation);
        //missile.GetComponent<Missiles>().Shoot(Target, Damage);    
        missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value, Origin);    
        current++;
        return true;
    }
    
}