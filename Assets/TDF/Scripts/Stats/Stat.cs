using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Stat{
	public bool Display = true;
	public string Name;
	public StatType Type;
	public string Description;
	public float BaseValue;
	private float _value;
	public float Value{
		set{
			_value = value;
		}
		get{
			if(!_init){
				Init();
			}
			return (_value + BaseValue * _baseFactor) * _factor;			
		}
	}
	private float _factor = 1f;
	private float _baseFactor = 1f;
	public  LevelScale LevelScale;
	private int _levelScaleLevel = 0;
	public float[] LevelScaleValues;
	public List<StatModifier> Modifiers  = new List<StatModifier>();
	// Use this for initialization
	public delegate void ValueChangedEventHandler(object source, EventArgs args);
	public event ValueChangedEventHandler ValueChanged;
	bool _init = false;
	public void Init(){
		_factor = 1;
		_baseFactor = 1;
		_levelScaleLevel = 0;
		_value = 0;
		OnValueChanged();
		_init = true;
	}
	protected virtual void OnValueChanged(){
		if(ValueChanged != null){
			ValueChanged(this, EventArgs.Empty);
		}
	}
	public Stat (string name, StatType type)
	{
		this.Name = name;
		this.Type = type;
		this.BaseValue = this._value = 0f;
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
	public bool CanLevelUP(){
		return LevelScaleValues.Length > _levelScaleLevel;
	}
	public bool LevelUP(){
		if(CanLevelUP()){
			this._levelScaleLevel++;
			UpdateLevelScaling();
			return true;
		}
		return false;
	}
	public float GetLevelScaleAddValue(){
		if(!CanLevelUP()){
			return -1f;
		}
		switch(LevelScale){
			case LevelScale.ADD:
				return this.BaseValue + LevelScaleValues[_levelScaleLevel];
			case LevelScale.MULTIPLY:
				return this.BaseValue * LevelScaleValues[_levelScaleLevel];
			case LevelScale.SUBTRACT:
				return this.BaseValue - LevelScaleValues[_levelScaleLevel];
			default:
				return -1f;
		}
	}
	public void UpdateLevelScaling(){
		switch(LevelScale){
			case LevelScale.ADD:
				this.BaseValue += LevelScaleValues[_levelScaleLevel-1];
				break;
			case LevelScale.MULTIPLY:
				this.BaseValue *= LevelScaleValues[_levelScaleLevel-1];
				break;
			case LevelScale.SUBTRACT:
				this.BaseValue -= LevelScaleValues[_levelScaleLevel-1];
				break;										
		}
		OnValueChanged();
	}
	public void AddBase(float value){
		BaseValue += value;
		OnValueChanged();
	}
	public bool hasModifier(StatModifier mod){
		return Modifiers.Contains(mod);
	}
	public void AddModifier(StatModifier mod)
	{	
		Debug.Log("@@@@ NEW ADDING MODIFIER @@@@");
		Modifiers.Add(mod);
		//Modifiers.Add(mod);
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				//percentage 0-1
				this._baseFactor = mod.Value * this._baseFactor;
				break;
			case StatModifierAddType.VALUE_BASE_PERMANENT:
				this.BaseValue += mod.Value;
				break;
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				/*float current = this.Value; */
				this._factor = mod.Value * this._factor;
				break;
			case StatModifierAddType.VALUE_MODIFIED:
				this.Value = _value + mod.Value;
				break;
		}
		OnValueChanged();	
	}
	public void RemoveModifier(StatModifier mod)
	{	
		Modifiers.Remove(mod);
		//Modifiers.Remove(mod);
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				//percentage 0-1
				this._baseFactor = this._baseFactor / mod.Value ;
				break;
			case StatModifierAddType.VALUE_BASE_PERMANENT:
				this.BaseValue -= mod.Value;
				break;
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				this._factor = this._factor / mod.Value;
				break;
			case StatModifierAddType.VALUE_MODIFIED:
				this.Value = _value - mod.Value;
				break;
		}
		OnValueChanged();
	}
	public void RemoveAllModifiers()
	{
		foreach(StatModifier mod in Modifiers){
			RemoveModifier(mod);
		}
	}
}
public enum LevelScale {
	NONE,
	ADD,
	MULTIPLY,
	SUBTRACT
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
	Effect,
	//ENDTOWER
	//ENEMY
	Speed, 
	MaxHealth,
	Reward
	//ENDENEMY
}