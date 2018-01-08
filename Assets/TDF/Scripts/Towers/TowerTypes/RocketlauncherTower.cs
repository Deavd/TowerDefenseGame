﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketlauncherTower : AttackTower {

    public override bool AttackEnemy(){
        //differenziert sich kaum von der ursprünglichen Funktion
        //schiesst 2 Raketen statt eine
        if(Target == null){return false;}
        Transform spawnPoints = this.transform.GetChild(0).GetChild(0);
        GameObject missile = (GameObject) Instantiate(Missile, spawnPoints.GetChild(0).position, this.transform.GetChild(0).rotation);
		GameObject missile2 = (GameObject) Instantiate(Missile, spawnPoints.GetChild(1).position, this.transform.GetChild(0).rotation);

		missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value/2, Origin);   
		missile2.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value/2, Origin);       
        return true;
    }
}