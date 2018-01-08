using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour {
	private static SoundManager _instance;
	public static SoundManager Instance{
        get
        {
            return (_instance == null ? _instance = FindObjectOfType<SoundManager>() : _instance) == null ? _instance = new GameObject().AddComponent<SoundManager>(): _instance;
        }
	}
	public Sound[] Sounds;

	void Awake(){
		foreach(Sound s in Sounds){			
			s.AudioSource = this.gameObject.AddComponent<AudioSource>();
		}
	}
	public void PlaySound(string name){
		foreach(Sound s in Sounds){
			if(s.name.Equals(name)){
				s.AudioSource.Play();
			}
		}
	}
	[System.Serializable]
	public class Sound {
		public String name;
		public  AudioClip AudioClip;
		private AudioSource audioSource;
		public AudioSource AudioSource{
			set{
				value.clip = AudioClip;
				value.pitch = Pitch;
				value.volume = Volume;				
				audioSource = value;
			}
			get{
				return audioSource;
			}
		}
		[Range(0f,3f)]
		public float Volume;
		[Range(-3f,3)]
		public float Pitch;
	}
}
