using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum MissleTypes { ROCKET, NORMAL, ELECTRIC };
public enum TargetTypes { LAST, FIRST, CLOSE, FARE, STRONG, WEAK };
public class TowerInformation
{
    public bool canUpgrade()
    {
        return level < maxLevel;
    }
    public Button selectButton;
    public readonly int id;
    public string name;
    public int level = 1;
    public int maxLevel = 3;
    public float[] range;
    public float[] damage;
    public float[] attackSpeed;
    public float[] buildTime;
    public double[] buyPrice;
    public double BuyPrice {get{return buyPrice[level];}}
    public double[] sellPrice;
    public MissleTypes[] missleType;
    public TargetTypes targetType = TargetTypes.CLOSE;
    MapObject mapObject;
    public TowerInformation(string name, int level, int maxLevel, float[] range, float[] damage, float[] attackSpeed,
                            float[] buildTime, double[] buyPrice, double[] sellPrice, MissleTypes[] missleType, int id)
    {
        this.name = name;
        this.level = level;
        this.maxLevel = maxLevel;
        this.range = range;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.buildTime = buildTime;
        this.buyPrice = buyPrice;
        this.sellPrice = sellPrice;
        this.missleType = missleType;
        this.id = id;
    }
    public void Upgrade()
    {
        if (canUpgrade())
        {
            level++;
        }
    }
}
public class Tower : MonoBehaviour
{
    bool active = false;
    public TowerInformation TowerInformation {set; get;}
    public void LoadInformations(TowerInformation info){
        TowerInformation = info;
    }

    float updateTime = 2.1f; //seconds
    float lastUpdateTime; 
    void Update(){
        if(!active || Time.time - lastUpdateTime < updateTime){
            return;
        }
        lastUpdateTime = Time.time;
        RotateToEnemy();
        AttackEnemy();
    }
    void OnMouseOver()
    {

    }
    public void Build(){
        //buildtime?
        active = true;
    }
    private void TargetEnemy()
    {
    }
    private void RotateToEnemy()
    {

    }
    public virtual void AttackEnemy()
    {
        Debug.Log("ATTACK");
    }
    public void changeTarget(TargetTypes type)
    {
        TowerInformation.targetType = type;
    }
}
