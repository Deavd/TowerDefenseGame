using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour {
	public GameObject TowerGUI;
	public Text TowerInfo;
	public bool TowerGUIEnabled = false;
	public static UIHandler Instance
    {
        get
        {
            return (UIHandler)FindObjectOfType(typeof(UIHandler));
        }
    }
	public void LoadTowerGui(Tower t){
		string text = "<color=#00ff00ff>"+t.name + "</color>\r\n DPS: "
			+(t.Damage/t.AttackSpeed) 
			+ "\r\n Range: "+t.Range+"\r\n ";
		TowerInfo.text = text;
	}
	public void OpenGui(){

	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
