using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierAddType{
	PERCENTAGE_BASE,
	VALUE_BASE,
	PERCENTAGE_MODIFIED,
	VALUE_MODIFIED
}
[System.Serializable]
public class StatModifier{
	public string EffectName;
	public string EffectNameVerb;
	public float Time;
	public float Period;
	public bool hasTime = false;
	public bool hasPeriod = false;
	public float Value;
	public StatType statType;

	public StatModifierAddType modifierAddType;
	public StatModifier(StatModifierAddType addType, float value, StatType statType)
	{
		this.modifierAddType = addType;
		this.Value = value;
		this.statType = statType;
	}
	public StatModifier withTime(float time)
	{
		this.Time = time;
		this.hasTime = true;
		return this;
	}
	public StatModifier withPeriod(float period)
	{
		this.hasPeriod = true;
		this.Period = period;
		return this;
	}
}
