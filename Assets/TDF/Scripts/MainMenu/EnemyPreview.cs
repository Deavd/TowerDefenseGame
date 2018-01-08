using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyPreview : MonoBehaviour{

	public GameObject DeathEffect;
	public bool isDead = false;
	protected virtual void Awake () {
		this.tag = "Enemy";
		_HealthBarImage.transform.parent.transform.SetParent(GameObject.Find("UI").transform, false);
		_HealthBarImage.transform.parent.transform.position = Camera.main.WorldToScreenPoint(this.transform.position);
	}
	
	private Image _healthBarImage;
	private Image _HealthBarImage{
		set{
			_healthBarImage = value;
		}
		get{
			return _healthBarImage == null ? _healthBarImage = Instantiate(GameObject.Find("Healthbar")).transform.GetChild(0).GetComponent<Image>() : _healthBarImage;
		}
	}

	protected virtual void Update () {
		_healthBarImage.transform.parent.position = Camera.main.WorldToScreenPoint(this.transform.position+Vector3.up);
	}
	public float health = 100;
	public void ReceiveDamage(float dmg){
		if(isDead){
			return;
		}
		health -= dmg;
		_HealthBarImage.fillAmount = health/100;
		if(health <= 0){
			StartCoroutine(Die());
		}
	}
	IEnumerator Die(){
		isDead = true;
		Vector3 scale = this.gameObject.transform.localScale;
		gameObject.transform.localScale = new Vector3(0, 0, 0);
		_HealthBarImage.transform.parent.GetComponent<Image>().enabled = false;

		GameObject impacteffect = Instantiate(DeathEffect, transform.position, Quaternion.Euler(0, 0, 0));		
		
		yield return new WaitForSeconds(0.5f);
		
		Destroy(impacteffect);

		_HealthBarImage.transform.parent.GetComponent<Image>().enabled = true;
		//_HealthBarImage.enabled = true;
		this.gameObject.transform.localScale = scale;

		this.health = 100;
		_HealthBarImage.fillAmount = 1;
		isDead = false;
	}
}
