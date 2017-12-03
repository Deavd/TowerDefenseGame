using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class TowerMenuUI : MonoBehaviour {

	public GameObject TowerGUI;
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
	private Dropdown _drop;
	private Text[] _childTexts;
	void Awake(){
		_childTexts = TowerGUI.GetComponentsInChildren<Text>();
		_drop = TowerGUI.GetComponentInChildren<Dropdown>();
		_drop.ClearOptions();
		_drop.AddOptions(new List<String>(Enum.GetNames(typeof(TargetTypes))));
	}
	public enum clickable {SELL, BUY};
	public void clickSell(){
		if(selectedMapObj== null){return;}
		LevelManager.Instance.Money += selectedMapObj.Tower.Stats.SellPrice.Value;
		selectedMapObj.Tower.Sell();
		selectedMapObj.isBuildable = true;
		UnloadTowerGui();		
	}
	public void clickBuy(){
		if(selectedMapObj== null){return;}
		Tower t = selectedMapObj.Tower;
		float price = t.Stats.BuyPrice.GetLevelScaleAddValue();
		if(LevelManager.Instance.Money < price){
			ShowMessage.Instance.WriteMessageAt("Not enough money!", Input.mousePosition, MessageType.ERROR, 12, 0.3f);
		}else{
			if(t.Upgrade()){			
				LevelManager.Instance.Money -= t.Stats.BuyPrice.Value;
			}
		}
		LoadTowerGui(selectedMapObj);
	}
	public void LoadTowerGui(MapObject mapObj){
		TowerGUI.SetActive(true);
		Tower t = mapObj.Tower;
		ShowRangePreview(t.Stats.Range.Value, mapObj.transform.position);
		selectedMapObj = mapObj;
		foreach(Text text in _childTexts){
			switch(text.name){
				case "SELL_txt":
					text.text = t.Stats.SellPrice.Value + "$ SELL";
					break;
				case "UPGRADE_txt":
					float price = t.Stats.BuyPrice.GetLevelScaleAddValue();
					text.text = price == -1f ? "-" : price + "$ Upgrade";
					text.GetComponentInParent<Button>().interactable = t.Stats.CanLevelUp();
					break;
				case "INFO_txt":
					
					text.text = ColorString(t.displayName,"#00ff00ff")+ System.Environment.NewLine;
					foreach(Stat stat in  t.Stats.StatDict.Values){
						if(stat.Display){
							float scaleValue = stat.GetLevelScaleAddValue();
							text.text += ColorString(stat.Name+": ", "#66ff00ff")
								+ColorString(stat.Value.ToString(), "#ff0000ff") 
								+ (scaleValue != -1f ? ColorString(" (" +scaleValue+")", "#ff5500ff") : "")
								+ System.Environment.NewLine;
						}
					}					
					break;
				case "TARGETTYPE_txt":
					if(t.TowerType == TowerTypes.NORMAL){
						_drop.gameObject.SetActive(true);
						text.text = t.TargetType.ToString();
						_drop.value = (int)t.TargetType;
					}else{
						_drop.gameObject.SetActive(false);
					}
					break;
			}
		}
	}
	public void ChangeTargetType()
	{
		if(selectedMapObj== null){return;}
		Tower t = selectedMapObj.Tower;
		t.TargetType = (TargetTypes)Enum.Parse(typeof(TargetTypes), _drop.options[_drop.value].text);
		
	}
    public void UnloadTowerGui()
    {
		HideRangePreview();
		TowerGUI.SetActive(false);
		this.selectedMapObj = null;
       	foreach(Text text in TowerGUI.GetComponentsInChildren<Text>()){
			text.text = "";
	 	}
    }
	public string ColorString(string s, Color c){
		return ColorString(s, "#"+ColorUtility.ToHtmlStringRGB(c));
	}
    public string ColorString(string s, string c){
		return "<color="+c+">"+s+"</color>";
	}
	public void OpenGui(){

	}
	public void ShowRangePreview(float range, Vector3 pos){
		LevelManager.Instance.RangeUI.SetActive(true);
		LevelManager.Instance.RangeUI.transform.position = pos + new Vector3(0,0.1f,0);
		LevelManager.Instance.RangeUI.transform.localScale = range * new Vector3(2,2,0);	
    }
    public void HideRangePreview(){
		LevelManager.Instance.RangeUI.SetActive(false);
    }
}
