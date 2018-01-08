using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyStat : StatManager {

	public Stat Speed;
	public Stat Damage;
	public Stat Health;
	public Stat MaxHealth;
	public Stat Reward;
	protected override void Awake () {
		Speed.Type = StatType.Speed;
		Damage.Type = StatType.Damage;
		Health.Type = StatType.Health;
		MaxHealth.Type = StatType.MaxHealth;
		Reward.Type = StatType.Reward;
		Speed.ValueChanged += this.onValueChanged;
		Health.ValueChanged += this.onValueChanged;
		addStat(StatType.MaxHealth, MaxHealth);
		addStat(StatType.Health, Health);
		addStat(StatType.Speed, Speed);
		addStat(StatType.Damage, Damage);
		addStat(StatType.Reward, Reward);
		base.Awake();
	}

}
