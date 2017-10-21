using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour {
	//public List<Stat> TowerStats = new List<Stat>();
	[SerializeField]
	private Dictionary<StatType, Stat> _statDict;
	public Dictionary<StatType, Stat> StatDict {
        get {
            if (_statDict == null) {
                _statDict = new Dictionary<StatType, Stat>();
            }
            return _statDict;
        }
    }
	protected virtual void Awake () {
	}
	
	protected virtual void Start () {
		
	}
	protected virtual void Update () {
		foreach(Stat t in StatDict.Values){
			if(t.ModifiersWithExpiryTime == null){
				return;
			}
			Debug.Log("hastime");
			foreach(StatModifier mod in t.ModifiersWithExpiryTime){
				mod.Time -= Time.deltaTime;
				if(mod.Time <= 0){
					t.RemoveModifier(mod);
				}
			}
		}
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
		StatDict.Add(type, stat);
	}
	public bool addModifierToStat(StatType type, StatModifier mod){
		Stat stat;
		if((stat = getStat(type))!= null){
			stat.AddModifier(mod);
			return true;
		}
		return false;
	}
	public bool removeModifierToStat(StatType type, StatModifier mod){
		Stat stat;
		if((stat = getStat(type))!= null){
			stat.RemoveModifier(mod);
			return true;
		}
		return false;
	}
	// Update is called once per frame
}
