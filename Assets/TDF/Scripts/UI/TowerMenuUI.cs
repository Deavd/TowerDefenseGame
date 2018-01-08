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
	public Material[] LevelMaterial;
	//Wird beim Drücken des Kaufen-Buttons aufgerufen
	public void clickBuy(){
		//wenn kein Objekt markiert ist wird abgebrochen
		if(selectedMapObj== null){return;}
		Tower t = selectedMapObj.Tower;
		float price = t.Stats.BuyPrice.GetLevelScaleAddValue();
		//wenn der Spieler nicht genug Geld hat, wird eine Fehlernachticht ausgegeben
		if(LevelManager.Instance.Money < price){
			ShowMessage.Instance.WriteMessageAt("Not enough money!", Input.mousePosition, MessageType.ERROR, 12, 0.3f);
		}else{
			//im anderen Fall wird der Turm verbessert
			if(t.Upgrade()){		
				//hier wird die Materialfarbe nach jedem Update angepasst
				Renderer[] renderComponents = t.GetComponentsInChildren<Renderer>();
				for(int i = 0; i < renderComponents.Length; i++)
				{
					Material[] materials = renderComponents[i].materials;
					for(int j = 0; j < materials.Length; j++){
						Debug.Log(materials[j].name);
						if(materials[j].name.StartsWith("MatHead")){
							materials[j] = LevelMaterial[t.UpgradeLevel-1];
						}
					}
					renderComponents[i].materials = materials;
					
				}
				LevelManager.Instance.Money -= t.Stats.BuyPrice.Value;
			}
		}
		LoadTowerGui(selectedMapObj);
	}
	public GameObject StatText;
	public GameObject StatBar;
	private Transform _statContainer;
	//wird aufgerufen, wenn ein Turm ausgewählt wird, dient der visualisierung seiner Stats
	public void LoadTowerGui(Tower t){		 
		//setzte das GUI aktiv
		TowerGUI.SetActive(true);
		foreach(Text text in _childTexts){
			if(text == null){continue;}
			//gehe durch die Objekte und setzte entsprechend ihre Werte
			switch(text.name){
				case "SELL_txt":
					//im falle des Verkaufen-Textes, wird der Preis gesetzt.
					text.text = t.Stats.SellPrice.Value + "$ SELL";
					break;
				case "UPGRADE_txt":
					//falls der Turm sich Verbessern lässt, aktiviere den Button und setzte den Preis auf seinen Upgradepreis
					float price = t.Stats.BuyPrice.GetLevelScaleAddValue();
					text.text = price == -1f ? "-" : price + "$ Upgrade";
					text.GetComponentInParent<Button>().interactable = t.Stats.CanLevelUp();
					break;
				case "INFO_txt":
					if(_statContainer == null){
						_statContainer = text.transform.GetChild(0);
					}
					foreach (Transform child in _statContainer.transform) {
						GameObject.Destroy(child.gameObject);
					}
					//zeigt den namen des Turemes and
					text.text = "<b>"+ColorString(t.displayName,"#64fdd0ff")+"</b>\r\n\r\n\r\n";
					Vector3 position = _statContainer.position;
					//zeigen die verschiedenen Stats mit Balken
					foreach(Stat stat in  t.Stats.StatDict.Values){
						//Abfrage, ob das Stat angezeigt werden soll:
						if(stat.Display){
							Transform statText = Instantiate(StatText).transform;
							statText.SetParent(_statContainer);
							statText.position = position;
							position.y -= 20;
							Transform statBar = Instantiate(StatBar).transform;
							statBar.SetParent(_statContainer);
							statBar.position = position;
							position.y -= 40;

							statText.GetComponent<Text>().text = "<b>"+ColorString(stat.Name+":   ", "#ffffff")+"</b>";

							Image bar_upgrade = statBar.GetChild(0).GetComponent<Image>();
							Image bar_current = statBar.GetChild(0).GetChild(0).GetComponent<Image>();
							
							float value = stat.Value;
							//hier wird die Bar ensprechen den Optimalwerten angepasst und angezeigt
							bar_current.fillAmount = GetPercentage(stat.Type, value);

							float scaleValue = stat.GetLevelScaleAddValue();
							if(scaleValue != -1f && t.active){
								bar_upgrade.fillAmount = GetPercentage(stat.Type, scaleValue);
							}else{
								bar_upgrade.fillAmount = 0;
							}

						}
					}		
					
					if(t is AttackTower){
						AttackTower attackTower = (AttackTower)t;
						//falls der Turm einen Effekt hat werden diese beschrieben
						if(attackTower.HasEffect){
							Transform statText = Instantiate(StatText).transform;
							statText.SetParent(_statContainer);
							statText.position = position;

							Stat effetStat = t.Stats.getStat(StatType.Effect);
							statText.GetComponent<Text>().text = String.Format(
								"This tower has a {0} effect. It's active for {1} seconds and {2} the enemy"+ 
								(attackTower.Effect.hasPeriod ? " every "+attackTower.Effect.Period+" seconds" : " once")+
								" by {3}",
									attackTower.Effect.EffectName,
									attackTower.Effect.Time,
									attackTower.Effect.EffectNameVerb,
									String.Format(
										getAddTypeName(attackTower.Effect.modifierAddType), 
										((attackTower.Effect.modifierAddType == StatModifierAddType.PERCENTAGE_BASE  || 
											attackTower.Effect.modifierAddType == StatModifierAddType.PERCENTAGE_MODIFIED) ?	
											(1f-effetStat.Value)*100 : effetStat.Value)
									)
							);
						}
					}
					
					break;
				case "TARGETTYPE_txt":
					if(t is AttackTower){
						_drop.gameObject.SetActive(true);
						text.text = ((AttackTower)t).TargetType.ToString();
						_drop.value = (int)((AttackTower)t).TargetType;
					}else{
						_drop.gameObject.SetActive(false);
					}
					break;
			}
		}
	}
	public String getAddTypeName(StatModifierAddType type){
		switch(type){
			case StatModifierAddType.PERCENTAGE_BASE:
				return "{0}%  of its base value";
			case StatModifierAddType.PERCENTAGE_MODIFIED:
				return "{0}%";
			case StatModifierAddType.VALUE_BASE:
				return "{0} of its base value";
			case StatModifierAddType.VALUE_MODIFIED:
				return "{0}";

		}
		return "";
	}
	public float GetPercentage(StatType type, float value){
		switch(type){
			case StatType.Range:
				return value/10f;
			case StatType.AttackSpeed:
				return 0.1f/value;
			case StatType.Damage:
				return value/100f;
			case StatType.BuildTime:
				return value/0.5f;
			
		}
		return 1;
	}
	public void LoadTowerGui(MapObject mapObj){
		//diese Funktion lädt den Turm und läd dann die GUI des Turmes.
		Tower t = mapObj.Tower;
		ShowRangePreview(t.Stats.Range.Value, mapObj.transform.position);
		selectedMapObj = mapObj;
		LoadTowerGui(t);
	}
	public void ChangeTargetType()
	{
		if(selectedMapObj != null && selectedMapObj.Tower is AttackTower){
			AttackTower t = (AttackTower)selectedMapObj.Tower;
			t.TargetType = (TargetTypes)Enum.Parse(typeof(TargetTypes), _drop.options[_drop.value].text);
		}
		
	}
	//Deaktiviert die TowerGui wieder
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

	//Funktion zum einzeigen der Reichweite des Turmes
	public void ShowRangePreview(float range, Vector3 pos){
		//Range wird aktiviert
		LevelManager.Instance.RangeUI.SetActive(true);
		//Position wird auf die Position des Turmes gestellt
		LevelManager.Instance.RangeUI.transform.position = pos + new Vector3(0,0.1f,0);
		//Hier wird die Range der gemäss der Reichweite skaliert
		LevelManager.Instance.RangeUI.transform.localScale = range * new Vector3(2,2,0);	
    }
	//Funktion, die die Reichweite ausblendet
    public void HideRangePreview(){
		LevelManager.Instance.RangeUI.SetActive(false);
    }
}
