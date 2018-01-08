using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface StatHolder {
	void OnStatChanged(Stat stat);
}
public class StatManager : MonoBehaviour {
	//public List<Stat> TowerStats = new List<Stat>();
	[SerializeField]
	private Dictionary<StatType, Stat> _statDict;
	private StatHolder _statHolder;
	private StatHolder statHolder{
		set{
			_statHolder = value;
		}
		get{
			return _statHolder == null ? _statHolder = GetComponent<StatHolder>() : _statHolder;
		}
	}
	protected virtual void Awake(){}
	public Dictionary<StatType, Stat> StatDict {
        get {
            if (_statDict == null) {
                _statDict = new Dictionary<StatType, Stat>();
            }
            return _statDict;
        }
    }
	public bool CanLevelUp(int level = -1){
		bool state = false;
		foreach(Stat stat in StatDict.Values){
			if(stat.CanLevelUP(level)){
				state = true;
			}
		}
		return state;
	}
	public bool LevelUpAll(){
		bool state = false;
		foreach(Stat stat in StatDict.Values){
			if(stat.LevelUP()){
				state = true;
			}
		}
		return state;
	}
	public bool LevelUp(StatType type){
		Stat stat;
		if(StatDict.TryGetValue(type, out stat)){
			return(stat.LevelUP());
		}
		return false;
	}

	public bool hasStat(StatType type){
		return StatDict.ContainsKey(type);
	}
	public Stat getStat(StatType type){
		if(hasStat(type)){
			return StatDict[type];
		}
		return null;
	}
	public void addStat(StatType type, Stat stat){
		stat.Type = type;
		StatDict.Add(type, stat);
		stat.Init();
	}
	public bool addModifierToStat(StatModifier mod){
		Stat stat;
		if((stat = getStat(mod.statType))!= null){
			stat.AddModifier(mod);
			return true;
		}
		return false;
	}
	class ModValues{
		public ModValues(float time, StatModifier mod){
			isActive = false;
			activatedTime = time;
			stackedTime = mod.Time;
			modifier = mod;
		}
		public float activatedTime;
		public bool isActive;
		public StatModifier modifier;
		public float stackedTime;
	}
	List<ModValues> _activeModifierValues = new List<ModValues>();
	public bool addTimeModifierToStat(StatModifier mod){
		Stat stat;
		if((stat = getStat(mod.statType))!= null){
			ModValues values = _activeModifierValues.Find(x => x.modifier == mod);
			if(values == null){
				Debug.Log("Adding");
				values = new ModValues(Time.time, mod);
				_activeModifierValues.Add(values);
			}
			if(!values.isActive){	
				stat.AddModifier(mod);	
				values.isActive = true;
				if(mod.hasPeriod){
					StartCoroutine(ReplayEffect(stat, values));
				}else if(mod.hasTime){
					StartCoroutine(RemoveEffect(stat, values));
				}
				return true;
			}else{
				values.stackedTime = mod.Time;
			}
			return true;
		}
		return false;
	}
	IEnumerator ReplayEffect(Stat stat, ModValues values){
		if(values.isActive){
			yield return new WaitForSeconds(values.modifier.Period);
			//Debug.Log("REPLAYING: "+mod.hasPeriod);
			stat.AddModifier(values.modifier);
			StartCoroutine(ReplayEffect(stat, values));
		}else{
			Debug.Log("NO LONGER REPLAYING EFFECT!");
		}
	}
	IEnumerator RemoveEffect(Stat stat, ModValues values){
		yield return new WaitForSeconds(values.modifier.Time);
		Debug.Log("REMOVING EFFECT");	
		while(values.stackedTime > 0){		
			float deltaTime = Time.time - values.activatedTime;
			float time = values.stackedTime-deltaTime;
			values.stackedTime = 0;
			yield return new WaitForSeconds(time);
		}
		_activeModifierValues.Remove(values);
		stat.RemoveModifier(values.modifier);
		values.isActive = false;
	}
	public bool removeModifierFromStat(StatModifier mod){
		Stat stat;
		if((stat = getStat(mod.statType))!= null){
			stat.RemoveModifier(mod);
			return true;
		}
		return false;
	}
	public void onValueChanged(object source, EventArgs args){
		//Debug.Log("Event triggered");
		Stat stat = (Stat) source;
		statHolder.OnStatChanged(stat);
	}
	// Update is called once per frame
}
