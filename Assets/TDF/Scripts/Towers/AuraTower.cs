using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable, RequireComponent(typeof(AuraStat))]
public class AuraTower : Tower {
    public StatModifierAddType addType;
    public List<StatModifier> TowerModifiers;
    private AuraStat _auraStat;
    public AuraStat AuraStats {
        set{
            _auraStat = value;
        }
        get{
            return _auraStat == null ? _auraStat = GetComponent<AuraStat>() : _auraStat;
        }
    }
    protected override void Awake(){
        base.Awake();
    }
	public override void Build(){
        UpdateModifiers();
        base.Build();
    }
    public override void Sell(){
        RemoveModifiersFromTower();
        base.Sell();
    }
    public override bool Upgrade(){
        var b = base.Upgrade();
        AuraStats.LevelUpAll();
        UpdateModifiers();
        return b;
    }

    public void RemoveModifiersFromTower()
    {
        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject towerObject in towerObjects){
            float distance = (this.transform.position - towerObject.transform.position).magnitude;  
                if(distance <= Stats.Range.Value){
                    Tower tower = towerObject.GetComponent<Tower>();
                    if(tower.TowerType == TowerTypes.NORMAL){
                        foreach(StatModifier mod in TowerModifiers){
                            tower.Stats.removeModifierFromStat(mod);
                        }
                    }
                }
        }
    }

    public void UpdateModifiers(){
        RemoveModifiersFromTower();
        TowerModifiers.Clear();
        foreach(Stat stat in AuraStats.StatDict.Values){
            StatModifier mod = new StatModifier(addType, stat.Value, stat.Type);
            TowerModifiers.Add(mod);
        }
        AddModifiersToTowers();
    }
    GameObject[] towerObjects;
    public void AddModifiersToTowers(){
        towerObjects = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject towerObject in towerObjects){
            float distance = (this.transform.position - towerObject.transform.position).magnitude;  
            if(distance <= Stats.Range.Value){
                Tower tower = towerObject.GetComponent<Tower>();
                foreach(StatModifier statModifier in TowerModifiers){
                    if(tower.Stats.hasStat(statModifier.statType)){
                        tower.Stats.addModifierToStat(statModifier);
                    }
                }
            }
        }
    }
}
