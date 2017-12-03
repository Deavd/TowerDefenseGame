using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(TowerStat)), RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class AttackTower : Tower
{
    public GameObject missile;
    public GameObject Missile{
        get{return missile;}
    }

    

    public GameObject Target;
    private float _updateTime = 0.1f; //seconds
    private float _lastUpdateTime; 
    private float _lastShootTime; 
    protected virtual void Update(){
		RotateToEnemy();
		//if(Time.time > lastShootTime + AttackSpeed /* speed */){
		/*if(Time.time > _lastShootTime + Stats.AttackSpeed.Value /* speed *//*){
			_lastShootTime = Time.time;
			Debug.Log("SHOOTING! NExt in "+ Stats.AttackSpeed.Value+"s");
			AttackEnemy();
		}*/
		if(!active || Time.time - _lastUpdateTime < _updateTime){
			return;
		}
		_lastUpdateTime = Time.time;
		TargetEnemy();        
    }
    private bool _isShooting = false;
    IEnumerator ShootCycle(){
        if(AttackEnemy()){
            _isShooting = true;
            yield return new WaitForSeconds(Stats.AttackSpeed.Value);
           _isShooting = false;
        }else{
            //is ready to shoot again;
            _isShooting = false;
        }
    }
    private void TargetEnemy()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject target = null;
        //var closestDistance = Range;
        var closestDistance = Stats.Range.Value;
        var farthestDistance = 0f;
        var minHealth = float.MaxValue;
        var maxHealth = 0f;
        foreach (GameObject enemyObject in enemyObjects){
            float distanceToEnemy = (this.transform.position - enemyObject.transform.position).magnitude;
            //if(distanceToEnemy < Range){
            if(distanceToEnemy <= Stats.Range.Value){
                float health = enemyObject.GetComponent<Enemy>().Stats.Health.Value;
                switch(TargetType){
                    case TargetTypes.CLOSE:                        
                        if(distanceToEnemy <= closestDistance){
                            closestDistance = distanceToEnemy;
                            target = enemyObject;                        
                        }
                        break;
                    case TargetTypes.FAR:
                        if(distanceToEnemy >= farthestDistance){
                            farthestDistance = distanceToEnemy;
                            target = enemyObject;                        
                        }
                        break;
                    case TargetTypes.WEAK:
                        if(health <= minHealth){
                            minHealth = health;
                            target = enemyObject;  
                        }
                        break;
                    case TargetTypes.STRONG:
                        if(health >= maxHealth){
                            maxHealth = health;
                            target = enemyObject;  
                        }
                        break;
                }
            }
        }
        if(target != null){
            if(_isShooting == false){
                StartCoroutine(ShootCycle());
            }
        }
        this.Target = target;      
    }
    //has movement -> later for buffs etc...
    private float _rotationSpeed = 10f;
    private void RotateToEnemy()
    {
        if(Target != null){
            Transform towerMain;
            if(transform.childCount == 0){
                towerMain = transform;
            }else{
                towerMain = transform.GetChild(0);
            }
            //get the direction
            Vector3 direction = Target.transform.position - towerMain.position;
            //calc the roation needed to look at the target 
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 targetPos = Target.transform.position;
            towerMain.rotation = Quaternion.Slerp(towerMain.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
            

        }
    }
    public virtual bool AttackEnemy()
    {
        if(Target == null){return false;}
        if(transform.childCount == 0){
            missile = (GameObject) Instantiate(Missile, this.transform.position, this.transform.rotation);
        }else{
            missile = (GameObject) Instantiate(Missile, this.transform.GetChild(2).position, this.transform.GetChild(0).rotation);
        }
        //missile.GetComponent<Missiles>().Shoot(Target, Damage);     
        missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value);
        return true;
    }
    public void changeTarget(TargetTypes type)
    {
        TargetType = type;
    }

}
