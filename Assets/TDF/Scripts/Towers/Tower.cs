using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetTypes { LAST, FIRST, CLOSE, FARE, STRONG, WEAK };

public class Tower : MonoBehaviour
{
    public Button selectButton;
    public readonly int id;
    public string displayName;
    public int level = 0;
    public int maxLevel = 3;
    public float[] range;
    public float Range{
        get{return range[level];}
    }
    public float[] damage;
    public float Damage{
        get{return damage[level];}
    }
    public float[] attackSpeed;
    public float AttackSpeed{
        get{return attackSpeed[level];}
    }
    public float[] buildTime;
    public float BuildTime{
        get{return buildTime[level];}
    }
    public double[] buyPrice;
    public double BuyPrice {get{return buyPrice[level];}}
    public double[] sellPrice;
    public double SellPrice{
        get{return sellPrice[level];}
    }
    public GameObject[] missile;
    public GameObject Missile{
        get{return missile[level];}
    }
    public TargetTypes targetType = TargetTypes.CLOSE;
    MapObject mapObject;
    public bool canUpgrade()
    {
        return level < maxLevel;
    }
    public void Upgrade()
    {
        if (canUpgrade())
        {
            level++;
        }
    }
    bool active = false;
    private GameObject target;
    float updateTime = 0.1f; //seconds
    float lastUpdateTime; 
    float lastShootTime; 
    void Update(){
        RotateToEnemy();
        if(Time.time > lastShootTime + AttackSpeed /* speed */){
            lastShootTime = Time.time;
            AttackEnemy();
        }
        if(!active || Time.time - lastUpdateTime < updateTime){
            return;
        }
        lastUpdateTime = Time.time;
        TargetEnemy();
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
        float distance = Range;
        GameObject closestTarget = null;
        foreach (GameObject enemy in enemies){
            float distanceToEnemy = (this.transform.position - enemy.transform.position).magnitude;
            if(distanceToEnemy < distance){
                distance=distanceToEnemy;
                closestTarget = enemy;
            }
        }
        this.target = closestTarget;        
        RotateToEnemy();
    }
    //has movement -> later for buffs etc...
    private float rotationSpeed = 150f;
    private void RotateToEnemy()
    {
        if(target != null){
            //get the direction
            Vector3 direction = target.transform.position - transform.position;
            //calc the roation needed to look at the target 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 targetPos = target.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10.1f /* speed */);
           

        }
    }
    public virtual void AttackEnemy()
    {
        if(target == null){return;}
        GameObject missile = (GameObject) Instantiate(Missile, this.transform.position, this.transform.rotation);
        missile.GetComponent<Missiles>().Shoot(target, Damage);     
    }
    public void changeTarget(TargetTypes type)
    {
        targetType = type;
    }
}
