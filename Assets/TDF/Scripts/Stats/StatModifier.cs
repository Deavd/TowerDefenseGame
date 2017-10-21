using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierAddType{
	PERCENTAGE_BASE,
	VALUE_BASE_PERMANENT,
	PERCENTAGE_MODIFIED,
	VALUE_MODIFIED
}
public class StatModifier{
	public float Time{set;get;}
	public bool hasTime{
		get{
			return Time != 0;
		}
	}
	public float Value{set;get;}
	public StatModifierAddType modifierAddType;
	public StatModifier(StatModifierAddType addType, float value)
	{
		this.modifierAddType = addType;
		this.Value = value;
	}
	public StatModifier withTime(float time)
	{
		this.Time = time;
		return this;
	}
}
