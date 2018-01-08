using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerStat : StatManager {
	public Stat AttackSpeed;
	public Stat Damage;
	public Stat Range;
	public Stat BuyPrice;
	public Stat SellPrice;
	public Stat BuildTime;
	public Stat Effect;
	protected override void Awake () {
		loadStats();
		base.Awake();
	}
	public void loadStats(){
			addStat(StatType.AttackSpeed, AttackSpeed);
			addStat(StatType.Damage, Damage);
			addStat(StatType.Range, Range);
			addStat(StatType.BuyPrice, BuyPrice);
			addStat(StatType.SellPrice, SellPrice);
			addStat(StatType.BuildTime, BuildTime);
			addStat(StatType.Effect, Effect);
		}
	
}
