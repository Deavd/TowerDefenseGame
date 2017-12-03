using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour {
	public Color Error;
	public Color Info;
	public Color Warning;
	public GameObject TextHolder;
	private static ShowMessage _instance;
    public static ShowMessage Instance
    {
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<ShowMessage>() : _instance) == null ? _instance = new GameObject().AddComponent<ShowMessage>(): _instance;
        }
    }
	public void WriteMessageAt(string s, Vector2 pos, MessageType type = MessageType.INFO, int fontSize = 12, float time = 0.5f){
		Image image = Instantiate(TextHolder, pos, Quaternion.identity).GetComponent<Image>();
		Text text = image.transform.GetChild(0).GetComponent<Text>();
		
		image.transform.SetParent(this.transform);
		text.text = s;
		text.fontSize = fontSize;
		switch(type){
			case MessageType.ERROR:
				image.color = Error;
				break;
			case MessageType.INFO:
				image.color = Info;
				break;
			case MessageType.WARNING:
				image.color = Warning;
				break;
		}
		StartCoroutine(fadeOut(image, text, time));
	}
	IEnumerator fadeOut(Image image, Text text, float time){
		yield return new WaitForSeconds(time);
		float decrease = 1f/25f;
		float a = image.color.a;

		Color imageC = image.color;
		Color textC = text.color;
		int i = 0;
		while(a > 0.00f){
			yield return new WaitForSeconds(0.002f);
			i++;
			a -= decrease;
			imageC.a = a;
			textC.a = a;
			image.color = imageC;
			text.color = textC;
		}
		Destroy(text.gameObject);
		Destroy(image.gameObject);
	}
}
	public enum MessageType{
		ERROR,
		WARNING,
		INFO
	}