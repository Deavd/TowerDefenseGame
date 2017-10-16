using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TowerMenuUI : MonoBehaviour {
    public int segmentes = 5000;
    [Range(0,5)]
    public float xradius = 5;
    [Range(0,5)]
    public float yradius = 5;
    LineRenderer line;
	void Start ()
    {
		line = gameObject.GetComponent<LineRenderer>();

		line.positionCount = segmentes+2;
		CreatePoints ();
    }
	public GameObject TowerGUI;
	public Text TowerInfo;
	public bool TowerGUIEnabled = false;
	public MapObject selectedMapObj;
	public static TowerMenuUI Instance
    {
        get
        {
            return (TowerMenuUI)FindObjectOfType(typeof(TowerMenuUI));
        }
    }
	void Update () {
		if(selectedMapObj != null){
			//DrawRange();
		}
	}
    void CreatePoints ()
    {
        float x;
        float y;
        float z;
        float angle =0f;

        for (int i = 0; i < (segmentes + 1); i+=2)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

           line.SetPosition (i,new Vector3(x,5,z) );
			line.SetPosition (i+1,new Vector3(0,5,0) );
            angle += (360f / segmentes *2.1f);
        }
    }
	public enum clickable {SELL, BUY};
	public void clickSell(){
		if(selectedMapObj== null){return;}
		LevelHandler.Instance.Money += selectedMapObj.tower.GetComponent<Tower>().SellPrice;
		Destroy(selectedMapObj.tower);
		selectedMapObj.isBuildable = true;
		UnloadTowerGui();
	}
	public void clickBuy(){
		if(selectedMapObj== null){return;}
		Tower t = selectedMapObj.tower.GetComponent<Tower>();
		if(t.Upgrade()){			
			LevelHandler.Instance.Money -= t.BuyPrice;
		}
		LoadTowerGui(selectedMapObj);
	}
	public void LoadTowerGui(MapObject mapObj){
		Tower t = mapObj.tower.GetComponent<Tower>();
		selectedMapObj = mapObj;
		foreach(Text text in TowerGUI.GetComponentsInChildren<Text>()){
			switch(text.name){
				case "SELL_txt":
					text.text = t.SellPrice + "$ SELL";
					break;
				case "UPGRADE_txt":
					text.text = t.BuyPrice + "$ Upgrade";
					break;
				case "INFO_txt":
					text.text = ColorString(t.displayName,"#00ff00ff")+ System.Environment.NewLine
					+ColorString("DPS: ","#66ff00ff")+ColorString((t.Damage/t.AttackSpeed).ToString(), "#ff0000ff") + System.Environment.NewLine
					+ColorString("Range: ","#66ff00ff")+ColorString(t.Range.ToString(), "#ff0000ff") + System.Environment.NewLine
					+ColorString("Level: ","#66ff00ff")+ColorString((t.level+1).ToString(), "#ff0000ff") + System.Environment.NewLine;
					break;
			}
		}
	}

    public void UnloadTowerGui()
    {
		this.selectedMapObj = null;
       foreach(Text text in TowerGUI.GetComponentsInChildren<Text>()){
			text.text = "";
	   }
    }

    public string ColorString(string s, string c){
		return "<color="+c+">"+s+"</color>";
	}
	public void OpenGui(){

	}

}
