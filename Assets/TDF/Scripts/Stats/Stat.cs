using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using UnityEngine.Serialization;
[System.Serializable]
public class Stat{
	public bool Display = true;
	private string name;
	StringBuilder displayName;
	public string Name{
		get{
			// Funktion die aus dem Stattype-String einen Namen macht
			// z.B testObject -> Test Object
			if(!String.IsNullOrEmpty(name)){
				return name;
			}
			String typeName = Type.ToString();
			displayName = new StringBuilder(typeName.Length+3);
			displayName.Append(typeName[0]);
			for (int i = 1; i < typeName.Length; i++){
				//starte beim 2. Buchstabe
				if (char.IsUpper(typeName[i]) && typeName[i - 1] != ' '){
					//wenn dieser Gross geschrieben ist füge ein Leerzeichen hinzu
					displayName.Append(' ');
				}
				displayName.Append(typeName[i]);
			}
			return name = displayName.ToString();
		}
		set{
			this.name = value;
		}
	}
	public StatType Type;
	public string Description;
	public float BaseValue;
	public float _value;
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
	private int _scaleLevel = 0;
	public float[] scaleValues;
	public List<StatModifier> Modifiers  = new List<StatModifier>();
	// Use this for initialization
	public delegate void ValueChangedEventHandler(object source, EventArgs args);
	public event ValueChangedEventHandler ValueChanged;
	bool _init = false;
	//initialisierung, setzt Startwerte
	public void Init(){
		_init = true;
		_factor = 1;
		_baseFactor = 1;
		_scaleLevel = 0;
		_value = 0;
		OnValueChanged();
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
	public bool CanLevelUP(int level = -1){
		if(scaleValues == null){
			return false;
		}
		return scaleValues.Length > _scaleLevel;
	}
	public bool LevelUP(int level = -1){
		if(CanLevelUP(level)){
			this._scaleLevel++;
			UpdateLevelScaling();
			return true;
		}
		return false;
	}
	//funktion die den Wert ausrechnet die der Stat haben würde, wenn man ihn Verbessert
	public float GetLevelScaleAddValue(){
		if(!CanLevelUP()){
			return -1f;
		}
		switch(LevelScale){
			case LevelScale.ADD:
				return this.BaseValue + scaleValues[_scaleLevel];
			case LevelScale.MULTIPLY:
				return this.BaseValue * scaleValues[_scaleLevel];
			case LevelScale.SUBTRACT:
				return this.BaseValue - scaleValues[_scaleLevel];
			case LevelScale.SET:
				return this.BaseValue = scaleValues[_scaleLevel];
			default:
				return -1f;
		}
	}
	//funtkion die den Stat enstprechend ders LevelScales verbesser 
	public void UpdateLevelScaling(){
		switch(LevelScale){
			case LevelScale.ADD:
				this.BaseValue += scaleValues[_scaleLevel-1];
				break;
			case LevelScale.MULTIPLY:
				this.BaseValue *= scaleValues[_scaleLevel-1];
				break;
			case LevelScale.SUBTRACT:
				this.BaseValue -= scaleValues[_scaleLevel-1];
				break;		
			case LevelScale.SET:
				this.BaseValue = scaleValues[_scaleLevel-1];
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
	//hinzufügen eines Modifiers
	public void AddModifier(StatModifier mod)
	{	
		//Modifier wird der Liste der aktiven Modifiers hinzugefügt
		Modifiers.Add(mod);
		//anpassen der Werte
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				this._baseFactor = mod.Value * this._baseFactor;
				break;
			case StatModifierAddType.VALUE_BASE:
				this.BaseValue += mod.Value;
				break;
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				this._factor = mod.Value * this._factor;
				break;
			case StatModifierAddType.VALUE_MODIFIED:
				this.Value = _value + mod.Value;
				break;
		}
		OnValueChanged();	
	}
	//entfernen eines Modifiers
	public void RemoveModifier(StatModifier mod)
	{	
		//der Modifier wird erstmal aus der Liste der aktiven Modifiers enfernt
		Modifiers.Remove(mod);
		//Werte werden angepasst
		switch(mod.modifierAddType){
			case StatModifierAddType.PERCENTAGE_BASE:
				this._baseFactor = this._baseFactor / mod.Value ;
				break;
			case StatModifierAddType.VALUE_BASE:
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
	SUBTRACT,
	SET
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