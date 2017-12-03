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

    public TowerTypes TowerType;
    public TargetTypes TargetType = TargetTypes.CLOSE;
    private TowerStat _towerStat;
    public TowerStat Stats {
        set{
            _towerStat = value;
        }
        get{
            return _towerStat == null ? _towerStat = GetComponent<TowerStat>() : _towerStat;
        }
    }


    protected virtual void Awake(){
        Stats = GetComponent<TowerStat>();
        GetComponent<UnityEngine.AI.NavMeshObstacle>().enabled = false;
        this.gameObject.tag = "Tower";
    }

    public virtual bool Upgrade()
    {
        return Stats.LevelUpAll();
    }
    public virtual void Sell(){
        Destroy(this.gameObject);
    }
    public bool active = false;


    public virtual void Build(){
        if(TowerType == TowerTypes.NORMAL){
            GameObject[] towerObjects = GameObject.FindGameObjectsWithTag("Tower");
            foreach(GameObject towerObject in towerObjects){
                Tower t = towerObject.GetComponent<Tower>();
                if(t.TowerType == TowerTypes.AURA){
                    float distance = (this.transform.position - towerObject.transform.position).magnitude;  
                    if(distance <= t.Stats.Range.Value){
                        foreach(StatModifier mod in ((AuraTower) t).TowerModifiers){
                            if(Stats.hasStat(mod.statType)){
                                Stats.addModifierToStat(mod);
                            }
                        }
                    }
                }
                
            }
        }
        //buildtime?
        active = true;
    }
    public void OnStatChanged(Stat stat)
    {
        
    }
}
