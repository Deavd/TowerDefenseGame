using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat{
	public bool Display = true;
	public Color Color;
	public string Name;
	public StatType Type;
	public string Description;
	public float BaseValue;
	private float _value;
	public float Value{
		set{
			Value = _value;
		}
		get{
			return BaseValue * BaseFactor + (_value + BaseValue) * Factor;
		}
	}
	public float Factor = 0f;
	public float BaseFactor = 1f;

	public List<StatModifier> Modifiers = new List<StatModifier>();
	[SerializeField]
	public List<StatModifier> ModifiersWithExpiryTime = new List<StatModifier>();

	// Use this for initialization
	public Stat (string name, StatType type)
	{
		this.Name = name;
		this.Type = type;
		this.BaseValue = this._value = 0f;
	}
	public Stat WithColor(Color c) {
		this.Color = c;
		return this;
	}
	public Stat WithDescription(string desc)
	{
		this.Description = desc;
		return this;
	}
	public Stat WithValue(float value)
	{
		this.BaseValue = value;
		return this;
	}

	public void AddBase(float value){
		BaseValue += value;
	}
	public void AddModifier(StatModifier mod)
	{
		Modifiers.Add(mod);
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				//percentage 0-1
				this.Value += mod.Value * this.BaseValue;
				break;
			case StatModifierAddType.VALUE_BASE_PERMANENT:
				this.BaseValue += mod.Value;
				break;
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				this.Value += mod.Value * this.Value;
				break;
			case StatModifierAddType.VALUE_MODIFIED:
				this.Value += mod.Value;
				break;
		}
		if(mod.hasTime)
		{
			ModifiersWithExpiryTime.Add(mod);
		}			
		
	}
	public void RemoveModifier(StatModifier mod)
	{		
		Modifiers.Remove(mod);
		ModifiersWithExpiryTime.Remove(mod);
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				//percentage 0-1
				this.BaseFactor -= mod.Value;
				break;
			case StatModifierAddType.VALUE_BASE_PERMANENT:
				this.BaseValue -= mod.Value;
				break;
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				this.Factor -= mod.Value;
				break;
			case StatModifierAddType.VALUE_MODIFIED:
				this.Value -= mod.Value;
				break;
		}
	}
	public void RemoveAllModifiers()
	{
		Modifiers.Clear();
		ModifiersWithExpiryTime.Clear();
	}
}
public enum StatType { 
	//PLAYER
	Health, 
	Money,
	//ENDPLAYER
	//TOWER
	Damage, 
	AttackSpeed, 
	Range,
	BuildTime,
	BuyPrice,
	SellPrice,
	//ENDTOWER
	//ENEMY
	Speed, 
	MaxHealth,
	Reward
	//ENDENEMY
}