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
	public bool CanLevelUp(){
		bool state = false;
		foreach(Stat stat in StatDict.Values){
			if(stat.CanLevelUP()){
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
		stat.Init();
		StatDict.Add(type, stat);
	}
	public bool addModifierToStat(StatModifier mod){
		Stat stat;
		if((stat = getStat(mod.statType))!= null){
			stat.AddModifier(mod);
			return true;
		}
		return false;
	}
	public bool addTimeModifierToStat(StatModifier mod){
		Stat stat;
		if((stat = getStat(mod.statType))!= null){
			if(!mod.isActive){			
				stat.AddModifier(mod);	
				mod.activatedTime = Time.time;
				mod.isActive = true;
				if(mod.hasPeriod){
					//Debug.Log("MODIFIER WITH PERIOD");
					StartCoroutine(ReplayEffect(stat, mod));
				}else if(mod.hasTime){
					//StatDict[type] = stat;
					StartCoroutine(RemoveEffect(stat, mod));
				}
				return true;
			}
			mod.stackedTime = mod.Time;
			return true;
		}
		return false;
	}
	IEnumerator ReplayEffect(Stat stat, StatModifier mod){
		if(mod.isActive){
			yield return new WaitForSeconds(mod.Period);
			//Debug.Log("REPLAYING: "+mod.hasPeriod);
			stat.AddModifier(mod);
			StartCoroutine(ReplayEffect(stat, mod));
		}else{
			Debug.Log("NO LONGER REPLAYING EFFECT!");
		}
	}
	IEnumerator RemoveEffect(Stat stat, StatModifier mod){
		yield return new WaitForSeconds(mod.Time);
		while(mod.stackedTime > 0){		
			float deltaTime = Time.time - mod.activatedTime;
			float time = mod.stackedTime-deltaTime;
			mod.stackedTime = 0;
			yield return new WaitForSeconds(time);
		}
		stat.RemoveModifier(mod);
		mod.isActive = false;
	}
	IEnumerator modtester(StatModifier mod){
		Debug.Log(mod.Period);
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(modtester(mod));
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
