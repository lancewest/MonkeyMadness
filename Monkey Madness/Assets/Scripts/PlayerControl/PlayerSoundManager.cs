using UnityEngine;
using System.Collections;

public class PlayerSoundManager : MonoBehaviour 
{
	public AudioClip clipAKFire;
	public AudioClip clipNoAmmo;
	public AudioClip clipMagIn;
	public AudioClip clipMagOut;
	public float testSpread = 1.0f;
	public float testDistance = 40.0f;
	
	private AudioSource audioAKFire;
	private AudioSource audioNoAmmo;
	private AudioSource audioMagIn;
	private AudioSource audioMagOut;
	
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
		audioNoAmmo = AddAudio(clipNoAmmo, false, false, 1.8f);
		audioMagIn = AddAudio(clipMagIn, false, false, 1.8f);
		audioMagOut = AddAudio(clipMagOut, false, false, 1.8f);

		audioAKFire.rolloffMode = AudioRolloffMode.Logarithmic;
		audioAKFire.volume = 3.0f;
		audioAKFire.maxDistance = 100.0f;
		audioNoAmmo.rolloffMode = AudioRolloffMode.Linear;
		audioNoAmmo.maxDistance = 10.0f;
		audioNoAmmo.volume = 1.0f;
		audioMagIn.rolloffMode = AudioRolloffMode.Linear;
		audioMagIn.maxDistance = 10.0f;
		audioMagIn.volume = 1.0f;
		audioMagOut.rolloffMode = AudioRolloffMode.Linear;
		audioMagOut.maxDistance = 10.0f;
		audioMagOut.volume = 1.0f;
	}



	public void playAKFire()
	{
		if (Network.isServer || Network.isClient)
			networkView.RPC("rpc_playAkFire", RPCMode.All);
		else
			audioAKFire.Play();
	}

	public void playNoAmmo()
	{
		if (Network.isServer || Network.isClient)
			networkView.RPC("rpc_playNoAmmo", RPCMode.All);
		else
			audioNoAmmo.Play();
	}

	public void playMagOut()
	{
		if (Network.isServer || Network.isClient)
			networkView.RPC("rpc_playMagOut", RPCMode.All);
		else
			audioMagIn.Play();
	}

	public void playMagIn()
	{
		if (Network.isServer || Network.isClient)
			networkView.RPC("rpc_playMagIn", RPCMode.All);
		else
			audioMagOut.Play();
	}



	[RPC]
	public void rpc_playAkFire()
	{
		audioAKFire.Play();
	}

	[RPC]
	public void rpc_playNoAmmo()
	{
		audioNoAmmo.Play();
	}

	[RPC]
	public void rpc_playMagOut()
	{
		audioMagIn.Play();
	}

	[RPC]
	public void rpc_playMagIn()
	{
		audioMagOut.Play();
	}
}