using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour 
{
	public Weapon[] weapons = new Weapon[2];
	public Sprite[] weaponSprites = new Sprite[2];
	public Weapon selectedWeapon;

	public Animation animationComponent;
	public PlayerSoundManager soundManager;
	public AimController aimController;

	public SpriteRenderer selectedWeaponSprite;
	public SpriteRenderer crosshair;

	public bool changingWeapons = false;
	public bool fireing = false;

	void Start() 
	{
		weapons [0] = new Unarmed ();
		weapons [1] = new AK47 (this, soundManager);
		selectedWeapon = weapons[0];
		selectedWeaponSprite.sprite = weaponSprites[0];
		crosshair.enabled = weapons[0].getShowCrosshair();

		weapons [1].changeAmmo(60);//testing purposes only
	}

	void Update() 
	{
		//Debug.Log ("Update says: " + fireing);

		if(Input.GetAxis("Fire") != 0 && !fireing)
			selectedWeapon.fire();
		if(Input.GetAxis("Weapon0") != 0)
			switchWeapon(0);
		else if(Input.GetAxis("Weapon1") != 0 && weapons[1].getAmmo() > 0)
			switchWeapon(1);
	}


	private void switchWeapon(int weaponIndex)
	{
		if(changingWeapons || (weapons[weaponIndex].getAmmo() <= 0 && weaponIndex != 0) || selectedWeapon == weapons[weaponIndex])
			return;

		changingWeapons = true;

		animationComponent.Blend("PutAwayWeapon");
		StartCoroutine(WaitThenTakeOutWeapon(animationComponent["PutAwayWeapon"].length, weaponIndex));
	}
	
	IEnumerator WaitThenTakeOutWeapon(float time, int weaponIndex)
	{
		yield return new WaitForSeconds(time);

		animationComponent.Stop("PutAwayWeapon");

		selectedWeapon = weapons[weaponIndex];
		selectedWeaponSprite.sprite = weaponSprites[weaponIndex];
		crosshair.enabled = weapons[weaponIndex].getShowCrosshair();
		
		animationComponent.Blend("GrabWeapon");
		StartCoroutine(WaitThenBeDoneChangingWeapons(animationComponent["GrabWeapon"].length));
	}

	IEnumerator WaitThenBeDoneChangingWeapons(float time)
	{
		yield return new WaitForSeconds(time);

		animationComponent.Stop("GrabWeapon");
		changingWeapons = false;
	}

	public void fire(float recoilTime)
	{
		fireing = true;

		StartCoroutine(WaitThenBeDoneFireing(recoilTime));
	}

	IEnumerator WaitThenBeDoneFireing(float time)
	{
		yield return new WaitForSeconds(time);
		
		fireing = false;
	}

	public void recoil(float recoilAngle)
	{
		aimController.recoil(recoilAngle);
	}
}
