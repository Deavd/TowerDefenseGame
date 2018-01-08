using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : StatManager {

	public Stat Speed;
	public Stat Damage;
	//public Stat Health;
	public Stat MaxHealth;
	public Stat Reward;
	public Stat Health;
	protected override void Awake () {
		Health.BaseValue = MaxHealth.Value;
		Speed.ValueChanged += this.onValueChanged;
		Health.ValueChanged += this.onValueChanged;
		MaxHealth.ValueChanged += this.onValueChanged;
		
		addStat(StatType.MaxHealth, MaxHealth);
		addStat(StatType.Health, Health);
		addStat(StatType.Speed, Speed);
		addStat(StatType.Damage, Damage);
		addStat(StatType.Reward, Reward);
		base.Awake();
	}

}
