using UnityEngine;
using System.Collections;

public class MonkeyAnimationController : MonoBehaviour 
{
	public Animation myAnimation;
	private PlayerController playerController;
	private WeaponController weaponController;
	public PlayerSoundManager playerSoundManager;

	public float animationSpeed;

	public int animationState = 0; // 0=Idle, 1=Walk, 



	void Start()
	{
		playerController = transform.parent.GetComponent<PlayerController>();
		weaponController = transform.parent.GetComponent<WeaponController>();
		myAnimation = GetComponent<Animation>();
		myAnimation.Play("Idle", PlayMode.StopAll);
	}

	void Update()
	{
		if(playerController.state == animationState)
			return;

		if(playerController.state == 0)
		{
			animationState = 0;
			myAnimation.CrossFade("Idle", 0.2f);
		}
		else if(playerController.state == 1)
		{
			animationState = 1;
			myAnimation["Walk"].speed = 1.0f;
			myAnimation.CrossFade("Walk", 0.2f);
		}
		else if(playerController.state == 5)
		{
			animationState = 5;
			myAnimation["Walk"].speed = -1.0f;
			myAnimation.CrossFade("Walk", 0.2f);
		}
	}

	public void prepLayersForReload()
	{
		animation["Idle"].layer = 1;
		animation["Walk"].layer = 1;
		animation["PutAwayWeapon"].layer = 2;
		animation["AKReload"].layer = 2;
	}



	//Animation Event Functions:
	public void letGoOfMagazine()
	{
		weaponController.letGoOfMagazine();
		playMagIn();
	}

	public void grabAKMagazine()
	{
		weaponController.grabAKMagazine();
	}

	public void doneReloading()
	{
		weaponController.doneReloadAnimation();
	}

	public void switchWeaponSprites()
	{
		weaponController.changeWeaponSprite();
	}

	public void playMagIn()
	{
		playerSoundManager.playMagIn();
	}

	public void playMagOut()
	{
		playerSoundManager.playMagOut();
	}
}
