using UnityEngine;
using System.Collections;

public class PlayerSoundManager : MonoBehaviour 
{
	public AudioClip clipAKFire;
	
	private AudioSource audioAKFire;
	
	AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) 
	{
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	
	void Start()
	{
		audioAKFire = AddAudio(clipAKFire, false, false, 1.8f);
	}



	public void playAKFire()
	{
		audioAKFire.Play();
	}
}