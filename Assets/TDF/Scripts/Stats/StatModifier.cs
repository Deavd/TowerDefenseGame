using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierAddType{
	PERCENTAGE_BASE,
	VALUE_BASE_PERMANENT,
	PERCENTAGE_MODIFIED,
	VALUE_MODIFIED
}
[System.Serializable]
public class StatModifier{
	public float Time;
	public float Period;
	public bool hasTime = false;
	public bool hasPeriod = false;
	public float Value;
	public bool isActive = false;
	public float activatedTime; 
	public float stackedTime = 0f;
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
		this.isActive = true;
		return this;
	}
	public StatModifier withPeriod(float period)
	{
		this.hasPeriod = true;
		this.Period = period;
		this.isActive = true;
		return this;
	}
}
