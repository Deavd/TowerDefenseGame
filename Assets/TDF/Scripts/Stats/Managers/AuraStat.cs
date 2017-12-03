using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraStat : StatManager {
	public Stat[] AuraStats;
	protected override void Awake () {
		foreach(Stat stat in AuraStats){			
			addStat(stat.Type, stat);
		}
		base.Awake();
	}
}
