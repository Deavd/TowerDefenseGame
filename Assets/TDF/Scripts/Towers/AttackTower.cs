using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(TowerStat)), RequireComponent(typeof(UnityEngine.AI.NavMeshObstacle))]
public class AttackTower : Tower
{
    public bool HasEffect;
    public TargetTypes TargetType = TargetTypes.CLOSE;
    public StatModifier Effect;
    public Origin Origin;
    public GameObject missile;
    public GameObject Missile{
        get{return missile;}
    }


    public GameObject Target;
    /*private AudioSource ShootSound;
    protected override void Start(){
        ShootSound = GetComponent<AudioSource>();
    }*/
    private float _updateTime = 0.01f; //seconds
    private float _lastUpdateTime; 
    private float _lastShootTime; 
    protected virtual void Update(){
		RotateToEnemy();
    }
    public override void Activate(){
        base.Activate();
        StartCoroutine(TargetEnemy());
    }
    public bool IsShooting = false;
    IEnumerator ShootCycle(){
        if(AttackEnemy()){
            //ShootSound.Play();
            IsShooting = true;
            yield return new WaitForSeconds(Stats.AttackSpeed.Value);
           IsShooting = false;
        }else{
            IsShooting = false;
        }
    }
    IEnumerator TargetEnemy()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject target = null;
        var closestDistance = Stats.Range.Value;
        var farthestDistance = 0f;
        var minHealth = float.MaxValue;
        var maxHealth = 0f;
        foreach (GameObject enemyObject in enemyObjects){
            float distanceToEnemy = (this.transform.position - enemyObject.transform.position).magnitude;
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
            if(IsShooting == false){
                StartCoroutine(ShootCycle());
            }
        }
        this.Target = target;      
        yield return new WaitForSeconds(_updateTime);
        if(active){
            StartCoroutine(TargetEnemy());
        }
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
        GameObject missile = (GameObject) Instantiate(Missile, transform.GetChild(0).GetChild(0).position, this.transform.rotation);
        
        if(HasEffect){
            Effect.Value = Stats.getStat(StatType.Effect).Value;
            missile.GetComponent<Missiles>().addEffect(Effect);
        }
        missile.GetComponent<Missiles>().Shoot(Target, Stats.Damage.Value, Origin);
        return true;
    }
    public void changeTarget(TargetTypes type)
    {
        TargetType = type;
    }

}
