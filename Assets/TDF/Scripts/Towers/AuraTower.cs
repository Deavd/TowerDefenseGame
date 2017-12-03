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
        Debug.Log("AWAKING!");
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
    public void AddModifiersToTowers(){
        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag("Tower");
        Debug.Log("- Tower building! Objects found on map: "+towerObjects.Length);
        foreach(GameObject towerObject in towerObjects){
            float distance = (this.transform.position - towerObject.transform.position).magnitude;  
            if(distance <= Stats.Range.Value){
                Debug.Log("- Tower in Range found!");
                Tower tower = towerObject.GetComponent<Tower>();
                if(tower.TowerType == TowerTypes.NORMAL){
                    Debug.Log("- IS Normal Tower!");
                    foreach(StatModifier statModifier in TowerModifiers){
                        Debug.Log("- Checking if stat is here! "+statModifier.statType);
                        if(tower.Stats.hasStat(statModifier.statType)){
                            Debug.Log("- Adding mod!");
                            tower.Stats.addModifierToStat(statModifier);
                        }
                    }
                }
            }
        }
    }
}
