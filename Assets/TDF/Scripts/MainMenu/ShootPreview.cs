using System.Collections;
using System.Collections.Generic; 
using UnityEngine.AI;
using UnityEngine;

public class ShootPreview : MonoBehaviour {

	public GameObject Missile;
	public GameObject[] Enemies;
	private int iE = 0;
	public float AttackSpeed;
	bool hasTarget = false;
	void Awake(){
		
	}
	void OnMouseEnter(){
		if(hasTarget){
			return;
		}
		hasTarget = true;
		GameObject enemyObj = Enemies[iE];
		StartCoroutine(Shoot(enemyObj));		
	}
	IEnumerator Shoot(GameObject enemyObj){
		yield return new WaitForSeconds(AttackSpeed);
		EnemyPreview enemy = enemyObj.GetComponent<EnemyPreview>();
		if(!enemy.isDead){		
			GameObject missileObj = Instantiate(Missile, this.transform.GetChild(0).GetChild(0).position, this.transform.rotation);
			int dmg = Random.Range(1,99);
			missileObj.GetComponent<MissilePreview>().Shoot(enemyObj, dmg);
			if(enemy.health - dmg <= 0){
				hasTarget = false;
				iE = Enemies.Length <= iE+1 ? iE = 0 : iE+1;
			}else{
				StartCoroutine(Shoot(enemyObj));
			}
		}else{
			hasTarget = false;
		}
	}
	void Update(){
		if(hasTarget){
			RotateToEnemy();
		}
	}
	private void RotateToEnemy()
    {
		Transform towerMain;
		if(transform.childCount == 0){
			towerMain = transform;
		}else{
			towerMain = transform.GetChild(0);
		}
		//get the direction
		Vector3 direction = Enemies[iE].transform.position - towerMain.position;
		//calc the roation needed to look at the target 
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		Vector3 targetPos = Enemies[iE].transform.position;
		towerMain.rotation = Quaternion.Slerp(towerMain.rotation, lookRotation, Time.deltaTime * 10f);           

        }    
}
