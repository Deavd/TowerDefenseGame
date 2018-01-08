using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetTypes { CLOSE = 0, FAR, STRONG, WEAK, NONE };
public enum TowerTypes { NORMAL, AURA, STATIC };

[RequireComponent(typeof(TowerStat)), RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class Tower : MonoBehaviour, StatHolder
{

    public  int id;
    public string displayName;
    public GameObject Button;
    public TowerTypes TowerType;
    private TowerStat _towerStat;
    public TowerStat Stats {
        set{
            _towerStat = value;
        }
        get{
            return _towerStat == null ? _towerStat = GetComponent<TowerStat>() : _towerStat;
        }
    }

    UnityEngine.AI.NavMeshObstacle navMeshObstacle;
    protected virtual void Start(){

    }
    protected virtual void Awake(){
        Stats = GetComponent<TowerStat>();
        (navMeshObstacle = GetComponent<UnityEngine.AI.NavMeshObstacle>()).enabled = false;
        this.gameObject.tag = "Tower";
    }
    public int UpgradeLevel = 0;
    public virtual bool Upgrade()
    {
        UpgradeLevel++;
        return Stats.LevelUpAll();
    }
    public virtual void Sell(){
        Destroy(this.gameObject);
    }
    public bool active = false;

    //Diese Funktion dient der Anwendung der Auraeffekte der Auratürme die schon auf der Map sind
    public virtual void Build(){
        //aktiviere den NavMeshObstace damit Gegner dem Turm ausweichen
        navMeshObstacle.enabled = true;
        //Abfrage ob es sich um einen normalen Turm also Angriffsturm handel.
        if(TowerType == TowerTypes.NORMAL){
            //Lese alle Objekte aus der Szene heraus die den Tag "Tower" haben
            GameObject[] towerObjects = GameObject.FindGameObjectsWithTag("Tower");
            foreach(GameObject towerObject in towerObjects){
                Tower t = towerObject.GetComponent<Tower>();                
                if(t.TowerType == TowerTypes.AURA){
                    float distance = (this.transform.position - towerObject.transform.position).magnitude;  
                    if(distance <= t.Stats.Range.Value){
                         //wenn es sich um einen Auraturm handle und er in Reichweite ist,
                         //werden dem Turm seine Effekte zugewiesen:
                        foreach(StatModifier mod in ((AuraTower) t).TowerModifiers){
                            if(Stats.hasStat(mod.statType)){
                                Stats.addModifierToStat(mod);
                            }
                        }
                    }
                }
                
            }
        }
        Activate();
    }
    public virtual void Activate(){
        active = true;
    }
    public void OnStatChanged(Stat stat)
    {
        
    }
}
