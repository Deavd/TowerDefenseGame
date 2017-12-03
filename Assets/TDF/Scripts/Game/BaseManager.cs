using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "Enemy"){
			other.GetComponent<Enemy>().Destinated();
		}
    }
}
