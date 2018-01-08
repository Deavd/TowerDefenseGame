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
        //beim Verkaufen des Turmes sollen alle Modifiers die vergeben wurden, wieder entfernt werden.
        RemoveModifiersFromTower();
        base.Sell();
    }
    //diese Funktion wird aufgerufen, wenn der Spieler den Upgrade-Button drückt
    public override bool Upgrade(){
        var b = base.Upgrade();
        AuraStats.LevelUpAll();
        UpdateModifiers();
        return b;
    }
    //diese Funktion entfernt alle Modifiers von den Türmen die dieser Turm beeinflusst
    public void RemoveModifiersFromTower()
    {
        GameObject[] towerObjects = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject towerObject in towerObjects){
            float distance = (this.transform.position - towerObject.transform.position).magnitude;  
                if(distance <= Stats.Range.Value){
                    Tower tower = towerObject.GetComponent<Tower>();
                    if(tower.TowerType == TowerTypes.NORMAL){
                        foreach(StatModifier mod in TowerModifiers){
                            //wenn der Turm in Reichweite ist und wird von werden Stats alle Modifier dieses Turmes entfernt
                            tower.Stats.removeModifierFromStat(mod);
                        }
                    }
                }
        }
    }
    //lädt alle Modifiers neu
    public void UpdateModifiers(){
        RemoveModifiersFromTower();
        TowerModifiers.Clear();
        //alle Modifiers werden erstmals entfernt und aus den AuraStats neue Modifiers erstellt
        foreach(Stat stat in AuraStats.StatDict.Values){
            StatModifier mod = new StatModifier(addType, stat.Value, stat.Type);
            TowerModifiers.Add(mod);
        }
        //Modifiers werden den Türmen wieder angefügt
        AddModifiersToTowers();
    }
    GameObject[] towerObjects;
    //wird beim Plazieren des Turmes ausgeführt
    public void AddModifiersToTowers(){
        //Fnde alle Türme in der Umgebung
        towerObjects = GameObject.FindGameObjectsWithTag("Tower");
        foreach(GameObject towerObject in towerObjects){
            float distance = (this.transform.position - towerObject.transform.position).magnitude;  
            if(distance <= Stats.Range.Value){
                Tower tower = towerObject.GetComponent<Tower>();
                //Wenn der Turm in Reichweite ist, werden ihm die Modifiers angefügt
                foreach(StatModifier statModifier in TowerModifiers){
                    if(tower.Stats.hasStat(statModifier.statType)){
                        tower.Stats.addModifierToStat(statModifier);
                    }
                }
            }
        }
    }
}
