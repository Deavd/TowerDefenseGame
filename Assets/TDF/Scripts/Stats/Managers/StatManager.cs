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
	//überprüfung ob Verbesserung möglich ist
	public bool CanLevelUp(int level = -1){
		bool state = false;
		foreach(Stat stat in StatDict.Values){
			//gehe jede Stat durch und schaue ob mindestens eins Verbesserbar ist
			if(stat.CanLevelUP(level)){
				state = true;
			}
		}
		return state;
	}
	public bool LevelUpAll(){
		//Levle alle stats hoch, die dieser StatManager besitzt
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
		//gibt zurück ob dieser StatManager diesen Stat besitzt
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
	//hinzufügen eines Modifiers zu einem Stat
	public bool addTimeModifierToStat(StatModifier mod){
		Stat stat;
		//zuerst wird geschaut, ob solch ein Stat überhaupt existiert
		if((stat = getStat(mod.statType))!= null){
			//fals ja, werden allenfalls schon nach vorhandenen Modifiern gesucht
			ModValues values = _activeModifierValues.Find(x => x.modifier == mod);
			if(values == null){
				//Falls diese Werte leer sind, werden neue erstellt
				values = new ModValues(Time.time, mod);
				_activeModifierValues.Add(values);
			}
			if(!values.isActive){	
				//wenn der Wert des Modifiers nicht aktiv ist, wird er dem Stat zugefügt
				stat.AddModifier(mod);	
				values.isActive = true;
				if(mod.hasPeriod){
					//starte eine periodische Wiederholung des Effektes
					StartCoroutine(ReplayEffect(stat, values));
				}else if(mod.hasTime){
					//starte das entfernen des Effektes
					StartCoroutine(RemoveEffect(stat, values));
				}
				return true;
			}else{
				//wenn der Wert des Modifiers schon aktiv, wird die Zeit zurückgesetzt bis er wieder entfernt wird
				values.stackedTime = mod.Time;
			}
			return true;
		}
		return false;
	}
	IEnumerator ReplayEffect(Stat stat, ModValues values){
		if(values.isActive){
			yield return new WaitForSeconds(values.modifier.Period);
			//füge nach jeder Periode den Effekt erneut hinzu
			stat.AddModifier(values.modifier);
			StartCoroutine(ReplayEffect(stat, values));
		}else{
		}
	}
	IEnumerator RemoveEffect(Stat stat, ModValues values){
		yield return new WaitForSeconds(values.modifier.Time);
		while(values.stackedTime > 0){		
			//fals die Zeit des Effektes zurückgesetzt wurde, wird hier nochmals gewartet.
			float deltaTime = Time.time - values.activatedTime;
			float time = values.stackedTime-deltaTime;
			values.stackedTime = 0;
			yield return new WaitForSeconds(time);
		}
		//entfernen des Effektes bzw Modifiers
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
		Stat stat = (Stat) source;
		statHolder.OnStatChanged(stat);
	}
}
