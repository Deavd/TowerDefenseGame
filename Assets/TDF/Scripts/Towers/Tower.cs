using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject[] missile;
    public TargetTypes targetType = TargetTypes.CLOSE;
    MapObject mapObject;
    public TowerInformation(string name, int level, int maxLevel, float[] range, float[] damage, float[] attackSpeed,
                            float[] buildTime, double[] buyPrice, double[] sellPrice, GameObject[] missile, int id)
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
        this.missile = missile;
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
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float distance = TowerInformation.range[TowerInformation.level];
        GameObject closestTarget = null;
        foreach (GameObject enemy in enemies){
            float distanceToEnemy = (this.transform.position - enemy.transform.position).magnitude;
            if(distanceToEnemy < distance){
                distance=distanceToEnemy;
                closestTarget = enemy;
            }
        }
        if(closestTarget != null){
            this.target = closestTarget;
        }
        RotateToEnemy();
    }
    private void RotateToEnemy()
    {
        if(target != null){
            Vector3 targetPos = target.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }
    public virtual void AttackEnemy()
    {
        if(target == null){return;}
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies){
            float distance = (this.transform.position - enemy.transform.position).magnitude;
            if(distance < TowerInformation.range[TowerInformation.level]){
                GameObject missile = (GameObject) Instantiate(TowerInformation.missile[0], this.transform.position, Quaternion.identity);
                missile.GetComponent<Missiles>().Shoot(target, TowerInformation.damage[TowerInformation.level]);

             Debug.Log("Shoot dist:"+distance);
                return;
            }
        }
       
    }
    public void changeTarget(TargetTypes type)
    {
        TowerInformation.targetType = type;
    }
}
